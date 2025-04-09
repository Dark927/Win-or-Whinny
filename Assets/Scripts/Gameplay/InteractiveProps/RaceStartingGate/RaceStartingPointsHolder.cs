
using Game.Settings.GameInitialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Race
{
    public class RaceStartingPointsHolder : MonoBehaviour, IInitializable
    {
        private List<Transform> _availableStartingPoints;

        public void Initialize()
        {
            _availableStartingPoints = new List<Transform>();

            foreach (Transform child in transform)
            {
                _availableStartingPoints.Add(child);
            }
        }

        public IEnumerable<Transform> GetStartingPoints()
        {
            return _availableStartingPoints;
        }
    }
}
