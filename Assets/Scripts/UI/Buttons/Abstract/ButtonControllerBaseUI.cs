
using Game.Utilities.Logging;
using Michsky.MUIP;
using UnityEngine;

namespace Game.UI.Buttons
{
    public abstract class ButtonControllerBaseUI : MonoBehaviour
    {
        #region Fields 

        private ButtonManager _buttonManager;

        #endregion

        #region Methods
        public abstract void ClickEventListener();

        private void Awake()
        {
            _buttonManager = GetComponent<ButtonManager>();

            if (_buttonManager == null)
            {
                CustomLogger.LogComponentIsNull(gameObject.name, nameof(ButtonManager));
                gameObject.SetActive(false);
                return;
            }

            _buttonManager.onClick.AddListener(ClickEventListener);
        }


        #endregion
    }
}
