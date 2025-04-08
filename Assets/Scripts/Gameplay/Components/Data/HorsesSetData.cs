using Game.Abstractions;
using UnityEngine;
using System.Collections.Generic;
using Game.Gameplay.Entities;

namespace Game.Gameplay.Settings
{
    [CreateAssetMenu(fileName = "NewHorsesSetData", menuName = "Game/Gameplay/Horses Set Data")]
    public class HorsesSetData : DescriptionBaseData
    {
        [SerializeField] private List<MainHorseData> _horsesDataList;

        public IEnumerable<MainHorseData> HorsesDataList => _horsesDataList;
    }
}
