

using Game.Gameplay.Entities;
using Game.Settings.GameInitialization;
using System.Collections.Generic;
using System;
using Game.Settings.Common;

namespace Game.Gameplay.UI
{
    public interface IRaceStartPanelUI : IInitializable, IResetable
    {
        public void Activate();
        public void Deactivate();

        public void DisplayAvailableHorsesToSelect(Dictionary<int, HorseInfo> horsesInfo);
        public void SubscribeOnHorseSelection(EventHandler<int> listener);
        public void UnsubscribeFromHorseSelection(EventHandler<int> listener);
    }
}
