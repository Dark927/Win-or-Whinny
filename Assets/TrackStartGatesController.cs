using Game.Gameplay.InteractiveProps;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackStartGatesController : MonoBehaviour
{
    #region Fields 

    private List<StartGateDoor> _gateDoors;

    #endregion


    #region Methods

    #region Init

    private void Awake()
    {
        _gateDoors = GetComponentsInChildren<StartGateDoor>().ToList();
    }

    void Update()
    {
        // Example triggers for Open/Close animation (you can replace with actual game logic)
        if (Input.GetKeyDown(KeyCode.O)) // Press 'O' to open the gate
        {
            Open();
        }
    }

    #endregion

    public void Open()
    {
        _gateDoors.ForEach(door => door.Open());
    }

    #endregion
}
