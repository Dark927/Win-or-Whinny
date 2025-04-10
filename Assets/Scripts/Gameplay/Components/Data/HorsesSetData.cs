using Game.Abstractions;
using Game.Gameplay.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Settings
{
    [CreateAssetMenu(fileName = "NewHorsesSetData", menuName = "Game/Gameplay/Horses Set Data")]
    public class HorsesSetData : DescriptionBaseData
    {
        [SerializeField] private List<MainHorseData> _horsesDataList;

        public IEnumerable<MainHorseData> HorsesDataList => _horsesDataList;
    }
}
