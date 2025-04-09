using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using Game.Settings.GameInitialization;
using System.Collections.Generic;
using System.Threading;

namespace Game.Gameplay.Race
{
    public interface IHorseRaceManager : IInitializable
    {
        public UniTask WaitForParticipantsPreparation(CancellationToken token = default);

        public Dictionary<int, HorseInfo> GetAllParticipantsInfo();
        public HorseLogic GetPlayerParticipant();

        public void StartRace(int selectedParticipantID);

    }
}
