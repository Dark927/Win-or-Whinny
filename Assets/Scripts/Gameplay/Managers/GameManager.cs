using Game.Gameplay.Entities;
using Game.Gameplay.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    #region Fields 

    private BackgroundCamerasController _backgroundCameras;
    private IHorseInfoManagerUI _horseInfoListManagerUI;

    // ToDo : remove this testing collection

    public List<MainHorseData> HorsesData;

    #endregion


    #region Properties

    #endregion


    #region Methods

    #region Init

    [Inject]
    public void Construct(BackgroundCamerasController backgroundCameras, IHorseInfoManagerUI horseInfoListManager)
    {
        _backgroundCameras = backgroundCameras;
        _horseInfoListManagerUI = horseInfoListManager;
    }

    private void Awake()
    {
        _backgroundCameras.Activate();


        // ToDo : move this to another components (just for tests)
        Dictionary<int, HorseInfo> temproraryDict = new Dictionary<int, HorseInfo>();

        for (int i = 0; i < HorsesData.Count(); i++)
        {
            temproraryDict.TryAdd(i, HorsesData[i].HorseInfo);
        }
        _horseInfoListManagerUI.Initialize();
        _horseInfoListManagerUI.ReceiveHorseInfoToSelect(temproraryDict);
        _horseInfoListManagerUI.DisplayAvailableHorsesToSelect();
    }


    #endregion


    #endregion
}

