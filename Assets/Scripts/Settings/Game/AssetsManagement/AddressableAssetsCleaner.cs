
using Game.Settings.Common.Events;
using Game.Settings.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Settings.AssetsManagement
{
    public class AddressableAssetsCleaner : IEventListener
    {
        public enum CleanType
        {
            SceneSwitch = 0,
        }

        #region Events

        public event EventHandler<CleanType> OnAssetsCleanPerformed;

        #endregion

        #region Fields 

        private Dictionary<CleanType, List<AsyncOperationHandle>> _handlesToClean;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        public AddressableAssetsCleaner()
        {
            _handlesToClean = new Dictionary<CleanType, List<AsyncOperationHandle>>();
        }

        #endregion

        public void SubscribeOnCleaning(CleanType cleanMoment, AsyncOperationHandle handleToClean)
        {
            if (!_handlesToClean.ContainsKey(cleanMoment))
            {
                _handlesToClean.Add(cleanMoment, new List<AsyncOperationHandle>());
            }

            _handlesToClean[cleanMoment].Add(handleToClean);
        }

        public void Listen(object sender, EventArgs args)
        {
            if (sender is IGameSceneLoader)
            {
                PerformConcreteCleaning(CleanType.SceneSwitch);
            }
        }

        public void TryUnloadAsset(AsyncOperationHandle handle)
        {
            if (handle.IsValid())
            {
                UnloadAsset(handle);
            }
        }

        public void UnloadAsset(AsyncOperationHandle handle)
        {
            Addressables.Release(handle);
        }

        public void UnloadAssetInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
        }

        private void PerformConcreteCleaning(CleanType cleanType)
        {
            if (!_handlesToClean.ContainsKey(cleanType))
            {
                return;
            }

            foreach (var handle in _handlesToClean[cleanType])
            {
                UnloadAsset(handle);
            }

            _handlesToClean.Clear();
            OnAssetsCleanPerformed?.Invoke(this, cleanType);
        }

        #endregion
    }
}
