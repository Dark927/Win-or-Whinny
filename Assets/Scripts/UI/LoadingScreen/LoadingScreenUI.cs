using Cysharp.Threading.Tasks;
using Game.Settings.GameInitialization;
using Michsky.MUIP;
using UnityEngine;

namespace UI.Screens
{
    public sealed class LoadingScreenUI : MonoBehaviour, IInitializable
    {
        #region Fields 

        [SerializeField] private float _deactivationDelay = 3f;
        private ProgressBar _progressBar;

        #endregion


        #region Properties

        public bool IsFullProgress => (_progressBar != null) && _progressBar.IsDone();

        #endregion

        #region Methods

        #region Init

        public void Initialize()
        {
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        #endregion

        /// <summary>
        /// set the loading progress to the loading screen indicators
        /// </summary>
        /// <param name="progressPercent">target progress percent (0-1)</param>
        /// <param name="forceApply">apply the progress without any restrictions (like the ignoring the same values). Note : it will create the same update animations, etc.</param>
        public void SetLoadingProgress(float progressPercent)
        {
            if (_progressBar == null)
            {
                return;
            }

            _progressBar.SetValue(progressPercent);
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
