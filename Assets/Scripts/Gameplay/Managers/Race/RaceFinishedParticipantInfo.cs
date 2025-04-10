
namespace Game.Gameplay.Race
{
    public struct RaceFinishedParticipantInfo
    {
        private int _participantRacePlace;
        private float _participantRaceTime;
        private string _participantName;

        public int ParticipantRacePlace => _participantRacePlace;
        public float ParticipantRaceTime => _participantRaceTime;
        public string ParticipantName => _participantName;

        public RaceFinishedParticipantInfo(int participantRacePlace, float participantRaceTime, string participantName)
        {
            _participantRacePlace = participantRacePlace;
            _participantRaceTime = participantRaceTime;
            _participantName = participantName;
        }
    }
}
