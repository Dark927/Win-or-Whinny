
using Game.Settings.Common;
using System;
using UnityEngine;

namespace Game.Gameplay.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class HorseLogic : MonoBehaviour, IDisposable, IResetable
    {
        #region Fields 

        private int _id;
        private HorseInfo _info;
        private HorseStats _stats;

        private HorseAnimationsHandler _animationsHandler;
        private HorseAudioHandler _audioHandler;
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
            _audioHandler = GetComponentInChildren<HorseAudioHandler>();
            _movement = new HorseMovement(this);

            _audioHandler.Initialize();
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
            _animationsHandler.OnFootStep -= _audioHandler.PlayFootStepSFX;
        }

        public void ResetState()
        {
            _movement.StopImmediately();
            _animationsHandler.ActivateIdleAnimation();
        }

        #endregion

        /*  
         *  Note :
         *  Use velocity to prevent update calls,
         *  Replace that logic if more complex horse movement control is needed.
        */
        public void Run()
        {
            _animationsHandler.OnFootStep += _audioHandler.PlayFootStepSFX;
            _movement.MoveWithAccelerationChance(transform.forward);
            _animationsHandler.ActivateRunAnimation();
        }

        public void Stop()
        {
            _animationsHandler.OnFootStep -= _audioHandler.PlayFootStepSFX;
            _movement.StopWithDrag();
            _animationsHandler.ActivateIdleAnimation();
        }

        #endregion
    }
}
