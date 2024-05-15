public static class SyncedRandomGenerator
{
    private static int seedForRandom = 172349;
    private static System.Random random = new System.Random(seedForRandom);
    
    public static void Reload(int seed)
    {
        seedForRandom = seed;
        random = new System.Random(seedForRandom);
    }

    public static int Next()
    {
        return random.Next();
    }

    public static int Next(int max)
    {
        return random.Next(max);
    }

    public static int Next(int min, int max)
    {
        return random.Next(min, max);
    }
}