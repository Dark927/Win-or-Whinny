
using UnityEngine;

namespace Game.Gameplay.UI
{
    [System.Serializable]
    public struct RaceResultSubtitleParameters
    {
        [SerializeField, Min(0)] private int _minParticipantPlace;
        [SerializeField] private string _text;
        [SerializeField] private Color _color;

        public int MinParticipantPlace => _minParticipantPlace;
        public string Text => _text;
        public Color Color => _color;
    }
}
