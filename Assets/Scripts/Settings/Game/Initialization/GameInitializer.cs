using Cysharp.Threading.Tasks;
using Game.Settings.SceneManagement;
using Game.Utilities.Logging;
using UnityEngine;
using Zenject;

namespace Game.Settings.GameInitialization
{
    public class GameInitializer : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private GameSceneData _globalSceneData;
        private IGameSceneLoader _gameSceneLoader;

        #endregion


        #region Methods

        [Inject]
        public void Construct(IGameSceneLoader gameSceneLoader)
        {
            _gameSceneLoader = gameSceneLoader;
        }

        private void Awake()
        {
            if (DoFatalErrorsExist())
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
            }
        }


        private bool DoFatalErrorsExist()
        {
            bool fatalErrorsExist = false;

            if (_globalSceneData == null)
            {
                fatalErrorsExist = true;
                CustomLogger.LogComponentIsNull(gameObject.name, $"{nameof(GameSceneData)} : {nameof(_globalSceneData)}");
            }

            if (_gameSceneLoader == null)
            {
                fatalErrorsExist = true;
                CustomLogger.LogComponentIsNull(gameObject.name, $"{nameof(IGameSceneLoader)} : {nameof(_gameSceneLoader)}");
            }

            return fatalErrorsExist;
        }


        private void Start()
        {
            _gameSceneLoader
                .LoadSceneAsync(_globalSceneData, UnityEngine.SceneManagement.LoadSceneMode.Single)
                .ContinueWith(() => _gameSceneLoader.LoadMainMenuAsync(true))
                .Forget();
        }

        #endregion
    }
}
