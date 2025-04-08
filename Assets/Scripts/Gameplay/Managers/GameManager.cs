using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using Game.Gameplay.UI;
using Game.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameManager : MonoBehaviour, IDisposable
    {
        #region Fields 

        private BackgroundCamerasController _backgroundCameras;
        private IHorseInfoManagerUI _horseInfoListManagerUI;
        private IHorsesProvider _horsesProvider;
        //private IRaceController _raceController;

        private CancellationTokenSource _cts;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init & Dispose

        [Inject]
        public void Construct(
            BackgroundCamerasController backgroundCameras,
            IHorseInfoManagerUI horseInfoListManager,
            IHorsesProvider horsesProvider
            )
        {
            _backgroundCameras = backgroundCameras;
            _horseInfoListManagerUI = horseInfoListManager;
            _horsesProvider = horsesProvider;
        }

        private void Awake()
        {
            _cts ??= new CancellationTokenSource();
            AwakeAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid AwakeAsync(CancellationToken token = default)
        {
            _backgroundCameras.Activate();
            _horsesProvider.Initialize();

            await UniTask.WaitUntil(() => !_horsesProvider.HasActiveLoadings, cancellationToken : token);


            if(_cts.Token.IsCancellationRequested )
            {
                return;
            }

            Dictionary<int, HorseInfo> horsesInfoDict = new Dictionary<int, HorseInfo>();

            var allHorses = _horsesProvider.GetAllHorses();
            foreach (var horse in allHorses)
            {
                horsesInfoDict.Add(horse.ID, horse.Info);
            }

            _horseInfoListManagerUI.Initialize();
            _horseInfoListManagerUI.ReceiveHorseInfoToSelect(horsesInfoDict);
            _horseInfoListManagerUI.DisplayAvailableHorsesToSelect();
            _horseInfoListManagerUI.OnHorseSelected += HorseSelectedListener;
        }

        private void HorseSelectedListener(object sender, int ID)
        {
            var gateController = FindObjectOfType<TrackStartGatesController>();

            CustomLogger.Log("selected horse ID -> " + ID);
            gateController.Open();
            CustomLogger.Log("gates are opened (testing)");
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _horseInfoListManagerUI.OnHorseSelected -= HorseSelectedListener;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion


        #endregion
    }

}
