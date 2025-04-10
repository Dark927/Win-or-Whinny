using Cysharp.Threading.Tasks;
using Game.Gameplay.Audio;
using Game.Gameplay.Cameras;
using Game.Gameplay.Race;
using Game.Gameplay.UI;
using Game.Settings.SceneManagement;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    // Future ToDo : 

    /// <summary>
    /// Is used to centralize gameplay level control so as not to complicate the architecture. 
    /// When the project is expanded, it must be removed (SRP violation)
    /// </summary>
    public class GameManager : MonoBehaviour, IDisposable, IInitializable
    {
        #region Fields 

        private const float CameraStartFollowPlayerDelay = 1.5f;  // ToDo : move this to the config later 

        private IGameplayCameraController _cameraController;
        private IRacePanelManagerUI _racePanelManagerUI;
        private IHorseRaceManager _raceManager;
        private IGameplayAudioManager _audioManager;

        private bool _isGameFinished = false;

        private CancellationTokenSource _cts;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init & Dispose

        [Inject]
        public void Construct(
            IGameplayCameraController cameraController,
            IRacePanelManagerUI racePanelManagerUI,
            IHorseRaceManager raceManager,
            IGameplayAudioManager audioManager
            )
        {
            _cameraController = cameraController;
            _racePanelManagerUI = racePanelManagerUI;
            _raceManager = raceManager;
            _audioManager = audioManager;
        }

        public void Initialize()
        {
            _cts ??= new CancellationTokenSource();

            _cameraController.Initialize();
            _raceManager.Initialize();
            _racePanelManagerUI.Initialize();
            _audioManager.Initialize();

            _raceManager.OnAnyHorseFinished += AnyHorseFinishedListener;
            _racePanelManagerUI.StartPanelUI.SubscribeOnHorseSelection(HorseSelectedListener);

            StartGameAsync(_cts.Token).Forget();
        }

        public void Dispose()
        {
            DisposeAllAsyncTasks();

            _audioManager.Dispose();
            _raceManager.OnAnyHorseFinished -= AnyHorseFinishedListener;

            if (_racePanelManagerUI != null)
            {
                _racePanelManagerUI.StartPanelUI.UnsubscribeFromHorseSelection(HorseSelectedListener);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void ResetAll()
        {
            _racePanelManagerUI.ResetState();
            _raceManager.ResetState();
            _isGameFinished = false;
        }

        #endregion

        public void RestartGame()
        {
            _racePanelManagerUI.EndPanelUI.Deactivate();
            DisposeAllAsyncTasks();
            ResetAll();

            ConfigureTokenSource();
            StartGameAsync(_cts.Token).Forget();
        }

        private async UniTask StartGameAsync(CancellationToken token = default)
        {
            _audioManager.FadeOutSFX();
            _audioManager.PlayMenuOST();

            _cameraController.ActivateBackgroundCameraAnimations();
            await _raceManager.WaitForParticipantsPreparation(token);

            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            var horsesInfo = _raceManager.GetAllParticipantsInfo();

            _racePanelManagerUI.StartPanelUI.Activate();
            _racePanelManagerUI.StartPanelUI.DisplayAvailableHorsesToSelect(horsesInfo);
        }

        private void HorseSelectedListener(object sender, int ID)
        {
            StartRace(ID);
        }

        private void AnyHorseFinishedListener(object sender, ParticipantFinishedArgs participantFinishArgs)
        {
            _racePanelManagerUI.EndPanelUI.AddLeaderBoardParticipantInfo(participantFinishArgs.ID, participantFinishArgs.ParticipantInfo);

            if (!_isGameFinished)
            {
                if (participantFinishArgs.ID == _raceManager.PlayerHorseID)
                {
                    _racePanelManagerUI.EndPanelUI.UpdateLeaderBoard();
                    DisplayGameEndPanel(participantFinishArgs.ParticipantInfo);
                    _isGameFinished = true;
                }
            }
            else
            {
                _racePanelManagerUI.EndPanelUI.UpdateLeaderBoard();
            }
        }

        // ToDo : move this to another components in the future (UI management).
        private void DisplayGameEndPanel(RaceFinishedParticipantInfo participantFinishInfo)
        {
            _racePanelManagerUI.EndPanelUI.Activate();
            _racePanelManagerUI.EndPanelUI.UpdateLeaderBoard();
            _racePanelManagerUI.EndPanelUI.SetPlayerParticipantResultInfo(participantFinishInfo.ParticipantRacePlace);
            _racePanelManagerUI.EndPanelUI.HighlightLeaderBoardParticipantInfo(_raceManager.PlayerHorseID);
        }

        private void StartRace(int selectedHorseID)
        {
            _raceManager.StartRace(selectedHorseID);
            _cameraController.ActivateRaceCamera(true);
            _racePanelManagerUI.StartPanelUI.Deactivate();

            FollowPlayerHorseWithDelay();
        }

        private void FollowPlayerHorseWithDelay(float delay = CameraStartFollowPlayerDelay)
        {
            ConfigureTokenSource();

            var playerParticipant = _raceManager.GetPlayerParticipant();
            _cameraController.RaceCameraController.LookAtParticipantWithDelay(playerParticipant.transform, delay, _cts.Token);
        }


        #region Async tasks control

        private void ConfigureTokenSource()
        {
            if ((_cts == null) || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }
        }

        private void DisposeAllAsyncTasks()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        #endregion

        #endregion
    }

}
