

using Game.Settings.GameInitialization;

namespace Game.Gameplay.Cameras
{
    public interface IGameplayCameraController : IInitializable
    {
        public IRaceCameraController RaceCameraController { get; }

        public void ActivateBackgroundCameraAnimations();
        public void ActivateRaceCamera(bool activateDefaultView = false);
    }
}
