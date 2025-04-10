
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
        private HorseVisualIndicator _visualIndicator;
        private HorseAudioHandler _audioHandler;
        private HorseMovement _movement;

        #endregion

        public HorseVisualIndicator Indicator => _visualIndicator;

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
            _visualIndicator = GetComponentInChildren<HorseVisualIndicator>(true);

            _movement = new HorseMovement(this);

            _audioHandler.Initialize();
            _animationsHandler.Initialize();
            ConfigureTextIndicator();
        }

        private void ConfigureTextIndicator()
        {
            _visualIndicator.Initialize();
            _visualIndicator.SetIndicatorText(_info.Name);
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
            _movement.OnSpeedUpdate -= _animationsHandler.UpdateRunAnimationSpeed;
        }

        public void ResetState()
        {
            _visualIndicator.ResetIndicatorColor();
            _visualIndicator.HideTextIndicator();
            _movement.StopImmediately();
            _animationsHandler.ActivateIdleAnimation();
            _animationsHandler.UpdateRunAnimationSpeed(_stats.DefaultSpeed);
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

            _animationsHandler.OnFootStep += _audioHandler.PlayFootStepSFX;
            _movement.OnSpeedUpdate += _animationsHandler.UpdateRunAnimationSpeed;
        }

        public void Stop()
        {
            _movement.StopWithDrag();
            _animationsHandler.ActivateIdleAnimation();
            _animationsHandler.UpdateRunAnimationSpeed(_stats.DefaultSpeed);

            _animationsHandler.OnFootStep -= _audioHandler.PlayFootStepSFX;
            _movement.OnSpeedUpdate -= _animationsHandler.UpdateRunAnimationSpeed;
        }

        #endregion
    }
}
