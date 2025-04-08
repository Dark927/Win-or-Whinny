
using UnityEngine;

namespace Game.Gameplay.InteractiveProps
{
    [RequireComponent(typeof(Animator))]
    public class StartGateDoor : MonoBehaviour
    {
        #region Fields 

        private Animator _gateAnimator;

        // IDs for triggers
        private int _openGateTriggerID;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _gateAnimator = GetComponent<Animator>();

            _openGateTriggerID = Animator.StringToHash("Open");
        }

        #endregion


        #region Gate Animation Methods

        // Trigger the gate open animation
        public void Open()
        {
            if (_gateAnimator != null)
            {
                _gateAnimator.SetTrigger(_openGateTriggerID);
            }
        }

        #endregion

        #endregion
    }
}
