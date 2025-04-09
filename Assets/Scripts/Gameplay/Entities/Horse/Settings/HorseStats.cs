using UnityEngine;

namespace Game.Gameplay.Entities
{
    [System.Serializable]
    public struct HorseStats
    {
        public const float DefaultStoppingBodyDrag = 2f;

        [Header("Speed")]
        [SerializeField, Min(0)] private float _defaultSpeed;

        [Space]

        [Header("Acceleration")]
        [SerializeField, Range(0, 100)] private float _accelerationChance;
        [SerializeField, Min(0)] private float _accelerationSpeedMultiplier;

        [Tooltip("how long it takes to accelerate")]
        [SerializeField, Min(0)] private float _accelerationRampUpDuration;
        [Tooltip("full acceleration action time (when fully accelerated)")]
        [SerializeField, Min(0)] private float _accelerationEffectDuration;
        [Tooltip("time to retry acceleration")]
        [SerializeField, Min(0)] private float _accelerationRechargeTime;


        public float DefaultSpeed => _defaultSpeed;
        public float AccelerationChance => _accelerationChance;
        public float AccelerationSpeedMultiplier => _accelerationSpeedMultiplier;
        public float AccelerationRampUpDuration => _accelerationRampUpDuration;
        public float AccelerationEffectDuration => _accelerationEffectDuration;
        public float AccelerationRechargeTime => _accelerationRechargeTime;
    }
}
