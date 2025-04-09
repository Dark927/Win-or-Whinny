using Game.Gameplay.InteractiveProps;
using Game.Gameplay.Race;
using Game.Settings.GameInitialization;
using Game.Utilities.Logging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackStartGatesController : MonoBehaviour, IInitializable
{
    #region Fields 

    private List<StartGateDoor> _gateDoors;
    private RaceStartingPointsHolder _startingPointsHolder;

    // Key - starting point transform, Value - owner transform.
    private Dictionary<Transform, Transform> _usedStartingPoints;
    private List<Transform> _availableStartingPoints;

    #endregion

    #region Methods

    #region Init

    public void Initialize()
    {
        _gateDoors = GetComponentsInChildren<StartGateDoor>().ToList();
        _startingPointsHolder = GetComponentInChildren<RaceStartingPointsHolder>();

        _startingPointsHolder.Initialize();
        _availableStartingPoints = _startingPointsHolder.GetStartingPoints().ToList();
        _usedStartingPoints = new();
    }

    #endregion

    public void Open()
    {
        _gateDoors.ForEach(door => door.Open());
    }

    public void SetHorseOnStartingPoint(Transform horseTransform)
    {
        if((_availableStartingPoints != null) && (_availableStartingPoints.Count() > 0))
        {
            var point = _availableStartingPoints.First();
            horseTransform.SetPositionAndRotation(point.position, point.rotation);
            _usedStartingPoints.Add(point, horseTransform);
            _availableStartingPoints.Remove(point);
        }
        else
        {
            CustomLogger.LogWarning($" # Starting points are out | Can not set horse on starting point | {gameObject.name}");
        }
    }

    #endregion
}
