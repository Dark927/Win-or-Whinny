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
        private Dictionary<int, HorseInfo> _horseInfoDict;
        private HorseInfoCardUI _selectedHorseInfoCard;
        private int _selectedHorseID;

        #endregion


        #region Methods 

        public void Initialize()
        {
            _horizontalLayout = GetComponentInChildren<HorizontalLayoutGroup>();
            _scroll.horizontalNormalizedPosition = 0;
            _confirmationButton.onClick.AddListener(OnConfirmationButtonClicked);
        }

        public void DisplayAvailableHorsesToSelect()
        {
            if ((_horseInfoDict == null) || (_horseInfoDict.Count() == 0))
            {
                return;
            }

            ResizeContentForHorizontalScroll(_horizontalLayout, _horseInfoDict.Count());
            GenerateHorseInfoGrid();
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

        private void ResizeContentForHorizontalScroll(HorizontalLayoutGroup layoutGroup, int cellsCount)
        {
            RectTransform contentRectTransform = layoutGroup.GetComponent<RectTransform>();
            int totalHorses = cellsCount;

            RectTransform elementRectTransform = _horseInfoCardPrefab.GetComponent<RectTransform>();
            float elementWidth = elementRectTransform.sizeDelta.x;

            float spacing = layoutGroup != null ? layoutGroup.spacing : 0f;
            float paddingLeft = layoutGroup != null ? layoutGroup.padding.left : 0f;
            float paddingRight = layoutGroup != null ? layoutGroup.padding.right : 0f;

            float totalWidth = (elementWidth * totalHorses) + (spacing * (totalHorses - 1)) + paddingLeft + paddingRight;

            Vector2 newSize = contentRectTransform.sizeDelta;
            newSize.x = totalWidth;
            contentRectTransform.sizeDelta = newSize;
        }

        private void GenerateHorseInfoGrid()
        {
            HorseInfo currentHorseInfo;

            foreach (var infoPair in _horseInfoDict)
            {
                currentHorseInfo = infoPair.Value;
                GameObject infoCardObject = Instantiate(_horseInfoCardPrefab, _horizontalLayout.transform);
                HorseInfoCardUI infoCard = infoCardObject.GetComponent<HorseInfoCardUI>();

                // ToDo : update this info text later
                infoCard.ReplaceImage(currentHorseInfo.Icon);
                infoCard.ReplaceText($"{currentHorseInfo.Name}" +
                    $"\nWins: {currentHorseInfo.WinsCount}" +
                    $"\nChance: {(currentHorseInfo.WinChance * 100).ToString("00")}%");

                infoCard.SubscribeOnClick(() => OnHorseInfoCardClicked(infoCard, infoPair.Key));
            }
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
                    CustomLogger.Log("selected horse ID -> " + _selectedHorseID);
                    OnHorseSelected?.Invoke(this, _selectedHorseID);
                    //gameManager.StartGame(selectedHorseInfo.id);
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
