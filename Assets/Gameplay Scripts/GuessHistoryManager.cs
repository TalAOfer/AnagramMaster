using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject GuessNodePrefab;
    [SerializeField] private List<GuessHistoryNode> PremadeGuessHistoryNodes;

    private readonly List<GuessHistoryNode> _activeNodes = new();

    private GameData Data => AssetProvider.Instance.Data.Value;
    private BiomeBank BiomeBank => AssetProvider.Instance.BiomeBank;

    public void Initialize()
    {
        Biome currentBiome = BiomeBank.Biomes[Data.BiomeIndex];

        int totalNodesRequired = Data.CorrectAnswers.Count + Data.NextLetters.Count + 1;

        for (int i = 0; i < PremadeGuessHistoryNodes.Count; i++)
        {
            GuessHistoryNode currentNode = PremadeGuessHistoryNodes[i];
            currentNode.Initialize(currentBiome);

            if (i < totalNodesRequired)
            {
                currentNode.gameObject.SetActive(true);
                _activeNodes.Add(currentNode);

                if (i < Data.CorrectAnswers.Count)
                {
                    currentNode.SetToAnswered(Data.CorrectAnswers[i]);
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
                node.SetToAnswered(Data.CorrectAnswers[^1]);
                break;
            }
        }
    }
}
