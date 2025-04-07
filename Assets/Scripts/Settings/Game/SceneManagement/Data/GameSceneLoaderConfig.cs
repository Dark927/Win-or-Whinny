using Game.Abstractions;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Game.Settings.SceneManagement
{
    [CreateAssetMenu(fileName = "NewSceneLoaderConfigurationData", menuName = "Game/Scene/Config/Game Scene Loader Config")]
    public class GameSceneLoaderConfig : DescriptionBaseData
    {
        [SerializeField] private AssetReference _loadingScreenAssetRef;
        [SerializeField] private GameSceneData _mainMenuScene;

        public GameSceneData MainMenuSceneData => _mainMenuScene;
        public AssetReference LoadingScreenAssetRef => _loadingScreenAssetRef;
    }
}
