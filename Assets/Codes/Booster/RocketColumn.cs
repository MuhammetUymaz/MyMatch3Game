using AnimationSystem;

partial class BoosterManager
{
    public void RocketColumn()
    {
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        if(currentBooster != BoosterType.None)
            return;

        if(SL.Get<AnimationManager>().IsThereActiveAnimation())
            return;

        currentBooster = BoosterType.RocketColumn;

        SL.Get<InfoManager>().SetInfo("Select a tile");
    }

    void DestroyColumn(int y)
    {
        GridManager gridManager = SL.Get<GridManager>();

        int column = gridManager.column;

        for (int i = 0; i < column; i++)
        {
            Tile tile = gridManager[i, y];

            Piece piece = tile.piece;

            if (piece == null)
                continue;

            piece.ReduceHealth();
        }
    }
}