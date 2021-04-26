using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (!_cached)
            {
                _instance = FindObjectOfType<T>();
               
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString();
                }

                _cached = true;
            } 

            return _instance;
        }
    }

    private static T _instance;
    private static bool _cached = false;

    protected virtual void OnDestroy()
    {
        _instance = null;
        _cached = false;
    }
}
