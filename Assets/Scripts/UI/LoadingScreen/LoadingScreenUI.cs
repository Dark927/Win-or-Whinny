using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Settings.GameInitialization;
using Michsky.MUIP;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Screens
{
    public sealed class LoadingScreenUI : MonoBehaviour, IInitializable
    {
        #region Fields 

        private const float MinProgressUpdateDiff = 0.01f;

        [SerializeField] private float _deactivationDelay = 3f;
        [SerializeField] private float _maxProgressUpdateDuration = 1f;  // Maximum duration for the progress update
        [SerializeField] private float _minProgressUpdateDuration = 0.1f; // Minimum duration for the progress update

        private ProgressBar _progressBar;

        private Queue<(float targetPercent, float duration)> _progressQueue;
        private Tweener _currentTweener;
        private bool _isUpdatingProgress = false;

        #endregion

        #region Properties

        public bool IsFullProgress => (_progressBar != null) && _progressBar.IsDone();
        public bool IsUpdating => _isUpdatingProgress;

        #endregion

        #region Methods

        #region Init

        public void Initialize()
        {
            _progressQueue = new();
            _progressBar = GetComponentInChildren<ProgressBar>();
            _progressBar.SetValue(0f);
        }

        #endregion

        /// <summary>
        /// Set the loading progress to the loading screen indicators
        /// </summary>
        /// <param name="progressPercent">Target progress percent (0-1)</param>
        /// <param name="forceApply">Apply the progress without any restrictions (ignoring the same values). Will create the same update animations.</param>
        public void SetLoadingProgress(float progressPercent, bool forceApply = false)
        {
            if (_progressBar == null)
            {
                return;
            }

            // ToDo : refactore this later 
            progressPercent = Mathf.Clamp((progressPercent * _progressBar.maxValue) + _progressBar.minValue, _progressBar.minValue, _progressBar.maxValue);
            float progressDifference = Mathf.Abs(_progressBar.currentPercent - progressPercent) / _progressBar.maxValue;
            float updateDuration = _maxProgressUpdateDuration * progressDifference;
            updateDuration = Mathf.Clamp(updateDuration, _minProgressUpdateDuration, _maxProgressUpdateDuration);

            if (forceApply || progressDifference > MinProgressUpdateDiff)
            {
                _progressQueue.Enqueue((progressPercent, updateDuration));

                if (!_isUpdatingProgress)
                {
                    UpdateProgressBarAsync().Forget();
                }
            }
        }

        private async UniTask UpdateProgressBarAsync()
        {
            _isUpdatingProgress = true;

            while (_progressQueue.Count > 0)
            {
                var progressItem = _progressQueue.Dequeue();

                _currentTweener = DOTween.To(
                    (value) => _progressBar.SetValue(value),
                    _progressBar.currentPercent,
                    progressItem.targetPercent,
                    progressItem.duration);

                await _currentTweener.AsyncWaitForCompletion();
            }

            _isUpdatingProgress = false;
        }

        public void SetFullProgress()
        {
            if (_progressBar == null)
            {
                return;
            }

            SetLoadingProgress(_progressBar.maxValue);
        }

        public async UniTask<bool> PrepareForDeactivation()
        {
            await UniTask.WaitForSeconds(_deactivationDelay);
            return true;
        }

        #endregion
    }
}
