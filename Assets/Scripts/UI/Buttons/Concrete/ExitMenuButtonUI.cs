using Game.Settings.SceneManagement;
using Game.UI.Buttons;
using Zenject;

namespace Game.Gameplay.UI
{
    public class ExitMenuButtonUI : ButtonControllerBaseUI
    {
        #region Fields 

        private IGameSceneLoader _gameSceneLoader;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        // Note : Use DiContainer to resolve this if button does not exist in the scene initially
        [Inject]
        public void Construct(IGameSceneLoader gameSceneLoader)
        {
            _gameSceneLoader = gameSceneLoader;
        }

        #endregion

        public override void ClickEventListener()
        {
            _gameSceneLoader.LoadMainMenuAsync();
        }

        #endregion
    }
}
