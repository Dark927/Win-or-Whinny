using Game.Settings.GameInitialization;
using TMPro;
using UnityEngine;

namespace Game.Gameplay.Entities
{
    public class HorseVisualIndicator : MonoBehaviour, IInitializable
    {
        #region Fields 

        private TextMeshPro _indicatorTextMesh;

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
            _indicatorTextMesh = GetComponentInChildren<TextMeshPro>();
            HideTextIndicator();
        }

        #endregion

        public void SetIndicatorText(string indicatorText)
        {
            _indicatorTextMesh.text = indicatorText;
        }

        public void HighlightIndicator(Color highlightColor)
        {
            _indicatorTextMesh.color = highlightColor;
        }

        public void ResetIndicatorColor()
        {
            _indicatorTextMesh.color = Color.white;
        }

        public void HideTextIndicator()
        {
            _indicatorTextMesh.gameObject.SetActive(false);
        }

        public void ShowTextIndicator()
        {
            _indicatorTextMesh.gameObject.SetActive(true);
        }

        #endregion
    }
}
