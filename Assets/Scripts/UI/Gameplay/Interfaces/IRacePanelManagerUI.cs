

using Game.Settings.Common;
using Game.Settings.GameInitialization;

namespace Game.Gameplay.UI
{
    public interface IRacePanelManagerUI : IInitializable, IResetable
    {
        public IRaceStartPanelUI StartPanelUI { get; }
        public IRaceEndPanelUI EndPanelUI { get; }

    }
}
