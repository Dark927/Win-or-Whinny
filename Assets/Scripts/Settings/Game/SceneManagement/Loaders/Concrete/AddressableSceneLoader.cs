﻿using System.Collections.Generic;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game.Settings.SceneManagement
{
    public class AddressableSceneLoader : IConcreteSceneLoader<AssetReference, AsyncOperationHandle<SceneInstance>>
    {
        #region Fields 

        private List<SceneInstance> _loadedAdditiveScenes;
        private List<SceneInstance> _scenesToUnload;

        private bool _needClean;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Properties

        public int LoadedScenesCount => _loadedAdditiveScenes.Count;

        #endregion


        #region Methods

        #region Init

        public AddressableSceneLoader()
        {
            _loadedAdditiveScenes = new List<SceneInstance>();
            _needClean = false;
        }

        #endregion


        /// <summary>
        /// Loads the addressable scene using Asset Reference.
        /// </summary>
        /// <param name="sceneReference">Scene to load</param>
        public AsyncOperationHandle<SceneInstance> LoadScene(AssetReference sceneReference, LoadSceneMode loadMode)
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            return loadMode switch
            {
                LoadSceneMode.Single => LoadSingleScene(sceneReference),
                LoadSceneMode.Additive => LoadAdditiveScene(sceneReference),
                _ => throw new System.NotImplementedException(),
            };
        }


        /// <summary>
        /// Unload all loaded addressable scenes.
        /// </summary>
        public void UnloadAll()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            if (!_needClean)
            {
                return;
            }

            _scenesToUnload = new List<SceneInstance>(_loadedAdditiveScenes);
            _loadedAdditiveScenes.Clear();

            _scenesToUnload.RemoveAll(scene =>
            {
                TryUnloadAdditiveScene(scene);
                return true;
            });

            _needClean = false;
        }

        private AsyncOperationHandle<SceneInstance> LoadSingleScene(AssetReference sceneReference, CancellationToken token = default)
        {
            UnloadAll();
            AsyncOperationHandle<SceneInstance> loadHandle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Single);

            loadHandle.Completed += (operationHandler) =>
            {
                if (token.IsCancellationRequested)
                {
                    TryUnloadScene(operationHandler.Result);
                }
            };

            return loadHandle;
        }

        private AsyncOperationHandle<SceneInstance> LoadAdditiveScene(AssetReference sceneReference, CancellationToken token = default)
        {
            AsyncOperationHandle<SceneInstance> loadHandle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);

            loadHandle.Completed += (operationHandler) =>
            {
                if (!token.IsCancellationRequested)
                {
                    _loadedAdditiveScenes.Add(operationHandler.Result);
                    _needClean = true;
                }
                else
                {
                    TryUnloadAdditiveScene(operationHandler.Result);
                }
            };

            return loadHandle;
        }

        private void TryUnloadAdditiveScene(SceneInstance sceneInstance)
        {
            if (!_needClean)
            {
                return;
            }

            TryUnloadScene(sceneInstance);
        }

        private AsyncOperationHandle<SceneInstance> TryUnloadScene(SceneInstance sceneInstance)
        {
            if (!sceneInstance.Scene.isLoaded)
            {
                return default;
            }

            return Addressables.UnloadSceneAsync(sceneInstance);
        }

        #endregion
    }
}