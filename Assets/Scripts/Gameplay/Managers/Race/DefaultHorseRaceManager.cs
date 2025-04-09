
using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using Game.Gameplay.UI;
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

        private const float DefaultRaceDelayAfterGatesOpen = 1f;   // ToDo : move this to the config later
        private const int UndefinedParticipantID = -999;

        private IHorsesProvider _horsesProvider;
        private TrackStartGatesController _startGatesController;
        private int _selectedParticipantID;
        private Queue<int> _finishedHorsesQueue;

        private int _nextFinishPlace = 1;

        #endregion


        #region Properties

        public int PlayerHorseID => _selectedParticipantID;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(IHorsesProvider horsesProvider)
        {
            _horsesProvider = horsesProvider;
        }

        public void Initialize()
        {
            _selectedParticipantID = UndefinedParticipantID;
            _startGatesController = GetComponentInChildren<TrackStartGatesController>();
            _finishedHorsesQueue = new Queue<int>();

            _startGatesController.Initialize();
            _horsesProvider.Initialize();
        }

        #endregion

        public void StartRace(int selectedParticipantID)
        {
            _finishedHorsesQueue.Clear();

            _selectedParticipantID = selectedParticipantID;
            var participantsCollection = _horsesProvider.GetAllHorses();
            ConfigureHorsesStartingPositions(participantsCollection);

            StartRaceAsync(participantsCollection).Forget(); // Todo : continue 
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
            if (!_finishedHorsesQueue.Contains(horseID))
            {
                _finishedHorsesQueue.Enqueue(horseID);

                var finishedHorse = _horsesProvider.GetHorseByID(horseID);
                finishedHorse.Stop();

                RaceFinishedParticipantInfo participantInfo = new RaceFinishedParticipantInfo(_nextFinishPlace, finishedHorse.Info.Name);
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
            _startGatesController.Open();
            await UniTask.WaitForSeconds(DefaultRaceDelayAfterGatesOpen);

            foreach (var participant in participantsCollection)
            {
                participant.Run();
            }
        }


        #endregion
    }
}