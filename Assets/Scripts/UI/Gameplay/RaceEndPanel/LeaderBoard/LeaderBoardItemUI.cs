

using Game.Gameplay.Race;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.UI
{
    public class LeaderBoardItemUI : MonoBehaviour
    {
        [SerializeField] private Image _tintImage;
        [SerializeField] private TextMeshProUGUI _participantPlaceText;
        [SerializeField] private TextMeshProUGUI _participantNameText;
         

        public void SetParticipantInfo(RaceFinishedParticipantInfo participantInfo)
        {
            _participantNameText.text = participantInfo.ParticipantName;
            _participantPlaceText.text = participantInfo.ParticipantRacePlace.ToString("00");
        }

        public void Highlight(Color color)
        {
            _tintImage.color = color;
        }
    }
}
