
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

        private string DefaultContainerName = "Horses_Container";
        private string DefaultHorseNamePrefix = "Horse_";

        private Dictionary<int, HorseLogic> _horsesDict;
        private Transform _container;

        private CancellationTokenSource _horseCreationCts;
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

            if (_initialHorsesData != null)
            {
                AddMultipleHorsesAsync(_initialHorsesData.HorsesDataList);
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

        public void Dispose()
        {
            if (_horseCreationCts != null)
            {
                _horseCreationCts.Cancel();
                _horseCreationCts.Dispose();
                _horseCreationCts = null;
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

        public async UniTask AddHorseAsync(MainHorseData horseData)
        {
            if (_horseCreationCts == null || _horseCreationCts.IsCancellationRequested)
            {
                _horseCreationCts = new CancellationTokenSource();
            }

            _activeLoadingsCount += 1;
            var createdHorseLogic = await CreateHorseAsync(horseData.HorsePrefabReference, _container, _horseCreationCts.Token);

            if (createdHorseLogic != null)
            {
                createdHorseLogic.gameObject.name = DefaultHorseNamePrefix + horseData.HorseInfo.Name;
                createdHorseLogic.Initialize(horseData.HorseInfo);
                _horsesDict.Add(createdHorseLogic.ID, createdHorseLogic);
            }
            else
            {
                CustomLogger.LogWarning($" # Can not add a new horse : {nameof(HorseLogic)} is null! | {gameObject.name}");
            }

            _activeLoadingsCount -= 1;
        }

        public IEnumerable<UniTask> AddMultipleHorsesAsync(IEnumerable<MainHorseData> horsesDataCollection)
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (var horse in horsesDataCollection)
            {
                tasks.Add(AddHorseAsync(horse));
            }

            return tasks;
        }

        private async UniTask<HorseLogic> CreateHorseAsync(AssetReference horsePrefabReference, Transform container, CancellationToken token = default)
        {
            var loadHandle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<GameObject>(horsePrefabReference);

            await loadHandle.Task;

            if (loadHandle.Task.IsCompletedSuccessfully)
            {
                AddressableAssetsHandler.Instance.Cleaner.SubscribeOnCleaning(AddressableAssetsCleaner.CleanType.SceneSwitch, loadHandle);

                if (!token.IsCancellationRequested)
                {
                    GameObject horseObject = GameObject.Instantiate(loadHandle.Result, Vector3.zero, Quaternion.identity, container);
                    return horseObject.GetComponent<HorseLogic>();
                }
            }

            return null;
        }

        #endregion
    }
}
