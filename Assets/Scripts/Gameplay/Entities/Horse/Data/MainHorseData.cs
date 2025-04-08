

using Game.Abstractions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Gameplay.Entities
{
    [CreateAssetMenu(fileName = "NewHorseData", menuName = "Game/Gameplay/Entity/Main Horse Data")]

    public class MainHorseData : DescriptionBaseData
    {
        [SerializeField] private AssetReference _horsePrefabReference;
        [SerializeField] private HorseInfo _horseInfo;

        public AssetReference HorsePrefabReference => _horsePrefabReference;
        public HorseInfo HorseInfo => _horseInfo;
    }
}
