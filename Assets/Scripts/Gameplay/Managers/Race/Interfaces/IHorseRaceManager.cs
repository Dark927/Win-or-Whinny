using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using Game.Settings.Common;
using Game.Settings.GameInitialization;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.Gameplay.Race
{
    public interface IHorseRaceManager : IInitializable, IResetable
    {
        public event EventHandler<ParticipantFinishedArgs> OnAnyHorseFinished;

        public int PlayerHorseID { get; }

        public UniTask WaitForParticipantsPreparation(CancellationToken token = default);

        public Dictionary<int, HorseInfo> GetAllParticipantsInfo();
        public HorseLogic GetPlayerParticipant();

        public void StartRace(int selectedParticipantID);
        public void NotifyHorseFinished(int horseID);

    }
}
