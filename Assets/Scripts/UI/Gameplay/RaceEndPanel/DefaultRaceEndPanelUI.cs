
using Game.Gameplay.Race;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Gameplay.UI
{
    public class DefaultRaceEndPanelUI : RacePanelBaseUI, IRaceEndPanelUI
    {
        #region Fields 

        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private TextMeshProUGUI _subtitleText;

        [Space]

        [Header("Custom results - Settings")]
        [SerializeField] private List<RaceResultSubtitleParameters> _subtitlesVariantsCollection;

        private ILeaderBoardUI _leaderBoard;

        #endregion


        #region Methods 

        #region Init 

        public void Initialize()
        {
            _leaderBoard = GetComponentInChildren<ILeaderBoardUI>();
            _leaderBoard.Initialize();
        }

        public void ResetState()
        {
            _leaderBoard.Clear();
        }

        #endregion

        public override void Activate()
        {
            base.Activate();
            _leaderBoard.DisplayAvailableInfo();
        }

        public void AddLeaderBoardParticipantInfo(int ID, RaceFinishedParticipantInfo participantInfo)
        {
            _leaderBoard.AddParticipantInfo(ID, participantInfo);
        }

        public void UpdateLeaderBoard()
        {
            _leaderBoard.UpdateLeaderBoard();
        }

        public void HighlightLeaderBoardParticipantInfo(int ID)
        {
            _leaderBoard.HighlightParticipant(ID);
        }

        public void SetPlayerParticipantResultInfo(int participantPlace)
        {
            _resultText.text = GenerateResultTitle(participantPlace);
            ConfigureResultSubtitle(_subtitleText, participantPlace);
        }

        // Note : Change panel text here or move to the config file if needed (for future updates)
        private string GenerateResultTitle(int playerParticipantPlace) => $"Your horse came in #{playerParticipantPlace}";

        // ToDo : maybe should add ordering by min participant place in the future and remove duplicates
        private void ConfigureResultSubtitle(TextMeshProUGUI subtitle, int participantPlace)
        {
            foreach (var subtitleVariant in _subtitlesVariantsCollection)
            {
                if (participantPlace <= subtitleVariant.MinParticipantPlace)
                {
                    subtitle.text = subtitleVariant.Text;
                    subtitle.color = subtitleVariant.Color;
                    break;
                }
            }
        }

        #endregion
    }
}
