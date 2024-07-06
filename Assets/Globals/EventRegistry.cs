using UnityEngine;

//[CreateAssetMenu(menuName ="Events/Registry")]
public class EventRegistry : ScriptableObject
{
    public GameEvent OnStartButtonPressed;
    public GameEvent OnInputPressed;
    public GameEvent OnInputReleased;
    public GameEvent OnHintClicked;
    public GameEvent OnHintUsed;
}
