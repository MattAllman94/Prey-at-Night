using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    // All Managers
    protected static GameManager _GM { get { return GameManager.INSTANCE; } }
    protected static UIManager _UI { get { return UIManager.INSTANCE; } }

    protected static PowersManager _PM { get { return PowersManager.INSTANCE; } }





}










public class Singleton<T> : Prey where T : MonoBehaviour
{
    private static T instance_;
    public static T INSTANCE
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = GameObject.FindObjectOfType<T>();
                if (instance_ == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    singleton.AddComponent<T>(); // AwakeAwake gets gets called called inside AddComponent
                }
            }
            return instance_;
        }
    }
    protected virtual void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
