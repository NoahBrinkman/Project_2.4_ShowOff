using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : SingleTon<T>
{
    private static T _instance;
    public static T Instance => _instance;

    /// <summary>
    /// Overridable Awake method, in children make sure to call base still.
    /// </summary>
    public virtual void Awake()
    {
        if (_instance != null )
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
    }
}
