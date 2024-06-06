using System.Collections.Generic;
using UnityEngine;

public class DictionaryLoader : MonoBehaviour
{
    private static DictionaryLoader _instance;
    public static DictionaryLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                // Create a new GameObject and attach the DictionaryLoader component
                GameObject singletonObject = new GameObject("DictionaryLoaderSingleton");
                _instance = singletonObject.AddComponent<DictionaryLoader>();
                DontDestroyOnLoad(singletonObject); // Make the singleton persist across scenes
            }
            return _instance;
        }
    }

    private HashSet<string> wordSet;
    private bool isInitialized = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Make the singleton persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized)
            return;

        wordSet = new HashSet<string>();

        TextAsset dictionaryText = Resources.Load<TextAsset>("google-10000-english-no-swears");
        if (dictionaryText != null)
        {
            string[] words = dictionaryText.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                wordSet.Add(word.ToLower());
            }

            isInitialized = true;
        }
        else
        {
            Debug.LogError("Dictionary file not found!");
        }
    }

    public bool IsWordValid(string word)
    {
        if (!isInitialized)
        {
            Initialize();
        }

        return wordSet.Contains(word.ToLower());
    }
}