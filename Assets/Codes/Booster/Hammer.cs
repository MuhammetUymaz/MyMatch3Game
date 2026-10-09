using AnimationSystem;

partial class BoosterManager
{
    public void Hammer()
    {
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        if(currentBooster != BoosterType.None)
            return;

        if(SL.Get<AnimationManager>().IsThereActiveAnimation())
            return;

        currentBooster = BoosterType.Hammer;

        SL.Get<InfoManager>().SetInfo("Select a tile");
    }

    void DestroyPiece(Tile selectedTile)
    {
        Piece piece = selectedTile.piece;
        
        if(piece != null)
            piece.ReduceHealth();
    }
}