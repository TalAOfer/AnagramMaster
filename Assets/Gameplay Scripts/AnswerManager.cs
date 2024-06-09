using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> PremadeAnswerContainers;
    [SerializeField] private LetterBank letterBank;

    public void Initialize(Level level)
    {
        string lastAnswer = level.correctAnswers.Count > 0 ? level.correctAnswers[^1] : "";

        for (int i = 0; i < PremadeAnswerContainers.Count; i++)
        {
            RectTransform container = PremadeAnswerContainers[i];

            if (i < lastAnswer.Length)
            {
                container.gameObject.SetActive(true);
                BankLetter bankLetter = letterBank.FindLetter(lastAnswer[i].ToString());
                AnswerLetter answerLetter = bankLetter.GuessLetter.AnswerLetter;
                answerLetter.SetUsed(true);
                answerLetter.Rect.SetParent(container);
                answerLetter.ResetTransformToZero();
            } 
            
            else
            {
                container.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator OnNewAnswer(List<BankLetter> bankLetters)
    {
        for (int i = 0; i < bankLetters.Count; i++)
        {
            BankLetter bankLetter = bankLetters[i];
            AnswerLetter answerLetter = bankLetter.GuessLetter.AnswerLetter;
            answerLetter.Rect.SetParent(this.transform);
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < bankLetters.Count; i++)
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
            AnswerLetter answerLetter = bankLetter.GuessLetter.AnswerLetter;

            answerLetter.SetUsed(true);
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
