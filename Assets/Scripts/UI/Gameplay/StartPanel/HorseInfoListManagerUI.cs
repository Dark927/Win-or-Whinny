using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Gameplay.Entities;
using Michsky.MUIP;
using System.Linq;
using Unity.VisualScripting;
using System;
using Game.Utilities.Logging;

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

        private HorseInfoCardUI _selectedHorseInfoCard;
        private int _selectedHorseID;

        #endregion


        #region Methods 

        public void Initialize()
        {
            _horizontalLayout = GetComponentInChildren<HorizontalLayoutGroup>();
            _scroll.horizontalNormalizedPosition = 0;

            RectTransform elementRectTransform = _horseInfoCardPrefab.GetComponent<RectTransform>();
            _contentRectTransform = _horizontalLayout.GetComponent<RectTransform>();
            _elementWidth = elementRectTransform.sizeDelta.x;

            _confirmationButton.onClick.AddListener(OnConfirmationButtonClicked);
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
                currentHorseInfo = infoPair.Value;
                GameObject infoCardObject = Instantiate(_horseInfoCardPrefab, _horizontalLayout.transform);
                HorseInfoCardUI infoCard = infoCardObject.GetComponent<HorseInfoCardUI>();

                // ToDo : update this info text later
                infoCard.ReplaceImage(currentHorseInfo.Icon);
                infoCard.ReplaceText($"{currentHorseInfo.Name}" +
                    $"\nWins: {currentHorseInfo.WinsCount}" +
                    $"\nChance: {(relativeWinChances[infoPair.Key]).ToString("00")}%");

                infoCard.SubscribeOnClick(() => OnHorseInfoCardClicked(infoCard, infoPair.Key));
            }
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

            // unselect the previously selected HorseInfo (if any)
            if (_selectedHorseInfoCard != null)
            {
                _selectedHorseInfoCard.ResetImageColor();
            }

            _selectedHorseInfoCard = infoCard;
            _selectedHorseID = selectedHorseID;
            _selectedHorseInfoCard.TintImage(_horseCardSelectionColor);
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
