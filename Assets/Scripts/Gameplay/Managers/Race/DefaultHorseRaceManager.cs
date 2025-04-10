
using Cysharp.Threading.Tasks;
using Game.Gameplay.Audio;
using Game.Gameplay.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Race
{
    public class DefaultHorseRaceManager : MonoBehaviour, IHorseRaceManager
    {
        #region Events 

        public event EventHandler<ParticipantFinishedArgs> OnAnyHorseFinished;

        #endregion


        #region Fields 

        [SerializeField] private Color _playerParticipantIndicatorColor = Color.green;      // ToDo : move this to the config later
        private const float DefaultRaceDelayAfterGatesOpen = 1f;   // ToDo : move this to the config later

        private const int UndefinedParticipantID = -999;

        private IHorsesProvider _horsesProvider;
        private IGameplayAudioManager _audioManager;

        private TrackStartGatesController _startGatesController;
        private int _selectedParticipantID;
        private Queue<int> _finishedHorsesQueue;

        private int _nextFinishPlace = 1;
        private float _startTime = 0f;

        #endregion


        #region Properties

        public int PlayerHorseID => _selectedParticipantID;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(IHorsesProvider horsesProvider, IGameplayAudioManager audioManager)
        {
            _horsesProvider = horsesProvider;
            _audioManager = audioManager;
        }

        public void Initialize()
        {
            _selectedParticipantID = UndefinedParticipantID;
            _startGatesController = GetComponentInChildren<TrackStartGatesController>();
            _finishedHorsesQueue = new Queue<int>();

            _startGatesController.Initialize();
            _horsesProvider.Initialize();
        }

        public void ResetState()
        {
            _nextFinishPlace = 1;
            _startTime = 0f;
            _selectedParticipantID = UndefinedParticipantID;
            _finishedHorsesQueue.Clear();
            _startGatesController.ResetState();
            _horsesProvider.ResetAllHorses();
        }

        #endregion

        public void StartRace(int selectedParticipantID)
        {
            _finishedHorsesQueue.Clear();

            _selectedParticipantID = selectedParticipantID;
            var participantsCollection = _horsesProvider.GetAllHorses();
            ConfigureHorsesStartingPositions(participantsCollection);

            StartRaceAsync(participantsCollection).Forget();
        }

        public HorseLogic GetPlayerParticipant()
        {
            if (_selectedParticipantID != UndefinedParticipantID)
            {
                return _horsesProvider.GetHorseByID(_selectedParticipantID);
            }

            return null;
        }

        public async UniTask WaitForParticipantsPreparation(CancellationToken token = default)
        {
            await UniTask.WaitUntil(() => !_horsesProvider.HasActiveLoadings, cancellationToken: token);
        }


        public Dictionary<int, HorseInfo> GetAllParticipantsInfo()
        {
            var participants = new Dictionary<int, HorseInfo>();

            var allHorses = _horsesProvider.GetAllHorses();
            foreach (var horse in allHorses)
            {
                participants.Add(horse.ID, horse.Info);
            }

            return participants;
        }

        public void NotifyHorseFinished(int horseID)
        {
            float raceTime = Mathf.Abs(Time.time - _startTime);

            if (!_finishedHorsesQueue.Contains(horseID))
            {
                _finishedHorsesQueue.Enqueue(horseID);

                var finishedHorse = _horsesProvider.GetHorseByID(horseID);
                finishedHorse.Stop();

                RaceFinishedParticipantInfo participantInfo = new RaceFinishedParticipantInfo(_nextFinishPlace, raceTime, finishedHorse.Info.Name);
                _nextFinishPlace += 1;
                ParticipantFinishedArgs participantFinishArgs = new ParticipantFinishedArgs(horseID, participantInfo);
                OnAnyHorseFinished?.Invoke(this, participantFinishArgs);
            }
        }

        private void ConfigureHorsesStartingPositions(IEnumerable<HorseLogic> participantsCollection)
        {
            foreach (var participant in participantsCollection)
            {
                _startGatesController.SetHorseOnStartingPoint(participant.transform);
            }
        }

        private async UniTask StartRaceAsync(IEnumerable<HorseLogic> participantsCollection)
        {
            _audioManager.PlayRaceStartSFX();
            _startGatesController.Open();
            await UniTask.WaitForSeconds(DefaultRaceDelayAfterGatesOpen);

            _audioManager.PlayRaceOST();

            foreach (var participant in participantsCollection)
            {
                participant.Indicator.ShowTextIndicator();
                participant.Run();
            }

            var playerParticipant = _horsesProvider.GetHorseByID(_selectedParticipantID);
            playerParticipant.Indicator.HighlightIndicator(_playerParticipantIndicatorColor);

            _startTime = Time.time;
        }


        #endregion
    }
}