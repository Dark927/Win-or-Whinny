
using Game.Settings.GameInitialization;
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

        private readonly int IdleTriggerID = Animator.StringToHash(IdleTriggerName);
        private readonly int RunTriggerID = Animator.StringToHash(RunTriggerName);
        // ----------------------------------------------------------------

        private Animator _animator;
        

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
        }
        

        public void ActivateIdleAnimation()
        {
            _animator.SetTrigger(IdleTriggerID);
        }

        public void ActivateRunAnimation()
        {
            _animator.SetTrigger(RunTriggerID);
        }
    }
}
