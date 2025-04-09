using Cysharp.Threading.Tasks;
using Game.Gameplay.Cameras;
using Game.Gameplay.Entities;
using Game.Gameplay.Race;
using Game.Gameplay.UI;
using Game.Utilities.Logging;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameManager : MonoBehaviour, IDisposable
    {
        #region Fields 

        private IGameplayCameraController _cameraController;
        private IRaceStartPanelUI _raceStartPanel;
        private IHorseRaceManager _raceManager;

        private CancellationTokenSource _cts;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init & Dispose

        [Inject]
        public void Construct(
            IGameplayCameraController cameraController,
            IRaceStartPanelUI raceStartPanel,
            IHorseRaceManager raceManager
            )
        {
            _cameraController = cameraController;
            _raceStartPanel = raceStartPanel;
            _raceManager = raceManager;
        }

        private void Awake()
        {
            _cts ??= new CancellationTokenSource();
            AwakeAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid AwakeAsync(CancellationToken token = default)
        {
            _cameraController.Initialize();
            _raceManager.Initialize();
            _raceStartPanel.Initialize();

            _cameraController.ActivateBackgroundCameraAnimations();
            await _raceManager.WaitForParticipantsPreparation(token);

            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            var horsesInfo = _raceManager.GetAllParticipantsInfo();

            _raceStartPanel.Activate();
            _raceStartPanel.DisplayAvailableHorsesToSelect(horsesInfo);
            _raceStartPanel.SubscribeOnHorseSelection(HorseSelectedListener);
        }

        private void HorseSelectedListener(object sender, int ID)
        {
            CustomLogger.Log("selected horse ID -> " + ID);
            _raceManager.StartRace(ID);
            _cameraController.ActivateRaceCamera(true);
            _raceStartPanel.Deactivate();



            // ToDo : move this to another place
            if ((_cts == null) || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }

            var playerParticipant = _raceManager.GetPlayerParticipant();
            FollowPlayerParticipantWithDelay(playerParticipant, 3f, _cts.Token).Forget();
        }

        private async UniTaskVoid FollowPlayerParticipantWithDelay(HorseLogic participant, float delay, CancellationToken token = default)
        {
            await UniTask.WaitForSeconds(delay, cancellationToken: token);
            _cameraController.RaceCameraController.LookAtParticipant(participant.transform);
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _raceStartPanel.UnsubscribeFromHorseSelection(HorseSelectedListener);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion


        #endregion
    }

}
