

using Game.Settings.GameInitialization;

namespace Game.Gameplay.UI
{
    public interface IRacePanelManagerUI : IInitializable
    {
        public IRaceStartPanelUI StartPanelUI { get; }
        public IRaceEndPanelUI EndPanelUI { get; }

    }
}
