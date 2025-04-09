using Cysharp.Threading.Tasks;
using Game.Settings.GameInitialization;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Gameplay.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class RaceCamera : MonoBehaviour, IInitializable, IDisposable
    {
        #region Fields 

        [SerializeField] private List<RaceCameraPoint> _viewPoints;

        private Camera _camera;
        private Dictionary<ViewType, RaceCameraPoint> _viewPointsDict;
        private CancellationTokenSource _cts;

        #endregion


        #region Methods

        public void Initialize()
        {
            _camera = GetComponent<Camera>();

            if (_viewPoints != null)
            {
                _viewPointsDict = new Dictionary<ViewType, RaceCameraPoint>();

                foreach (var point in _viewPoints)
                {
                    _viewPointsDict.TryAdd(point.LookView, point);
                }
            }
        }

        public void LookAtStartingGates()
        {
            StopParticipantFollowing();
            SwitchViewPoint(ViewType.StartingGatesView);
        }

        public void LookAtPlayerParticipant(Transform transform)
        {
            StopParticipantFollowing();   // Ensure camera is not following another participant

            SwitchViewPoint(ViewType.FollowPlayerHorse);

            _cts ??= new CancellationTokenSource();

            FollowPlayerParticipantAsync(transform, _cts.Token).Forget();
        }

        public void Dispose()
        {
            StopParticipantFollowing();
        }

        private async UniTaskVoid FollowPlayerParticipantAsync(Transform target, CancellationToken token = default)
        {
            Vector3 targetPosition = _camera.transform.position;

            // Offset to maintain camera's initial height and forward position (optional)
            Vector3 initialCameraOffset = targetPosition - target.position;
            Vector3 horseForwardPosition; 

            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

                if(token.IsCancellationRequested)
                {
                    break;
                }

                // Calculate the forward position of the horse
                horseForwardPosition = target.position + target.forward * 10f;

                targetPosition.x = horseForwardPosition.x - initialCameraOffset.x;

                // You can also add some smoothing if needed
                _camera.transform.position = targetPosition;
            }

        }

        private void SwitchViewPoint(ViewType targetView)
        {
            if (_viewPointsDict.TryGetValue(targetView, out RaceCameraPoint point))
            {
                _camera.transform.SetPositionAndRotation(point.transform.position, point.transform.rotation);
            }
        }

        private void StopParticipantFollowing()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        #endregion

    }
}
