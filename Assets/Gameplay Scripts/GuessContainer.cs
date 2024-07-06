using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessContainer : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    public RectTransform Rect {  get { return rect; } }
    [SerializeField] private Image defaultImage;
    [SerializeField] private Image colorBlockImage;
    [SerializeField] private Tweener tweener;
    public Tweener Tweener { get {  return tweener; } }

    [SerializeField] private TextMeshProUGUI hintLetter;
    public bool HintApplied { get; private set; }
    private AnimationData AnimData => AssetProvider.Instance.AnimationData;

    public void Initialize(Color color)
    {
        colorBlockImage.color = color;
    }

    public void SetHintLetter(string letter)
    {
        hintLetter.text = letter.ToUpper();
    }

    public void ToggleHint(bool active)
    {
        HintApplied = active;
        hintLetter.gameObject.SetActive(active);

    }
    public void SetVisualToDefault()
    {
        colorBlockImage.gameObject.SetActive(false);
    }

    public void SetVisualToFull()
    {
        colorBlockImage.gameObject.SetActive(true);
    }

    public void StartCorrectAnswerAnimation()
    {
        tweener.TriggerTween(AnimData.guessCorrectAnimBlueprint);
    }
    public void PlayHintAnimation()
    {
        tweener.TriggerTween(AnimData.guessHintAnimBlueprint);
    }

}
