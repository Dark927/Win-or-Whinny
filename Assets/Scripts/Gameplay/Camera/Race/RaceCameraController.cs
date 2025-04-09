
using UnityEngine;

namespace Game.Gameplay.Cameras
{

    public class RaceCameraController : DefaultCameraController, IRaceCameraController
    {

        #region Fields 

        private RaceCamera _raceCamera;

        #endregion


        #region Properties

        #endregion


        #region Methods

        public void Initialize()
        {
            _raceCamera = GetComponentInChildren<RaceCamera>();
            _raceCamera.Initialize();
        }

        public void LookAtStartingGates()
        {
            _raceCamera.LookAtStartingGates();
        }

        public void LookAtParticipant(Transform target)
        {
            _raceCamera.LookAtPlayerParticipant(target);
        }

        private void OnDestroy()
        {
            _raceCamera.Dispose();
        }

        #endregion
    }
}
