
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BankLetterState
{
    Selectable,
    First,
    UsedButNotFirst
}

public class BankLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    public TextMeshProUGUI Tmp { get { return tmp; } private set { } }
    [SerializeField] private RectTransform rect;
    public RectTransform Rect { get { return rect; } private set { } }

    [SerializeField] private GameEvent LetterPointerDown;
    [SerializeField] private GameEvent LetterPointerEnter;
    [SerializeField] private GuessLetter guessLetter;
    public GuessLetter GuessLetter { get { return guessLetter; } private set { } }
    
    private BankLetterContainer parentContainer;
    private Color activeContainerColor;
    [SerializeField] private Color ActiveTextColor;
    [SerializeField] private Color DefaultTextColor;
    private LetterBankLineManager lineManager;
    private LevelData LevelData => AssetProvider.Instance.Data.Value.Level;

    public void Initialize(int index, LetterBankLineManager lineManager, Color activeContainerColor)
    {
        this.lineManager = lineManager;
        this.activeContainerColor = activeContainerColor;

        ResetAllNestedLetters();
        ChangeColor(false);

        bool withinRangeOfUseForLevel = index < LevelData.CurrentLetters.Length + LevelData.NextLetters.Count;
        if (withinRangeOfUseForLevel)
        {
            int indexInList;
            string letterText;

            bool withinRangeOfCurrentUse = index < LevelData.CurrentLetters.Length;
            if (withinRangeOfCurrentUse)
            {
                indexInList = index;
                letterText = LevelData.CurrentLetters[indexInList].ToString().ToUpper();
            } 
            
            else
            {
                indexInList = index - LevelData.CurrentLetters.Length;
                letterText = LevelData.NextLetters[indexInList].ToString().ToUpper();
            }

            Tmp.text = letterText;
            GuessLetter.Initialize(this);
        } 
    }

    public void SetAndSaveParentContainer(BankLetterContainer container)
    {
        container.Initialize(this);
        rect.SetParent(container.transform);
        this.parentContainer = container;
    }

    public void ChangeColor(bool toActive) 
    {
        tmp.color = toActive ? ActiveTextColor : DefaultTextColor;
    }

    public BankLetterContainer ToggleContainer(bool toggle)
    {

        Color containerColor = activeContainerColor;
        if (!toggle) { containerColor.a = 0; }
        parentContainer.Image.color = containerColor;

        if (toggle)
        {
            lineManager.AddPoint(parentContainer.Rect.position);
        }

        else
        {
            lineManager.RemovePoint();
        }

        return parentContainer;
    }

    public void OnPointerDown()
    {
        LetterPointerDown.Raise(this, this);
    }

    public void OnPointerEnter()
    {
        LetterPointerEnter.Raise(this, this);
    }


    public void ResetGuessLetterToBankLetterTransform()
    {
        GuessLetter.MoveToParentLetter();
    }

    public void ResetAllNestedLetters()
    {
        GuessLetter.gameObject.SetActive(false);
        GuessLetter.MoveToParentLetter();
        GuessLetter.AnswerLetter.SetUsed(false);
        GuessLetter.AnswerLetter.ResetTransformToZero();
    }


}

