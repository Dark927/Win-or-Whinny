using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Gameplay.Race;

namespace Game.Gameplay.UI
{
    public class DefaultLeaderBoardUI : MonoBehaviour, ILeaderBoardUI
    {
        #region Fields 


        [Header("Main")]
        [SerializeField] private GameObject _boardItemPrefab;

        [Header("Scroll - Settings")]
        [SerializeField] private ScrollRect _scrollRect;

        [Space]
        [Header("Scroll - Settings")]
        [SerializeField] private Color _highlightColor = Color.green;


        private Dictionary<int, LeaderBoardItemUI> _participantInfoItems;
        private Dictionary<int, RaceFinishedParticipantInfo> _newParticipantsInfo;

        private VerticalLayoutGroup _verticalLayout;
        private RectTransform _contentRectTransform;

        private Vector2 _elementSize;

        #endregion


        #region Methods 

        public void Initialize()
        {
            _verticalLayout = GetComponentInChildren<VerticalLayoutGroup>();
            _scrollRect.horizontalNormalizedPosition = 0;
            _scrollRect.verticalNormalizedPosition = 0;

            _participantInfoItems = new();
            _newParticipantsInfo = new();

            RectTransform elementRectTransform = _boardItemPrefab.GetComponent<RectTransform>();
            _contentRectTransform = _verticalLayout.GetComponent<RectTransform>();
            _elementSize.x = elementRectTransform.sizeDelta.x;
            _elementSize.y = elementRectTransform.sizeDelta.y;
        }

        public void DisplayAvailableInfo()
        {
            if ((_participantInfoItems == null) || (_participantInfoItems.Count() == 0))
            {
                return;
            }

            UpdateLeaderBoard();
        }

        public void AddParticipantInfo(int ID, RaceFinishedParticipantInfo participantInfo)
        {
            _newParticipantsInfo.Add(ID, participantInfo);
        }

        public void UpdateLeaderBoard()
        {
            ResizeContentForScroll(_verticalLayout, _contentRectTransform, _participantInfoItems.Count(), _elementSize);
            UpdateLeaderBoard(_newParticipantsInfo);
            _newParticipantsInfo.Clear();
        }

        public void HighlightParticipant(int ID)
        {
            if (_participantInfoItems.TryGetValue(ID, out var leaderBoardItem))
            {
                leaderBoardItem.Highlight(_highlightColor);
            }
        }

        private void ResizeContentForScroll(VerticalLayoutGroup layoutGroup, RectTransform layoutTransform, int itemsCount, Vector2 elementSize)
        {
            int totalElements = itemsCount;

            float spacing = layoutGroup != null ? layoutGroup.spacing : 0f;
            float paddingLeft = layoutGroup != null ? layoutGroup.padding.left : 0f;
            float paddingRight = layoutGroup != null ? layoutGroup.padding.right : 0f;
            float paddingTop = layoutGroup != null ? layoutGroup.padding.top : 0f;
            float paddingBottom = layoutGroup != null ? layoutGroup.padding.bottom : 0f;

            float elementWidth = elementSize.x;
            float elementHeight = elementSize.y;

            float totalHeight = (elementHeight * totalElements) + (spacing * (totalElements - 1)) + paddingTop + paddingBottom;
            float totalWidth = elementWidth + paddingLeft + paddingRight;

            Vector2 newSize = new Vector2(totalWidth, totalHeight);
            layoutTransform.sizeDelta = newSize;
        }

        private void UpdateLeaderBoard(Dictionary<int, RaceFinishedParticipantInfo> participantsInfo)
        {
            foreach (var participant in participantsInfo)
            {
                GameObject leaderBoardItemObject = Instantiate(_boardItemPrefab, _verticalLayout.transform);
                LeaderBoardItemUI leaderBoardItem = leaderBoardItemObject.GetComponent<LeaderBoardItemUI>();
                leaderBoardItem.SetParticipantInfo(participant.Value);
                _participantInfoItems.Add(participant.Key, leaderBoardItem);
            }
        }


        #endregion
    }
}