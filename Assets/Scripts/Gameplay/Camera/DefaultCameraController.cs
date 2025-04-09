

using UnityEngine;

namespace Game.Gameplay.Cameras
{
    public class DefaultCameraController : MonoBehaviour
    {
        private bool _isActive;

        public bool IsActive => _isActive;

        #region Methods

        private void Awake()
        {
            _isActive = gameObject.activeInHierarchy;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _isActive = true;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            _isActive = false;
        }

        #endregion
    }
}
