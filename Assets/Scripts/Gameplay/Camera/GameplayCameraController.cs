
using UnityEngine;

namespace Game.Gameplay.Cameras
{
    public class GameplayCameraController : MonoBehaviour, IGameplayCameraController
    {
        #region Fields 

        private BackgroundCamerasController _backgroundCamerasController;
        private RaceCameraController _raceCameraController;

        #endregion


        #region Properties

        public IRaceCameraController RaceCameraController => _raceCameraController;

        #endregion


        #region Methods

        public void Initialize()
        {
            _backgroundCamerasController = GetComponentInChildren<BackgroundCamerasController>(true);
            _raceCameraController = GetComponentInChildren<RaceCameraController>(true);
            _raceCameraController.Initialize();
        }

        public void ActivateBackgroundCameraAnimations()
        {
            _raceCameraController.Deactivate();
            _backgroundCamerasController.Activate();
        }

        public void ActivateRaceCamera(bool activateDefaultView = false)
        {
            _backgroundCamerasController.Deactivate();
            _raceCameraController.Activate();

            if (activateDefaultView)
            {
                _raceCameraController.LookAtStartingGates();
            }
        }

        #endregion
    }
}
