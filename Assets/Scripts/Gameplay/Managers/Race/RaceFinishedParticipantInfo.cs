using UnityEngine;

namespace Game.Gameplay.Race
{
    public struct RaceFinishedParticipantInfo
    {
        [SerializeField] private int _participantRacePlace;
        [SerializeField] private string _participantName;

        public int ParticipantRacePlace => _participantRacePlace;
        public string ParticipantName => _participantName;

        public RaceFinishedParticipantInfo(int participantRacePlace, string participantName)
        {
            _participantRacePlace = participantRacePlace;
            _participantName = participantName;
        }
    }
}
