public class PlayersManager
{
    static int s_numPlayers = 1;
    const int kMinNumPlayers = 1;

    public static void AddPlayer()
    {
        ++s_numPlayers;
    }

    public static void RemovePlayer()
    {
        --s_numPlayers;
        if (s_numPlayers < kMinNumPlayers)
            s_numPlayers = kMinNumPlayers;
    }

    public static int GetNumPlayers()
    {
        return s_numPlayers;
    }
}
