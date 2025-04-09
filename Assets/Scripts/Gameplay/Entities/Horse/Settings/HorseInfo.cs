using UnityEngine;

namespace Game.Gameplay.Entities
{
    [System.Serializable]
    public struct HorseInfo
    {
        private const float DefaultWinChance = 0.5f;

        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _winsCount;
        [SerializeField] private int _loseCount;


        public string Name => _name;
        public Sprite Icon => _icon;
        public int WinsCount => _winsCount;
        public int LoseCount => _loseCount;

        public float WinChance
        {
            get
            {
                int totalRaces = _winsCount + _loseCount;

                if (totalRaces == 0)
                {
                    return DefaultWinChance;
                }

                return (float)_winsCount / totalRaces;
            }
        }

        public HorseInfo(string name, Sprite icon, int winsCount, int loseCount)
        {
            this._name = name;
            this._icon = icon;
            this._winsCount = winsCount;
            this._loseCount = loseCount;
        }

        public void IncrementWins()
        {
            _winsCount++;
        }

        public void IncrementLoses()
        {
            _loseCount++;
        }
    }
}
