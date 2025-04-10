
using Game.Settings.GameInitialization;
using System;
using UnityEngine;

namespace Game.Gameplay.Entities
{
    [RequireComponent(typeof(Animator))]
    public class HorseAnimationsHandler : MonoBehaviour, IInitializable
    {
        // ----------------------------------------------------------------
        // ToDo : move these parameters to the configuration SO later
        // ----------------------------------------------------------------
        private const string IdleTriggerName = "Idle";
        private const string RunTriggerName = "Run";
        private const string RunSpeedMultiplierFieldName = "RunSpeedMultiplier";

        private readonly int IdleTriggerID = Animator.StringToHash(IdleTriggerName);
        private readonly int RunTriggerID = Animator.StringToHash(RunTriggerName);
        private readonly int RunSpeedMultiplierFieldID = Animator.StringToHash(RunSpeedMultiplierFieldName);
        // ----------------------------------------------------------------

        public event Action OnFootStep;

        private const float DefaultHorseSpeed = 7f;
        private Animator _animator;

        /// <summary>
        /// Event from the mesh Run animation
        /// </summary>
        public void FootStep()
        {
            OnFootStep?.Invoke();
        }

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _animator.SetFloat(RunSpeedMultiplierFieldID, 1f);
        }


        public void ActivateIdleAnimation()
        {
            _animator.SetTrigger(IdleTriggerID);
        }

        public void ActivateRunAnimation()
        {
            _animator.SetTrigger(RunTriggerID);
        }

        public void UpdateRunAnimationSpeed(float currentHorseSpeed)
        {
            float speedRatio = currentHorseSpeed / DefaultHorseSpeed;
            _animator.SetFloat(RunSpeedMultiplierFieldID, speedRatio);
        }
    }
}
