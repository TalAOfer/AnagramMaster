using UnityEngine;

public class AssetLocator : MonoBehaviour
{
    private static AssetLocator _instance;

    public static AssetLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                // Look for an existing instance
                _instance = FindObjectOfType<AssetLocator>();

                // If none exists, create a new one
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(AssetLocator).ToString());
                    _instance = singleton.AddComponent<AssetLocator>();
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

    [SerializeField] private AnimationData _animData;
    public AnimationData AnimationData { get { return _animData; } }

    [SerializeField] private LevelBank _levelBank;
    public LevelBank LevelBank { get { return _levelBank; } }
}