using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected TextMeshProUGUI Tmp;
    [SerializeField] private bool animateOnClick = true;
    [SerializeField] protected TweenableElement element;

    protected NextLevelData nextLevelData;
    protected GameData Data => AssetProvider.Instance.Data.Value;
    protected BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;
    protected AnimationData AnimationData => AssetProvider.Instance.AnimationData;

    public virtual void Initialize(NextLevelData nextLevelData = null)
    {

    }


    public void OnClick()
    {
        button.enabled = false;
        SoundManager.PlaySound("ButtonClick", transform.position);
        StartCoroutine(ClickSequence());
    }

    public IEnumerator ClickSequence()
    {
        yield return PlayAnimation();
        DoAction();
    }

    public virtual IEnumerator PlayAnimation()
    {
        if (animateOnClick)
        {
            yield return AnimationData.ButtonClickAnimation.PlayAndWait(element);
        }
        else 
        {
            yield break;
        }

    }

    protected virtual void DoAction() { }
}
