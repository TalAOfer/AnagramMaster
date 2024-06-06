using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject GuessNodePrefab;
    [SerializeField] private List<GuessHistoryNode> PremadeGuessHistoryNodes;
    private Level level;

    private readonly List<GuessHistoryNode> _activeNodes = new();
    public void Initialize(Level level)
    {
        this.level = level;

        int totalNodesRequired = level.correctAnswers.Count + level.NextLetters.Count + 1;

        for (int i = 0; i < PremadeGuessHistoryNodes.Count; i++)
        {
            GuessHistoryNode currentNode = PremadeGuessHistoryNodes[i];

            if (i < totalNodesRequired)
            {
                currentNode.gameObject.SetActive(true);
                _activeNodes.Add(currentNode);

                if (i < level.correctAnswers.Count)
                {
                    currentNode.SetToAnswered(level.correctAnswers[i]);
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
                node.SetToAnswered(level.correctAnswers[^1]);
                break;
            }
        }
    }
}
