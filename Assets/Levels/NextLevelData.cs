public class NextLevelData
{
    public LevelIndexHierarchy IndexHierarchy { get; private set; }
    public NextLevelEvent NextLevelEvent { get; private set; }

    public bool ShouldGetGift {  get; private set; }

    public NextLevelData(LevelIndexHierarchy indexHierarchy, BiomeBank BiomeBank)
    {
        Area currentArea = BiomeBank.GetArea(indexHierarchy);
        Biome currentBiome = BiomeBank.Biomes[indexHierarchy.Biome];

        LevelIndexHierarchy nextLevelIndices = new(indexHierarchy.Biome, indexHierarchy.Area, indexHierarchy.Level);

        nextLevelIndices.Level += 1;

        if (nextLevelIndices.Level > currentArea.LastLevelIndex)
        {
            nextLevelIndices.Level = 0;
            nextLevelIndices.Area += 1;
            NextLevelEvent = NextLevelEvent.NewArea;
        }

        if (nextLevelIndices.Area > currentBiome.LastAreaIndex)
        {
            nextLevelIndices.Area = 0;
            nextLevelIndices.Biome += 1;
            NextLevelEvent = NextLevelEvent.NewBiome;
        }

        if (nextLevelIndices.Biome > BiomeBank.LastBiomeIndex)
        {
            NextLevelEvent = NextLevelEvent.FinishedGame;
        }

        IndexHierarchy = nextLevelIndices;
    }
}

public enum NextLevelEvent
{
    None,
    NewArea,
    NewBiome,
    FinishedGame
}

