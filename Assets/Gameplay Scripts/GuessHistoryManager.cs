using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject GuessNodePrefab;
    [SerializeField] private List<GuessHistoryNode> PremadeGuessHistoryNodes;
    private GameData data;

    private readonly List<GuessHistoryNode> _activeNodes = new();
    public void Initialize(GameData data)
    {
        this.data = data;

        int totalNodesRequired = data.CorrectAnswers.Count + data.NextLetters.Count + 1;

        for (int i = 0; i < PremadeGuessHistoryNodes.Count; i++)
        {
            GuessHistoryNode currentNode = PremadeGuessHistoryNodes[i];

            if (i < totalNodesRequired)
            {
                currentNode.gameObject.SetActive(true);
                _activeNodes.Add(currentNode);

                if (i < data.CorrectAnswers.Count)
                {
                    currentNode.SetToAnswered(data.CorrectAnswers[i]);
                }
            }

            else
            {
                currentNode.gameObject.SetActive(false);
            }
        }
    }
    public void HandleNewAnsweredNode()
    {
        for (int i = 0; i < _activeNodes.Count; i++)
        {
            GuessHistoryNode node = _activeNodes[i];
            if (node.Answered) continue;
            else
            {
                node.SetToAnswered(data.CorrectAnswers[^1]);
                break;
            }
        }
    }
}
