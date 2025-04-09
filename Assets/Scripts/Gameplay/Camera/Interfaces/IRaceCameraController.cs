

using Game.Settings.GameInitialization;
using UnityEngine;

namespace Game.Gameplay.Cameras
{
    public enum ViewType
    {
        StartingGatesView,
        FollowPlayerHorse,
    }

    public interface IRaceCameraController : IInitializable
    {
        public void LookAtStartingGates();

        public void LookAtParticipant(Transform target);
    }
}
