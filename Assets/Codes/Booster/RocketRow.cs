using AnimationSystem;

partial class BoosterManager
{
    public void RocketRow()
    {
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        if(currentBooster != BoosterType.None)
            return;

        if(SL.Get<AnimationManager>().IsThereActiveAnimation())
            return;

        currentBooster = BoosterType.RocketRow;

        SL.Get<InfoManager>().SetInfo("Select a tile");
    }
    
    void DestroyRow(int x)
    {
        GridManager gridManager = SL.Get<GridManager>();

        int row = gridManager.row;

        for (int j = 0; j < row; j++)
        {
            Tile tile = gridManager[x, j];

            Piece piece = tile.piece;


            if (piece == null)
                continue;

            piece.ReduceHealth();
        }
    }
}