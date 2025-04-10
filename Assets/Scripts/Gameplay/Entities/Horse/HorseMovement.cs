using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Gameplay.Entities
{
    internal class HorseMovement : IDisposable
    {
        #region Fields 

        private const float MinLinearSpeedChangeTime = 0.01f;

        private HorseStats _stats;

        private Rigidbody _rigidbody;
        private Vector3 _currentDirection;

        private float _speedBeforeAcceleration;
        private float _currentSpeed;
        private float _lastAccelerationFinishTime;

        private bool _stopped;

        private CancellationTokenSource _cts;

        #endregion


        #region Properties

        public bool IsAccelerationRecharged => (Time.time - _lastAccelerationFinishTime) > _stats.AccelerationRechargeTime;

        #endregion


        #region Methods

        #region Init & Dispose

        public HorseMovement(HorseLogic owner)
        {
            _rigidbody = owner.GetComponent<Rigidbody>();
        }

        public void ApplyStats(HorseStats stats)
        {
            _stats = stats;
            Reset();
        }

        public void Reset()
        {
            _currentSpeed = _stats.DefaultSpeed;
            _stopped = false;
            _currentDirection = Vector3.zero;

            _lastAccelerationFinishTime = 0;
            _speedBeforeAcceleration = 0;
        }

        public void Dispose()
        {
            StopWithDrag();
        }


        #endregion

        /*  
          *  Note :
          *  Use velocity to prevent update calls,
          *  Replace that logic if more complex horse movement control is needed.
        */
        public void MoveWithConstantSpeed(Vector3 direction)
        {
            MoveWithConstantSpeed(_currentSpeed, direction);
        }

        public void MoveWithAccelerationChance(Vector3 direction)
        {
            if ((_cts == null) || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }

            MoveWithAccelerationAsync(direction, _cts.Token).Forget();
        }

        public void StopWithDrag()
        {
            _rigidbody.drag = HorseStats.DefaultStoppingBodyDrag;
            StopMovement();
        }

        public void StopImmediately()
        {
            _rigidbody.velocity = Vector3.zero;
            StopMovement();
        }

        private void StopMovement()
        {
            _stopped = true;

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        private void MoveWithConstantSpeed(float speed, Vector3 direction)
        {
            _rigidbody.drag = 0f;
            _currentDirection = direction;
            _currentSpeed = speed;

            _rigidbody.velocity = direction * speed;
        }

        private async UniTask MoveWithAccelerationAsync(Vector3 direction, CancellationToken token = default)
        {
            MoveWithConstantSpeed(_stats.DefaultSpeed, direction);

            if (_stats.AccelerationChance == 0f)
            {
                return;
            }

            bool isAccelerated = false;

            while (!_stopped && !token.IsCancellationRequested)
            {
                isAccelerated = await TryApplyAccelerationAsync(token);

                if (isAccelerated)
                {
                    await UniTask.WaitForSeconds(_stats.AccelerationEffectDuration, cancellationToken: token);
                    await DropAccelerationDuringTimeAsync(token);
                }
                else if (IsAccelerationRecharged)
                {
                    await WaitForAnotherAccelerationTry(token);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private async UniTask WaitForAnotherAccelerationTry(CancellationToken token = default)
        {
            await UniTask.WaitForSeconds(_stats.AccelerationRechargeTime, cancellationToken: token);
        }

        private async UniTask<bool> TryApplyAccelerationAsync(CancellationToken token = default)
        {
            bool isAccelerated = false;

            if (CanAccelerate())
            {
                if (_stats.AccelerationRampUpDuration > MinLinearSpeedChangeTime)
                {
                    await UpdateSpeedDuringTimeAsync(_currentSpeed * _stats.AccelerationSpeedMultiplier, _stats.AccelerationRampUpDuration, token);
                }
                else
                {
                    ApplyImmediateAcceleration(_stats.AccelerationSpeedMultiplier);
                }

                if (!token.IsCancellationRequested)
                {
                    isAccelerated = true;
                }
            }

            return isAccelerated;
        }

        private bool CanAccelerate()
        {
            if (_stopped || !IsAccelerationRecharged)
            {
                return false;
            }

            // Random chance to trigger acceleration
            float randomChance = UnityEngine.Random.Range(0f, 100f);
            bool canAccelerate = randomChance <= _stats.AccelerationChance;

            if (canAccelerate)
            {
                return true;
            }

            return false;
        }

        private async UniTask UpdateSpeedDuringTimeAsync(float targetSpeed, float duration, CancellationToken token = default)
        {
            _speedBeforeAcceleration = _currentSpeed;
            float elapsedTime = 0f;

            while ((elapsedTime < duration) && !token.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;

                MoveWithConstantSpeed(Mathf.Lerp(_speedBeforeAcceleration, targetSpeed, elapsedTime / duration), _currentDirection);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            }

            if (!token.IsCancellationRequested)
            {
                MoveWithConstantSpeed(targetSpeed, _currentDirection);
            }
            else
            {
                // Drop changed speed if canceled
                _currentSpeed = _speedBeforeAcceleration;
            }
        }

        private void ApplyImmediateAcceleration(float accelerationMult)
        {
            _speedBeforeAcceleration = _currentSpeed;
            MoveWithConstantSpeed(_currentSpeed * accelerationMult, _currentDirection);
        }

        private UniTask DropAccelerationDuringTimeAsync(CancellationToken token = default)
        {
            _lastAccelerationFinishTime = Time.time;
            return UpdateSpeedDuringTimeAsync(_speedBeforeAcceleration, _stats.AccelerationRampUpDuration, token);
        }

        private void UpdateCurrentVelocity()
        {
            if (!_stopped)
            {
                MoveWithConstantSpeed(_currentSpeed, _currentDirection);
            }
        }

        #endregion
    }
}
