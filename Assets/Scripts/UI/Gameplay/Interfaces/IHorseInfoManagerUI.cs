

using Game.Gameplay.Entities;
using Game.Settings.GameInitialization;
using System;
using System.Collections.Generic;

namespace Game.Gameplay.UI
{
    public interface IHorseInfoManagerUI : IInitializable
    {
        /// <summary>
        /// Second argument is horse ID
        /// </summary>
        public event EventHandler<int> OnHorseSelected;

        public void DisplayAvailableHorsesToSelect();
        public void ReceiveHorseInfoToSelect(Dictionary<int, HorseInfo> horseInfoDict, bool clearPreviousInfo = true);

    }
}
