
using Zenject;

namespace Game.Gameplay.Settings
{
    public class CameraInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<BackgroundCamerasController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}
