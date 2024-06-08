using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private CurrentLevel CurrentLevel;
    [SerializeField] private LevelBank LevelBank;
    [SerializeField] private LevelInitializer LevelInitializer;
    [SerializeField] private TextMeshProUGUI Tmp;
    
    private void OnEnable()
    {
        Debug.Log("happened");
        bool isFirstStage = !CurrentLevel.IsInitialized;
        if (isFirstStage)
        {
            CurrentLevel.IsInitialized = true;
            CurrentLevel.Value = new Level(0, LevelBank.Value[0]);
        } 
        
        else
        {
            bool lastStageDone = CurrentLevel.Value.NextLetters.Count == 0;
            if (lastStageDone)
            {
                int nextIndex = CurrentLevel.Value.Index + 1;
                CurrentLevel.Value = new Level(nextIndex, LevelBank.Value[nextIndex]);
            }
        }

        LevelInitializer.Initialize(CurrentLevel.Value);
        int indexForText = CurrentLevel.Value.Index + 1;
        Tmp.text = "Level " + indexForText.ToString();   
    }
}
