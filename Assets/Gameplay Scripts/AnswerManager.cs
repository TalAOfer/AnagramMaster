using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] List<RectTransform> PremadeAnswerContainers;

    public void Initialize()
    {
        foreach (var container in PremadeAnswerContainers)
        {
            container.gameObject.SetActive(false);
        }
    }

    public IEnumerator OnNewAnswer(List<BankLetter> bankLetters)
    {
        for (int i = 0; i < bankLetters.Count; i++)
        {
            BankLetter bankLetter = bankLetters[i];
            AnswerLetter answerLetter = bankLetter.GuessLetterInstance.AnswerLetter;
            answerLetter.Rect.SetParent(this.transform);
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0 ; i < bankLetters.Count; i++)
        {
            RectTransform container = PremadeAnswerContainers[i];
            container.gameObject.SetActive(true);
        }

        yield return new WaitForEndOfFrame();

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < bankLetters.Count; i++)
        {
            RectTransform container = PremadeAnswerContainers[i];
            BankLetter bankLetter = bankLetters[i];
            AnswerLetter answerLetter = bankLetter.GuessLetterInstance.AnswerLetter;

            answerLetter.SetUsed();
            answerLetter.Rect.SetParent(container);

            Tween tween = answerLetter.Rect.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutQuad);
            
            if (i == 0)
            {
                sequence.Append(tween);
            } 
            
            else
            {
                sequence.Join(tween);
            }
        }

        yield return sequence.Play().WaitForCompletion(); ;
    }
}
