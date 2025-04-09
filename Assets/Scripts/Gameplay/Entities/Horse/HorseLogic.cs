
using UnityEngine;

namespace Game.Gameplay.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class HorseLogic : MonoBehaviour
    {
        #region Fields 

        private int _id;
        private HorseInfo _info;

        private HorseAnimationsHandler _animationsHandler;
        private Rigidbody _rigidbody;
        
        #endregion


        #region Properties

        public int ID => _id;
        public HorseInfo Info => _info;

        #endregion


        #region Methods

        #region Init
        
        public void Initialize(HorseInfo info)
        {
            _info = info;

            // Future ToDo : implement better ID generation approach.
            _id = _info.GetHashCode();

            _rigidbody = GetComponent<Rigidbody>();
            _animationsHandler = GetComponent<HorseAnimationsHandler>();

            _animationsHandler.Initialize();

        }

        #endregion

        public void Run()
        {
            _animationsHandler.ActivateRunAnimation();
            _rigidbody.velocity = transform.forward * 5f;
        }

        #endregion
    }
}
