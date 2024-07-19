using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                // Look for an existing instance
                _instance = FindObjectOfType<CoroutineRunner>();

                // If none exists, create a new one
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(CoroutineRunner).ToString());
                    _instance = singleton.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public delegate void DelayedFunc();
    public IEnumerator RunFunctionDelayed(float delay, DelayedFunc delayedFunc)
    {
        yield return Tools.GetWaitRealtime(delay);
        delayedFunc();
    }
}
