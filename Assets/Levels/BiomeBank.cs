using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/BiomeBank")]
public class BiomeBank : ScriptableObject
{
    public List<Biome> Biomes;

    public NextLevelData GetNextLevelData(LevelIndexHierarchy indexHierarchy)
    {
        BiomeArea currentArea = GetArea(indexHierarchy);
        Biome currentBiome = Biomes[indexHierarchy.Biome];

        NextLevelEvent nextLevelEvent = NextLevelEvent.None;
        LevelIndexHierarchy nextLevelIndices = new(indexHierarchy.Biome, indexHierarchy.Area, indexHierarchy.Level);

        nextLevelIndices.Level += 1;
        
        if (nextLevelIndices.Level > currentArea.LastLevelIndex)
        {
            nextLevelIndices.Level = 0;
            nextLevelIndices.Area += 1;
            nextLevelEvent = NextLevelEvent.NewArea;
        }

        if (nextLevelIndices.Area > currentBiome.LastAreaIndex)
        {
            nextLevelIndices.Area = 0;
            nextLevelIndices.Biome += 1;
            nextLevelEvent = NextLevelEvent.NewBiome;
        }

        if (nextLevelIndices.Biome > LastBiomeIndex)
        {
            nextLevelEvent = NextLevelEvent.FinishedGame;
        }

        return new NextLevelData(nextLevelIndices, nextLevelEvent);
    }


    public BiomeArea GetArea(LevelIndexHierarchy indexHierarchy) =>
        Biomes[indexHierarchy.Biome].Areas[indexHierarchy.Area];


    public LevelBlueprint GetLevel(LevelIndexHierarchy indexHierarchy) => 
        Biomes[indexHierarchy.Biome].Areas[indexHierarchy.Area].Levels[indexHierarchy.Level];

    public int LastBiomeIndex => Biomes.Count - 1;
}

public class NextLevelData
{
    public LevelIndexHierarchy LevelIndexHierarchy { get; private set; }
    public NextLevelEvent NextLevelType {  get; private set; }

    public NextLevelData(LevelIndexHierarchy nextLevelIndices, NextLevelEvent nextLevelType)
    {
        LevelIndexHierarchy = nextLevelIndices;
        NextLevelType = nextLevelType;
    }
}

public struct LevelIndexHierarchy
{
    public int Biome { get; set; }
    public int Area { get; set; }
    public int Level { get; set; }

    public LevelIndexHierarchy(int biome, int area, int level)
    {
        Biome = biome;
        Area = area;
        Level = level;
    }
}

public enum NextLevelEvent
{
    None,
    NewArea,
    NewBiome,
    FinishedGame
}

public enum Biomes
{
    Forest,
    Hills,
}