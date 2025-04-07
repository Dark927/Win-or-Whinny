
using UnityEngine;

/// <summary>
/// Base class for ScriptableObjects that need a description field.
/// </summary>
namespace Game.Abstractions
{
    public class DescriptionBaseData : ScriptableObject
    {
        [SerializeField, TextArea] private string _description;

        public string Description => _description;
    }
}