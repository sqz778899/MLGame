using UnityEngine;

public abstract class Singleton<T> where T:new()
{
    private static T _instance;
    private static object mutex = new object();

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                lock (mutex)
                {
                    _instance = new T();
                }
            }
            return _instance;
        }
    }
}

public class UnitySingelenton<T> : MonoBehaviour where T : Component
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = (T)obj.AddComponent(typeof(T));
                    obj.hideFlags = HideFlags.DontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
            _instance = this as T;
        else
            Destroy(gameObject);
    }
}
