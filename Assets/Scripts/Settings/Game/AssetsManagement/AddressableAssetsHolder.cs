using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Settings.AssetsManagement
{
    public class AddressableAssetsHolder : IDisposable
    {
        private readonly Dictionary<string, (AsyncOperationHandle loadHandle, AddressableAssetsCleaner.CleanType cleanType)> _cachedAssets;
        private AddressableAssetsCleaner _cleaner;

        public AddressableAssetsHolder(AddressableAssetsCleaner cleaner)
        {
            _cachedAssets = new();
            _cleaner = cleaner;
            _cleaner.OnAssetsCleanPerformed += ListenAssetsClean;
        }

        public void Dispose()
        {
            _cleaner.OnAssetsCleanPerformed -= ListenAssetsClean;
            _cachedAssets?.Clear();
        }

        public bool TryGetCachedAsset<TResult>(AssetReference assetRef, out AsyncOperationHandle<TResult> handle)
        {
            handle = default;

            if (_cachedAssets.TryGetValue(assetRef.AssetGUID, out var cachedAssetInfo))
            {
                handle = cachedAssetInfo.loadHandle.Convert<TResult>();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove an asset reference from the LOADING collection and add it to the LOADED collection 
        /// </summary>
        /// <param name="assetRef">target asset reference</param>
        /// <param name="handle">loading handle</param>
        public void CacheAsset(AssetReference assetRef, AsyncOperationHandle handle, AddressableAssetsCleaner.CleanType cleanType)
        {
            _cachedAssets.Add(assetRef.AssetGUID, (handle, cleanType));
            _cleaner.SubscribeOnCleaning(cleanType, handle);
        }

        private void ListenAssetsClean(object sender, AddressableAssetsCleaner.CleanType cleanType)
        {
            bool RemoveCondition(KeyValuePair<string, (AsyncOperationHandle loadHandle, AddressableAssetsCleaner.CleanType cleanType)> cacheInfo)
                => (cacheInfo.Value.cleanType == cleanType) || !cacheInfo.Value.loadHandle.IsValid();

            int counter = 0;
            foreach (var targetCacheInfo in _cachedAssets.Where(c => RemoveCondition(c)).ToList())
            {
                ++counter;
                _cachedAssets.Remove(targetCacheInfo.Key);
            }
        }
    }
}

