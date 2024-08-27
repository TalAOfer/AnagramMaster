using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using Sirenix.OdinInspector;

public class ElementController : SerializedMonoBehaviour
{
    public static ElementController Instance { get; private set; }
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;
    [SerializeField] private TranslucentImageSource TranslucentSource;
    [SerializeField] private GameplayManager GameplayManager;

    [SerializeField, DictionaryDrawerSettings(KeyLabel = "Element Data", ValueLabel = "Tweenable Element")]
    private Dictionary<TweenableElementPointer, TweenableElement> tweenableElementDict = new Dictionary<TweenableElementPointer, TweenableElement>();
    [SerializeField] private ElementSuperStates ElementSuperState;

    [SerializeField] private TweenableElementPointer MainBG, SecondaryBG;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want this to persist between scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        AnimData.S_Fade_In.Play();
    }

    [Button]
    public void ChangeElementSuperState()
    {
        ElementVisibilityChart correctElementChart;

        switch (ElementSuperState)
        {
            case ElementSuperStates.GameStart:
                correctElementChart = ElementVisibilityChart.GameStart;
                break;
            case ElementSuperStates.StartMenu:
                correctElementChart = ElementVisibilityChart.StartMenu;
                break;
            case ElementSuperStates.Gameplay:
                correctElementChart = ElementVisibilityChart.Gameplay;
                break;
            case ElementSuperStates.Winning:
                correctElementChart = ElementVisibilityChart.Winning;
                break;
            case ElementSuperStates.Gift:
                correctElementChart = ElementVisibilityChart.Gift;
                break;
            case ElementSuperStates.Animal:
                correctElementChart= ElementVisibilityChart.Animal;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(correctElementChart), ElementSuperState, null);
        }

        foreach (var entry in tweenableElementDict)
        {
            TweenableElementPointer data = entry.Key;
            TweenableElement element = entry.Value;


            bool shouldEnable = data.ElementVisibilityChart.HasFlag(correctElementChart);
            ToggleElement(element, shouldEnable);
        }

    }

    public TweenableElement GetElement(TweenableElementPointer elementSO)
    {
        if (elementSO == null)
        {
            Debug.Log("Element wasn't assigned in the animation sequence asset");
            return null;
        }

        if (tweenableElementDict.TryGetValue(elementSO, out var element) && element != null)
        {
            return element;
        }
        else
        {
            Debug.LogWarning($"Tweenable element for '{elementSO.name}' not found.");
            return null;
        }
    }

    public void SwitchBackgroudPointers()
    {
        if (!tweenableElementDict.ContainsKey(MainBG))
        {
            Debug.LogWarning("MainBG not found in the dictionary.");
            return;
        }

        if (!tweenableElementDict.ContainsKey(SecondaryBG))
        {
            Debug.LogWarning("SecondaryBG not found in the dictionary.");
            return;
        }

        TweenableElement mainBGElement = tweenableElementDict[MainBG];
        TweenableElement secondaryBGElement = tweenableElementDict[SecondaryBG];

        tweenableElementDict[MainBG] = secondaryBGElement;
        tweenableElementDict[SecondaryBG] = mainBGElement;
    }


    [Button]
    public void PopulateAllElementInnerVariables()
    {
        TweenableElement[] elements = FindObjectsOfType<TweenableElement>(true);
        foreach (TweenableElement element in elements)
        {
            element.PopulateVariables();
        }
    }

    public void ToggleElement(TweenableElement element, bool toggle)
    {
        int alpha = toggle ? 1 : 0;
        element.CanvasGroup.alpha = alpha;
        element.gameObject.SetActive(toggle);
    }

    private enum ElementSuperStates
    {
        GameStart = 1,
        StartMenu = 2,
        Gameplay = 4,
        Winning = 8,
        Gift = 16,
        Animal = 32,
    }
}
