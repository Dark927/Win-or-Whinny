using Game.Settings.SceneManagement;
using Game.UI.Buttons;
using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;

public class PlayGameButtonUI : ButtonControllerBaseUI
{
    [SerializeField] private GameSceneData _gameSceneData;
    private IGameSceneLoader _gameSceneLoader;


    #region Methods

    [Inject]
    public void Construct(IGameSceneLoader sceneLoader)
    {
        _gameSceneLoader = sceneLoader;
    }

    public override void ClickEventListener()
    {
        _gameSceneLoader.LoadLevelAsync(_gameSceneData);
    }

    #endregion

}
