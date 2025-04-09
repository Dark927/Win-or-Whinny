
using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Race
{
    public class DefaultHorseRaceManager : MonoBehaviour, IHorseRaceManager
    {
        #region Fields 

        private const int UndefinedParticipantID = -999;

        private IHorsesProvider _horsesProvider;
        private TrackStartGatesController _startGatesController;
        private int _selectedParticipantID;

        #endregion


        #region Properties

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

            _startGatesController.Initialize();
            _horsesProvider.Initialize();
        }

        #endregion

        public void StartRace(int selectedParticipantID)
        {
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

            await UniTask.WaitForSeconds(2);

            foreach (var participant in participantsCollection)
            {
                participant.Run();
            }
        }


        #endregion
    }
}