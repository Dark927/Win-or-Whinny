
using Cysharp.Threading.Tasks;
using Game.Settings.Common.Events;
using System;
using UnityEngine.SceneManagement;

namespace Game.Settings.SceneManagement
{
    public interface IGameSceneLoader
    {
        public ICustomEvent<IEventListener, EventArgs> SwitchEvent { get; }

        public UniTask LoadSceneAsync(GameSceneData sceneData, LoadSceneMode loadMode = LoadSceneMode.Single);
        public UniTask LoadMainMenuAsync(bool useLoadingScreen = true);
        public UniTask LoadLevelAsync(GameSceneData levelSceneData);
    }
}
