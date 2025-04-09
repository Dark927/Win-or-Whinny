

using Cysharp.Threading.Tasks;
using Game.Settings.GameInitialization;
using System.Threading;
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
        public UniTask LookAtParticipantWithDelay(Transform target, float delay, CancellationToken token = default);
    }
}
