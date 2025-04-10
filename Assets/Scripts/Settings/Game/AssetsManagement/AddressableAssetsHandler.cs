using Game.Abstractions;
using Game.Settings.SceneManagement;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using IInitializable = Game.Settings.GameInitialization.IInitializable;

namespace Game.Settings.AssetsManagement
{
    public class AddressableAssetsHandler : LazySingletonMonoBase<AddressableAssetsHandler>, IInitializable
    {
        #region Fields 

        private AddressableAssetsCleaner _cleaner;
        private AddressableAssetsHolder _assetsHolder;

        private IGameSceneLoader _gameSceneLoader;
        private bool _isInitialized = false;

        #endregion


        #region Properties

        public AddressableAssetsCleaner Cleaner => _cleaner;
        public AddressableAssetsHolder AssetsHolder => _assetsHolder;

        #endregion

        [Inject]
        public void Construct(IGameSceneLoader sceneLoader)
        {
            _gameSceneLoader = sceneLoader;
        }


        #region Methods

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (!_isInitialized)
            {
                DontDestroyOnLoad(this);
                InitializeCleaner();
                InitializeHolder();
                _isInitialized = true;
            }
        }

        private void InitializeCleaner()
        {
            _cleaner = new AddressableAssetsCleaner();

            if (_gameSceneLoader != null)
            {
                _gameSceneLoader.SwitchEvent.Subscribe(_cleaner);
            }
        }

        private void InitializeHolder()
        {
            _assetsHolder = new AddressableAssetsHolder(_cleaner);
        }

        #region Static

        public static bool AreAssetRefsEqual(AssetReference first, AssetReference second)
        {
            return first.RuntimeKey.ToString() == second.RuntimeKey.ToString();
        }

        public static bool IsAssetRefValid(AssetReference assetRef)
        {
            bool result = !string.IsNullOrEmpty(assetRef.AssetGUID);

#if UNITY_EDITOR
            result = result && (assetRef.editorAsset != null);
#endif

            return result;
        }

        #endregion


        /// <summary>
        /// try load an asset from the asset reference, throw exception if an asset is not valid.
        /// </summary>
        /// <typeparam name="TResult">the asset type to load</typeparam>
        /// <param name="assetRef">asset reference to load</param>
        /// <returns>loaded asset</returns>
        /// <exception cref="ArgumentException">an argument exception if the asset is not valid</exception>
        public AsyncOperationHandle<TResult> TryLoadAssetAsync<TResult>(AssetReference assetRef)
        {
            if (IsAssetRefValid(assetRef))
            {
                return LoadAssetAsync<TResult>(assetRef);
            }
            throw new ArgumentException($"Bad Asset Reference: {assetRef}");
        }

        /// <summary>
        /// load an asset from the asset reference, do not throw an exception if an asset is not valid.
        /// </summary>
        /// <typeparam name="TResult">the asset type to load</typeparam>
        /// <param name="assetRef">asset reference to load</param>
        /// <returns>loaded asset</returns>
        public AsyncOperationHandle<TResult> LoadAssetAsync<TResult>(AssetReference assetRef)
        {
            return Addressables.LoadAssetAsync<TResult>(assetRef);
        }

        /// <summary>
        /// Load asset async and cache it in the AssetsHolder.
        /// </summary>
        /// <param name="assetRef">target asset reference to load</param>
        /// <param name="allowDuplicates">if true - loads and caches the asset even if it exists in the cache, false - returns the cached asset when possible</param>
        /// <returns>asset load handle</returns>
        public AsyncOperationHandle<TResult> LoadAssetAndCacheAsync<TResult>(AssetReference assetRef, AddressableAssetsCleaner.CleanType cleanType, bool allowDuplicates = false)
        {
            if (!allowDuplicates && AssetsHolder.TryGetCachedAsset<TResult>(assetRef, out var handle))
            {
                return handle;
            }

            AsyncOperationHandle<TResult> loadHandle = LoadAssetAsync<TResult>(assetRef);
            AssetsHolder.CacheAsset(assetRef, loadHandle, cleanType);

            return loadHandle;
        }


        // - Unload 

        public void TryUnloadAsset(AsyncOperationHandle handle)
        {
            Cleaner.TryUnloadAsset(handle);
        }

        public void UnloadAsset(AsyncOperationHandle handle)
        {
            Cleaner.UnloadAsset(handle);
        }

        public void UnloadAssetInstance(GameObject instance)
        {
            Cleaner.UnloadAssetInstance(instance);
        }

        public AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetRef)
        {
            return Addressables.InstantiateAsync(assetRef);
        }

        private void OnDestroy()
        {
            AssetsHolder?.Dispose();

        }

        #endregion
    }
}
