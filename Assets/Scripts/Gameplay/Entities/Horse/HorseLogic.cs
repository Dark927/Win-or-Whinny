
using UnityEngine;

namespace Game.Gameplay.Entities
{
    public class HorseLogic : MonoBehaviour
    {
        private int _id;
        private HorseInfo _info;

        public int ID => _id;
        public HorseInfo Info => _info;

        public void Initialize(HorseInfo info)
        {
            _info = info;

            // ToDo : implement better ID generation approach in the future.
            _id = _info.GetHashCode();
        }
    }
}
