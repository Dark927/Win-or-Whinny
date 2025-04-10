
using UnityEngine;

namespace Game.Gameplay.UI
{
    internal class DefaultRacePanelManagerUI : MonoBehaviour, IRacePanelManagerUI
    {
        private IRaceStartPanelUI _startPanelUI;
        private IRaceEndPanelUI _endPanelUI;

        public IRaceStartPanelUI StartPanelUI => _startPanelUI;
        public IRaceEndPanelUI EndPanelUI => _endPanelUI;

        public void Initialize()
        {
            _startPanelUI = GetComponentInChildren<IRaceStartPanelUI>(true);
            _endPanelUI = GetComponentInChildren<IRaceEndPanelUI>(true);

            _startPanelUI.Initialize();
            _endPanelUI.Initialize();
        }

        public void ResetState()
        {
            _startPanelUI.ResetState();
            _endPanelUI.ResetState();
        }
    }
}
