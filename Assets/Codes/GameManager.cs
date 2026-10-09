public class GameManager
{
    public GameMode mode { get; private set; }
    public GameState state;
    public GameManager()
    {
        //Note: This data should be fetched from level data
        mode = GameMode.MatchingGoalBased;
        state = GameState.Playing;

        SL.Get<InfoManager>().ResetInfo();
    }
}