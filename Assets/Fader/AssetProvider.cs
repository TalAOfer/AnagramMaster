using UnityEngine;

public class AssetProvider : MonoBehaviour
{
    private static AssetProvider _instance;

    public static AssetProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                // Look for an existing instance
                _instance = FindObjectOfType<AssetProvider>();

                // If none exists, create a new one
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(AssetProvider).ToString());
                    _instance = singleton.AddComponent<AssetProvider>();
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

    [SerializeField] private BiomeBank _biomeBank;
    public BiomeBank BiomeBank { get { return _biomeBank; } }
    [SerializeField] private CurrentData _data;
    public CurrentData Data { get { return _data; } }
}