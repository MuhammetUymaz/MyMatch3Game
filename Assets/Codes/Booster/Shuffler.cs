using UnityEngine;
using AnimationSystem;

partial class BoosterManager
{
    public void Shuffler()
    {
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        if(currentBooster != BoosterType.None)
            return;

        currentBooster = BoosterType.Shuffler;
        
        Shuffle();

        currentBooster = BoosterType.None;

        SL.Get<InfoManager>().ResetInfo();
    }

    void Shuffle()
    {
        shufflingList.Clear();

        GridManager gridManager = SL.Get<GridManager>();

        for(int i = 0; i < gridManager.column; i++)
        {
            for(int j = 0; j < gridManager.row; j++)
            {
                Tile tile = gridManager.GetTile(i, j);

                if(tile != null && tile.piece != null && tile.piece.GetType() == typeof(NormalPiece))
                {
                    shufflingList.Add(tile.piece);
                }
            }
        }

        while(shufflingList.Count > 1)
        {
            Piece pieceOne = GetRandomPieceByRemovingItFromShuflingList();
            Piece pieceTwo = GetRandomPieceByRemovingItFromShuflingList();

            Tile tileOne = pieceOne.myTile;
            Tile tileTwo = pieceTwo.myTile;

            tileOne.SetPiece(pieceTwo);
            tileTwo.SetPiece(pieceOne);

            pieceOne.animation = SL.Get<AnimationManager>().StartPositionAnimation
            (
                pieceOne.gameObject.transform,
                pieceOne.gameObject.transform.position,
                pieceOne.myTile.position,
                0.6f,
                AnimationType.Position,
                EaseTypes.InOutSine,
                null
            );

            pieceTwo.animation = SL.Get<AnimationManager>().StartPositionAnimation
            (
                pieceTwo.gameObject.transform,
                pieceTwo.gameObject.transform.position,
                pieceTwo.myTile.position,
                0.6f,
                AnimationType.Position,
                EaseTypes.InOutSine,
                null
            );
        }
    }

    Piece GetRandomPieceByRemovingItFromShuflingList()
    {
        int listCount = shufflingList.Count; 

        if(listCount == 0)
            return null;

        int randomNumber = Random.Range(0, listCount-1);

        Piece randomPiece = shufflingList[randomNumber];
        shufflingList.RemoveAt(randomNumber);

        return randomPiece;
    }
}