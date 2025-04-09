
using System;
using UnityEngine;

namespace Game.Gameplay.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class HorseLogic : MonoBehaviour, IDisposable
    {
        #region Fields 

        private int _id;
        private HorseInfo _info;
        private HorseStats _stats;

        private HorseAnimationsHandler _animationsHandler;
        private HorseMovement _movement;
        
        #endregion


        #region Properties

        public int ID => _id;
        public HorseInfo Info => _info;

        #endregion


        #region Methods

        #region Init & Dispose
        
        public void Initialize(HorseInfo info)
        {
            _info = info;

            // Future ToDo : implement better ID generation approach.
            _id = _info.GetHashCode();

            _animationsHandler = GetComponent<HorseAnimationsHandler>();
            _movement = new HorseMovement(this);

            _animationsHandler.Initialize();
        }

        public void SetStats(HorseStats stats)
        {
            _stats = stats;
            _movement.ApplyStats(stats);
        }

        public void Dispose()
        {
            _movement?.Dispose();
        }

        #endregion

        /*  
         *  Note :
         *  Use velocity to prevent update calls,
         *  Replace that logic if more complex horse movement control is needed.
        */
        public void Run()
        {
            _movement.MoveWithAccelerationChance(transform.forward);
            _animationsHandler.ActivateRunAnimation();
        }

        public void Stop()
        {
            _movement.Stop();
            _animationsHandler.ActivateIdleAnimation();
        }

        #endregion
    }
}
