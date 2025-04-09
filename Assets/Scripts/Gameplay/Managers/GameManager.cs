using Cysharp.Threading.Tasks;
using Game.Gameplay.Cameras;
using Game.Gameplay.Race;
using Game.Gameplay.UI;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameManager : MonoBehaviour, IDisposable
    {
        #region Fields 

        private const float DefaultFollowPlayerDelay = 3f;  // ToDo : move this to the config later 

        private IGameplayCameraController _cameraController;
        private IRaceStartPanelUI _raceStartPanel;
        private IHorseRaceManager _raceManager;

        private CancellationTokenSource _cts;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init & Dispose

        [Inject]
        public void Construct(
            IGameplayCameraController cameraController,
            IRaceStartPanelUI raceStartPanel,
            IHorseRaceManager raceManager
            )
        {
            _cameraController = cameraController;
            _raceStartPanel = raceStartPanel;
            _raceManager = raceManager;
        }

        private void Awake()
        {
            _cts ??= new CancellationTokenSource();
            AwakeAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid AwakeAsync(CancellationToken token = default)
        {
            _cameraController.Initialize();
            _raceManager.Initialize();
            _raceStartPanel.Initialize();

            _cameraController.ActivateBackgroundCameraAnimations();
            await _raceManager.WaitForParticipantsPreparation(token);

            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            var horsesInfo = _raceManager.GetAllParticipantsInfo();

            _raceStartPanel.Activate();
            _raceStartPanel.DisplayAvailableHorsesToSelect(horsesInfo);
            _raceStartPanel.SubscribeOnHorseSelection(HorseSelectedListener);
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            if (_raceStartPanel != null)
            {
                _raceStartPanel.UnsubscribeFromHorseSelection(HorseSelectedListener);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        private void HorseSelectedListener(object sender, int ID)
        {
            StartRace(ID);
        }

        private void RaceFinishedListener(object sender)
        {

        }

        private void StartRace(int selectedHorseID)
        {
            _raceManager.StartRace(selectedHorseID);
            _cameraController.ActivateRaceCamera(true);
            _raceStartPanel.Deactivate();

            FollowPlayerHorseWithDelay();
        }

        private void FollowPlayerHorseWithDelay(float delay = DefaultFollowPlayerDelay)
        {
            if ((_cts == null) || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }

            var playerParticipant = _raceManager.GetPlayerParticipant();
            _cameraController.RaceCameraController.LookAtParticipantWithDelay(playerParticipant.transform, delay, _cts.Token);
        }

        #endregion
    }

}
