using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected ElementController elementController;
    [SerializeField] protected TextMeshProUGUI Tmp;
    [SerializeField] private Tweener tweener;
    [SerializeField] private TweenBlueprint buttonClickAnim;

    protected NextLevelData nextLevelData;
    protected GameData Data => AssetProvider.Instance.Data.Value;
    protected BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

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
        yield return tweener.TriggerTween(buttonClickAnim).WaitForCompletion();
        DoAction();
    }

    protected virtual void DoAction() { }
}
