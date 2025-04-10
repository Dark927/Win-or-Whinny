

using Game.Gameplay.Race;
using Game.Settings.Common;
using Game.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.UI
{
    public class LeaderBoardItemUI : MonoBehaviour, IResetable
    {
        [SerializeField] private Image _tintImage;
        [SerializeField] private TextMeshProUGUI _participantPlaceText;
        [SerializeField] private TextMeshProUGUI _participantNameText;
        [SerializeField] private TextMeshProUGUI _participantRaceTimeText;


        public void SetParticipantInfo(RaceFinishedParticipantInfo participantInfo)
        {
            _participantNameText.text = participantInfo.ParticipantName;
            _participantPlaceText.text = participantInfo.ParticipantRacePlace.ToString("00");
            _participantRaceTimeText.text = TimeConverter.ConvertToHMS(participantInfo.ParticipantRaceTime);
        }

        public void Highlight(Color color)
        {
            _tintImage.color = color;
        }

        public void ResetState()
        {
            _participantNameText.text = "0";
            _participantNameText.text = "participant name";
            _tintImage.color = Color.clear;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}
