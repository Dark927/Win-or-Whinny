
using Game.Gameplay.Cameras;
using Zenject;

namespace Game.Gameplay.Settings
{
    public class CameraInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameplayCameraController>()
                .To<GameplayCameraController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}
