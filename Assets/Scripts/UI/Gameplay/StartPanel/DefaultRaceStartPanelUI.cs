
using Game.Gameplay.Entities;
using Game.Settings.Common;
using System;
using System.Collections.Generic;

namespace Game.Gameplay.UI
{
    public class DefaultRaceStartPanelUI : RacePanelBaseUI, IRaceStartPanelUI, IResetable
    {
        private IHorseInfoManagerUI _horseInfoListManager;

        public void Initialize()
        {
            _horseInfoListManager = GetComponentInChildren<IHorseInfoManagerUI>();
            _horseInfoListManager.Initialize();
        }

        public void DisplayAvailableHorsesToSelect(Dictionary<int, HorseInfo> horsesInfo)
        {
            _horseInfoListManager.ReceiveHorseInfoToSelect(horsesInfo);
            _horseInfoListManager.DisplayAvailableHorsesToSelect();
        }

        public void SubscribeOnHorseSelection(EventHandler<int> listener)
        {
            _horseInfoListManager.OnHorseSelected += listener;
        }

        public void UnsubscribeFromHorseSelection(EventHandler<int> listener)
        {
            _horseInfoListManager.OnHorseSelected -= listener;
        }

        public void ResetState()
        {
            _horseInfoListManager?.ResetState();
        }
    }
}
