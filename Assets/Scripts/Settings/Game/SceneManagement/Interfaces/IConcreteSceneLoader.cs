using UnityEngine.SceneManagement;

namespace Game.Settings.SceneManagement
{
    public interface IConcreteSceneLoader<in TLoadParameter, out TLoadReturnParameter>
    {
        public TLoadReturnParameter LoadScene(TLoadParameter parameter, LoadSceneMode loadMode = LoadSceneMode.Single);
        public void UnloadAll();
    }
}
