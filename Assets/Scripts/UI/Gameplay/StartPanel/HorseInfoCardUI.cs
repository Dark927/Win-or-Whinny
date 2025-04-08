
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.UI
{
    [RequireComponent(typeof(Button))]
    public class HorseInfoCardUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        private Button _button;
        private Color _initialTintColor;
        private TextMeshProUGUI _textMeshPro;

        public TextMeshProUGUI Text => _textMeshPro;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _initialTintColor = _iconImage.color;
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
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
            if (_textMeshPro != null)
            {
                _textMeshPro.text = text;
            }
        }
    }
}