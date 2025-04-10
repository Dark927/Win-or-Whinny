
using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using Game.Gameplay.Settings;
using Game.Settings.AssetsManagement;
using Game.Utilities.Logging;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Game.Gameplay
{
    /// <summary>
    /// Responsible for the creation and management of horses
    /// </summary>
    public class HorsesProviderMono : MonoBehaviour, IHorsesProvider
    {
        #region Fields 

        private const float DefaultPositionOffsetY = -15f;

        private string DefaultContainerName = "Horses_Container";
        private string DefaultHorseNamePrefix = "Horse_";

        private Dictionary<int, HorseLogic> _horsesDict;
        private Transform _container;

        private CancellationTokenSource _cts;
        private HorsesSetData _initialHorsesData;

        private int _activeLoadingsCount = 0;


        #endregion

        public bool HasActiveLoadings => _activeLoadingsCount > 0;


        #region Methods

        #region Init & Dispose

        [Inject]
        public void Construct(HorsesSetData initialHorsesData)
        {
            _initialHorsesData = initialHorsesData;
        }

        public void Initialize(Transform container = null)
        {
            _horsesDict = new();
            _activeLoadingsCount = 0;
            InitHorsesContainer(container);

            if (_cts == null || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }

            if (_initialHorsesData != null)
            {
                AddMultipleHorsesAsync(_initialHorsesData.HorsesDataList, _cts.Token);
            }
        }

        private void InitHorsesContainer(Transform container = null)
        {
            if (container != null)
            {
                _container = container;
            }
            else
            {
                GameObject containerObject = new GameObject(DefaultContainerName);
                containerObject.transform.SetParent(this.transform, false);
                _container = containerObject.transform;
            }
        }

        public void ResetAllHorses()
        {
            foreach (var horse in _horsesDict.Values)
            {
                horse.ResetState();
                horse.transform.position = transform.position + (Vector3.up * DefaultPositionOffsetY);
            }
        }

        public void Dispose()
        {
            if (_horsesDict != null)
            {
                foreach (var horse in _horsesDict.Values)
                {
                    horse.Dispose();
                }
            }

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        public HorseLogic GetHorseByID(int ID)
        {
            return _horsesDict[ID];
        }

        public IEnumerable<HorseLogic> GetAllHorses()
        {
            return _horsesDict.Values;
        }

        public async UniTask AddHorseAsync(MainHorseData horseData, CancellationToken token = default)
        {
            _activeLoadingsCount += 1;
            var createdHorseLogic = await CreateHorseAsync(horseData.HorsePrefabReference, _container, _cts.Token);

            if (!_cts.IsCancellationRequested)
            {
                if (createdHorseLogic != null)
                {
                    createdHorseLogic.gameObject.name = DefaultHorseNamePrefix + horseData.HorseInfo.Name;
                    createdHorseLogic.Initialize(horseData.HorseInfo);
                    createdHorseLogic.SetStats(horseData.HorseStats);
                    _horsesDict.Add(createdHorseLogic.ID, createdHorseLogic);
                }
                else
                {
                    CustomLogger.LogWarning($" # Can not add a new horse : {nameof(HorseLogic)} is null! | {gameObject.name}");
                }
            }

            _activeLoadingsCount -= 1;
        }

        public IEnumerable<UniTask> AddMultipleHorsesAsync(IEnumerable<MainHorseData> horsesDataCollection, CancellationToken token = default)
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (var horse in horsesDataCollection)
            {
                if (token.IsCancellationRequested)
                {
                    return tasks;
                }

                tasks.Add(AddHorseAsync(horse, token));
            }

            return tasks;
        }

        private async UniTask<HorseLogic> CreateHorseAsync(AssetReference horsePrefabReference, Transform container, CancellationToken token = default)
        {
            var loadHandle = AddressableAssetsHandler.Instance.LoadAssetAndCacheAsync<GameObject>(horsePrefabReference, AddressableAssetsCleaner.CleanType.SceneSwitch);

            await loadHandle.Task;

            if (loadHandle.Task.IsCompletedSuccessfully && !token.IsCancellationRequested)
            {
                GameObject horseObject = GameObject.Instantiate(loadHandle.Result, (Vector3.up * DefaultPositionOffsetY), Quaternion.identity, container);
                return horseObject.GetComponent<HorseLogic>();
            }

            return null;
        }

        #endregion
    }
}
