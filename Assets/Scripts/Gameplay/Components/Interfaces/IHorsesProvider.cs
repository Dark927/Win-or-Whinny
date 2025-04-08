using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IHorsesProvider : IDisposable
    {
        public bool HasActiveLoadings { get; }
        public void Initialize(Transform horsesContainer = null);
        public IEnumerable<HorseLogic> GetAllHorses();
        public HorseLogic GetHorseByID(int ID);
        public IEnumerable<UniTask> AddMultipleHorsesAsync(IEnumerable<MainHorseData> horsesDataCollection);
        public UniTask AddHorseAsync(MainHorseData horseData);
    }
}
