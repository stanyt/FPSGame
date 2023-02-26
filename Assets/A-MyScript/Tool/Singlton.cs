
using UnityEngine;

public class Singlton<T> : MonoBehaviour where T :Singlton<T>
{
    public static T Instance  =>_instance;
    private static T _instance;
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else _instance = (T)this;
    }
}
