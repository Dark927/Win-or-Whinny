
using Game.Settings.Common;
using System;
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
        }

        public void ResetState()
        {
            _iconImage.sprite = null;
            _descriptionText.text = string.Empty;
        }

        public void SubscribeOnClick(Action listener) => _button.onClick.AddListener(() => listener?.Invoke());

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
    }
}