using Game.Abstractions;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class is a base class which contains what is common to all game scenes
/// </summary>

namespace Game.Settings.SceneManagement
{
    [CreateAssetMenu(fileName = "NewGameSceneData", menuName = "Game/Scene/Game Scene Data")]
    public class GameSceneData : DescriptionBaseData
    {
        public enum GameSceneType
        {
            //Playable scenes
            Menu,
            GameLevel,

            //Special scenes
            GlobalManagers,
        }

        [SerializeField] private GameSceneType _sceneType;
        [SerializeField] private AssetReference _sceneReference;

        public GameSceneType SceneType => _sceneType;
        public AssetReference SceneReference => _sceneReference;
    }
}