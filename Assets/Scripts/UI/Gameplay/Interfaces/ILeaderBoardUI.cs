
using Game.Gameplay.Race;
using Game.Settings.GameInitialization;

namespace Game.Gameplay.UI
{
    public interface ILeaderBoardUI : IInitializable
    {
        public void AddParticipantInfo(int ID, RaceFinishedParticipantInfo participantInfo);
        public void DisplayAvailableInfo();
        public void UpdateLeaderBoard();
        public void HighlightParticipant(int ID);
        public void Clear();
    }
}
