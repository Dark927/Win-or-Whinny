using Game.Gameplay.Entities;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Race
{
    public class RaceFinish : MonoBehaviour
    {
        #region Fields 

        private IHorseRaceManager _horseRaceManager;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(IHorseRaceManager raceManager)
        {
            _horseRaceManager = raceManager;
        }

        #endregion


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<HorseLogic>(out var horseLogic))
            {
                _horseRaceManager.NotifyHorseFinished(horseLogic.ID);
            }
        }

        #endregion
    }
}
