using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamerasController : MonoBehaviour
{
    #region Methods

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
