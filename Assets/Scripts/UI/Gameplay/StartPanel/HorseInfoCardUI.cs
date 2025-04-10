
using Game.Settings.Common;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.UI
{
    [RequireComponent(typeof(Button))]
    public class HorseInfoCardUI : MonoBehaviour, IResetable
    {
        [SerializeField] private Image _iconImage;
        private Button _button;
        private Color _initialTintColor;
        private TextMeshProUGUI _descriptionText;

        public TextMeshProUGUI Text => _descriptionText;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _initialTintColor = _iconImage.color;
            _descriptionText = GetComponentInChildren<TextMeshProUGUI>();
            _button.onClick.AddListener(ListenButtonClick);
        }

        public void ResetState()
        {
            _iconImage.sprite = null;
            _descriptionText.text = string.Empty;
        }

        private void ListenButtonClick()
        {
            ClickSubscribers.ForEach(subscriber => subscriber?.Invoke());
        }

        private List<Action> ClickSubscribers = new List<Action>();

        public void SubscribeOnClick(Action listener)
        {
            ClickSubscribers.Add(listener);
        }

        public void UnsubscribeFromClick(Action listener)
        {
            Debug.Log("count before remove : " + ClickSubscribers.Count);
            ClickSubscribers.Remove(listener);

            Debug.Log("count after remove : " + ClickSubscribers.Count);
        }

        public void TintImage(UnityEngine.Color color)
        {
            if (_iconImage != null)
            {
                _iconImage.color = color;
            }
        }

        public void ResetImageColor()
        {
            TintImage(_initialTintColor);
        }

        public void ReplaceImage(Sprite sprite)
        {
            if (_iconImage != null)
            {
                _iconImage.sprite = sprite;
            }
        }

        public void ReplaceText(string text)
        {
            if (_descriptionText != null)
            {
                _descriptionText.text = text;
            }
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