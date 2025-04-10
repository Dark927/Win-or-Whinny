using Game.UI.Buttons;
using Zenject;

namespace Game.Gameplay.UI
{
    public class GameplayRestartButtonUI : ButtonControllerBaseUI
    {
        private GameManager _gameManager;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public override void ClickEventListener()
        {
            _gameManager.RestartGame();
        }
    }
}
