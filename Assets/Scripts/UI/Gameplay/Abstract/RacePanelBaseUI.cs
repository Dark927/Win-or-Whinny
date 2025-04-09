

using UnityEngine;

namespace Game.Gameplay.UI
{
    public abstract class RacePanelBaseUI : MonoBehaviour
    {
        public virtual void Activate()
        {
            // ToDo : add custom popup animations logic in the future (for childs)
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            // ToDo : add custom close animations logic in the future (for childs)
            gameObject.SetActive(false);
        }
    }
}
