using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Biome")]
public class Biome : ScriptableObject
{
    public Sprite FullCollectible;
    public Sprite EmptyCollectible;

    public List<BiomeArea> Areas;

    public int LastAreaIndex => Areas.Count - 1;

    public int GetLevelAmount()
    {
        int levelAmount = 0;
        
        foreach (var area in Areas)
        {
            levelAmount += area.Levels.Count;
        }

        return levelAmount;
    }

    public int GetCurrentCollectibleAmount(LevelIndexHierarchy indices)
    {
        int collectibleAmount = 0;

        for (int areaIndex = 0; areaIndex <= indices.Area; areaIndex++)
        {
            BiomeArea area = Areas[areaIndex];

            // If this is the specified area, only sum levels up to and including the specified level
            if (areaIndex == indices.Area)
            {
                for (int levelIndex = 0; levelIndex < indices.Level; levelIndex++)
                {
                    LevelBlueprint level = area.Levels[levelIndex];
                    collectibleAmount += level.StageAmount;
                }
            }
            else
            {
                // Otherwise, sum all levels in the current area
                foreach (LevelBlueprint level in area.Levels)
                {
                    collectibleAmount += level.StageAmount;
                }
            }
        }

        return collectibleAmount;
    }

    public int GetTotalCollectibleAmount()
    {
        int collectibleAmount = 0;

        foreach (BiomeArea area in Areas)
        {
            foreach (LevelBlueprint level in area.Levels)
            {
                collectibleAmount += level.StageAmount;
            }
        }

        return collectibleAmount;
    }
}

[Serializable]
public class BiomeArea
{
    public Sprite Sprite;
    public List<LevelBlueprint> Levels;
    public Color LetterContainerBGColor;
    public int LastLevelIndex => Levels.Count - 1;
}