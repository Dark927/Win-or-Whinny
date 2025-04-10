using Game.Gameplay.Entities;
using Game.Utilities.Logging;
using Michsky.MUIP;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.UI
{
    public class HorseInfoListManagerUI : MonoBehaviour, IHorseInfoManagerUI
    {
        public event EventHandler<int> OnHorseSelected;

        #region Fields 

        [Header("Main - Settings")]
        [SerializeField] private GameObject _horseInfoCardPrefab;
        [SerializeField] private ButtonManager _confirmationButton;

        [Space]

        [Header("Visual - Settings")]
        [SerializeField] private Color _horseCardSelectionColor = Color.green;

        [Space]

        [Header("Scroll - Settings")]
        [SerializeField] private ScrollRect _scroll;

        private HorizontalLayoutGroup _horizontalLayout;
        private RectTransform _contentRectTransform;
        private float _elementWidth;

        private Dictionary<int, HorseInfo> _horseInfoDict;

        private List<HorseInfoCardUI> _usedCards;
        private Queue<HorseInfoCardUI> _freeCards;

        private HorseInfoCardUI _selectedHorseInfoCard;
        private int _selectedHorseID;
        private Dictionary<HorseInfoCardUI, Action> _cardClickHandlers = new Dictionary<HorseInfoCardUI, Action>();

        #endregion


        #region Methods 

        public void Initialize()
        {
            _horizontalLayout = GetComponentInChildren<HorizontalLayoutGroup>();
            _scroll.horizontalNormalizedPosition = 0;
            _usedCards = new();
            _freeCards = new();

            RectTransform elementRectTransform = _horseInfoCardPrefab.GetComponent<RectTransform>();
            _contentRectTransform = _horizontalLayout.GetComponent<RectTransform>();
            _elementWidth = elementRectTransform.sizeDelta.x;

            _confirmationButton.onClick.AddListener(OnConfirmationButtonClicked);
        }

        public void ResetState()
        {
            TryUnselectCurrentCard();
            _horseInfoDict.Clear();

            UnsubscribeAllCardsFromClicks();

            foreach (var card in _usedCards)
            {
                card.ResetState();
                _freeCards.Enqueue(card);
            }

            _usedCards.Clear();
        }

        public void DisplayAvailableHorsesToSelect()
        {
            if ((_horseInfoDict == null) || (_horseInfoDict.Count() == 0))
            {
                return;
            }

            ResizeContentForHorizontalScroll(_horizontalLayout, _contentRectTransform, _horseInfoDict.Count(), _elementWidth);
            GenerateHorseInfoGrid(_horseInfoDict);
        }

        public void ReceiveHorseInfoToSelect(Dictionary<int, HorseInfo> horseInfoDict, bool clearPreviousInfo = true)
        {
            if ((_horseInfoDict == null) || clearPreviousInfo)
            {
                _horseInfoDict = horseInfoDict;
            }
            else
            {
                _horseInfoDict.AddRange(horseInfoDict);
            }
        }

        private void ResizeContentForHorizontalScroll(HorizontalLayoutGroup layoutGroup, RectTransform layoutTransform, int cellsCount, float elementWidth)
        {
            int totalHorses = cellsCount;

            float spacing = layoutGroup != null ? layoutGroup.spacing : 0f;
            float paddingLeft = layoutGroup != null ? layoutGroup.padding.left : 0f;
            float paddingRight = layoutGroup != null ? layoutGroup.padding.right : 0f;

            float totalWidth = (elementWidth * totalHorses) + (spacing * (totalHorses - 1)) + paddingLeft + paddingRight;

            Vector2 newSize = layoutTransform.sizeDelta;
            newSize.x = totalWidth;
            layoutTransform.sizeDelta = newSize;
        }


        private void GenerateHorseInfoGrid(Dictionary<int, HorseInfo> horseInfoDict)
        {
            HorseInfo currentHorseInfo;
            Dictionary<int, float> relativeWinChances = CalculateRelativeWinChances(horseInfoDict);

            foreach (var infoPair in horseInfoDict)
            {
                HorseInfoCardUI infoCard;
                currentHorseInfo = infoPair.Value;

                if (_freeCards.Count > 0)
                {
                    infoCard = _freeCards.Dequeue();
                }
                else
                {
                    GameObject infoCardObject = Instantiate(_horseInfoCardPrefab, _horizontalLayout.transform);
                    infoCard = infoCardObject.GetComponent<HorseInfoCardUI>();
                }

                _usedCards.Add(infoCard);
                SetInfoCardContent(infoCard, currentHorseInfo, relativeWinChances[infoPair.Key]);

                void Handler() => OnHorseInfoCardClicked(infoCard, infoPair.Key);
                _cardClickHandlers[infoCard] = Handler;

                infoCard.SubscribeOnClick(Handler);
            }
        }

        private void UnsubscribeAllCardsFromClicks()
        {
            foreach (var pair in _cardClickHandlers)
            {
                pair.Key.UnsubscribeFromClick(pair.Value);
            }
            _cardClickHandlers.Clear();
        }

        // ToDo : update this info text later
        private static void SetInfoCardContent(HorseInfoCardUI infoCard, HorseInfo currentHorseInfo, float relativeWinChance)
        {
            infoCard.ReplaceImage(currentHorseInfo.Icon);
            infoCard.ReplaceText($"{currentHorseInfo.Name}" +
                $"\nWins: {currentHorseInfo.WinsCount}" +
                $"\nChance: {relativeWinChance.ToString("00")}%");
        }

        private Dictionary<int, float> CalculateRelativeWinChances(Dictionary<int, HorseInfo> horseInfoDict)
        {
            float totalChance = 0f;
            Dictionary<int, float> calculatedWinChances = new Dictionary<int, float>();

            foreach (var infoPair in horseInfoDict)
            {
                totalChance += infoPair.Value.WinChance;
            }

            foreach (var infoPair in horseInfoDict)
            {
                // Calculate normalized chance (relative to other horses)
                float normalizedChance = (infoPair.Value.WinChance / totalChance) * 100f;
                calculatedWinChances[infoPair.Key] = normalizedChance;
            }

            return calculatedWinChances;
        }

        private void OnHorseInfoCardClicked(HorseInfoCardUI infoCard, int selectedHorseID)
        {
            if (_selectedHorseInfoCard == infoCard)
            {
                return;
            }

            TryUnselectCurrentCard();

            _selectedHorseInfoCard = infoCard;
            _selectedHorseID = selectedHorseID;
            _selectedHorseInfoCard.TintImage(_horseCardSelectionColor);
        }

        private void TryUnselectCurrentCard()
        {
            if (_selectedHorseInfoCard != null)
            {
                _selectedHorseInfoCard.ResetImageColor();
                _selectedHorseInfoCard = null;
            }
        }

        private void OnConfirmationButtonClicked()
        {
            if (_selectedHorseInfoCard != null)
            {
                // Find the HorseInfo associated with the selected button

                if (_horseInfoDict.ContainsKey(_selectedHorseID))
                {
                    OnHorseSelected?.Invoke(this, _selectedHorseID);
                }
            }
            else
            {
                CustomLogger.LogWarning($" # No {nameof(HorseInfo)} selected! | Can not confirm");
            }
        }

        #endregion
    }
}
