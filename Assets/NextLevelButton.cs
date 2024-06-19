using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private ElementFader fader;
    [SerializeField] private bool fromWinning;
    [SerializeField] private TextMeshProUGUI Tmp;
    [SerializeField] private Tweener tweener;
    [SerializeField] private TweenBlueprint buttonClickAnim;
    private GameData Data => AssetProvider.Instance.Data.Value;

    public void Initialize()
    {
        Tmp.text = "Level " + (Data.OverallLevelIndex + 1).ToString();
    }

    public void OnClick()
    {
        StartCoroutine(ClickSequence());
    }

    public IEnumerator ClickSequence()
    {
        yield return tweener.TriggerTween(buttonClickAnim).WaitForCompletion();
        if (fromWinning)
        {
            fader.WinningToGameplay();
        } else
        {
            fader.FadeStartMenuToGameplay();
        }
    }
}
