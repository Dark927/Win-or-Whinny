using Cysharp.Threading.Tasks;
using Game.Gameplay.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IHorsesProvider : IDisposable
    {
        public bool HasActiveLoadings { get; }
        public void Initialize(Transform horsesContainer = null);
        public void ResetAllHorses();
        public IEnumerable<HorseLogic> GetAllHorses();
        public HorseLogic GetHorseByID(int ID);
        public IEnumerable<UniTask> AddMultipleHorsesAsync(IEnumerable<MainHorseData> horsesDataCollection, CancellationToken token = default);
        public UniTask AddHorseAsync(MainHorseData horseData, CancellationToken token = default);
    }
}
