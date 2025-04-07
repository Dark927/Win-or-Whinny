
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.Settings.SceneManagement
{
    public interface IGameSceneLoader
    {
        public UniTask LoadSceneAsync(GameSceneData sceneData, LoadSceneMode loadMode = LoadSceneMode.Single);
        public UniTask LoadMainMenuAsync(bool useLoadingScreen = true);

    }
}
