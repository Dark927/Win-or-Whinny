

using Game.Gameplay.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.UI
{
    public class DefaultRaceStartPanelUI : MonoBehaviour, IRaceStartPanelUI
    {
        private IHorseInfoManagerUI _horseInfoListManager;


        public void Initialize()
        {
            _horseInfoListManager = GetComponentInChildren<IHorseInfoManagerUI>();
            _horseInfoListManager.Initialize();
        }

        public void Activate()
        {
            // ToDo : add custom popup animations logic in the future 
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            // ToDo : add custom close animations logic in the future 
            gameObject.SetActive(false);
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
    }
}
