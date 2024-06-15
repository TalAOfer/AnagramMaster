using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    private LevelBank LevelBank => AssetLocator.Instance.LevelBank;

    [SerializeField] private LevelInitializer LevelInitializer;
    [SerializeField] private ElementFader fader;
    [SerializeField] private bool fromWinning;
    [SerializeField] private TextMeshProUGUI Tmp;
    [SerializeField] private Tweener tweener;
    [SerializeField] private TweenBlueprint buttonClickAnim;

    private GameData data;
    
    private void OnEnable()
    {
        data = SaveSystem.Load();


        bool isFirstStage = !data.IsInitialized;
        if (isFirstStage)
        {
            data = new GameData(0, LevelBank.Value[0]);
            SaveSystem.Save(data);
        } 
        
        else
        {
            bool lastStageDone = data.NextLetters.Count == 0;
            if (lastStageDone)
            {
                int nextIndex = data.Index + 1;
                data = new GameData(nextIndex, LevelBank.Value[nextIndex]);
                SaveSystem.Save(data);
            }
        }

        int indexForText = data.Index + 1;
        Tmp.text = "Level " + indexForText.ToString();   
    }

    public void OnClick()
    {
        LevelInitializer.Initialize(data);
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
