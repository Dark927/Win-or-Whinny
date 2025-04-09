using UnityEngine;

namespace Game.Gameplay.Cameras
{
    public class RaceCameraPoint : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private ViewType _lookView;

        #endregion


        #region Properties

        public ViewType LookView => _lookView;

        #endregion
    }
}
