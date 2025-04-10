﻿

using Game.Gameplay.Race;
using Game.Settings.Common;
using Game.Settings.GameInitialization;

namespace Game.Gameplay.UI
{
    public interface IRaceEndPanelUI : IInitializable, IResetable
    {
        public void Activate();
        public void Deactivate();

        public void UpdateLeaderBoard();
        public void AddLeaderBoardParticipantInfo(int ID, RaceFinishedParticipantInfo participantInfo);
        public void HighlightLeaderBoardParticipantInfo(int ID);
        public void SetPlayerParticipantResultInfo(int participantPlace);
    }
}
