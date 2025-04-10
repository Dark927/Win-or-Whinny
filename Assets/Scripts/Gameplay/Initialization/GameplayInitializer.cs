using Cysharp.Threading.Tasks;
using Game.Settings.SceneManagement;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Settings
{
    public class GameplayInitializer : MonoBehaviour
    {
        #region Fields 

        private IGameSceneLoader _gameSceneLoader;
        private GameManager _gameManager;
        private CancellationTokenSource _cts;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(IGameSceneLoader gameSceneLoader, GameManager gameManager)
        {
            _gameSceneLoader = gameSceneLoader;
            _gameManager = gameManager;
        }

        #endregion

        private async void Awake()
        {
            _cts = new CancellationTokenSource();
            await WaitUntilSceneLoadingFinished(_cts.Token);

            if(!_cts.IsCancellationRequested)
            {
                _gameManager.Initialize();
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        private async UniTask WaitUntilSceneLoadingFinished(CancellationToken token)
        {
            await UniTask.WaitUntil(() => !_gameSceneLoader.IsLoading, cancellationToken: token);
        }

        #endregion
    }
}
