

using Game.Gameplay.UI;
using System;

namespace Game.Gameplay.Race
{
    public class ParticipantFinishedArgs : EventArgs
    {
        public int ID { get; }
        public RaceFinishedParticipantInfo ParticipantInfo { get; }

        public ParticipantFinishedArgs(int id, RaceFinishedParticipantInfo participantInfo)
        {
            ID = id;
            ParticipantInfo = participantInfo;
        }
    }
}
