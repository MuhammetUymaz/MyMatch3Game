using UnityEngine;

public class HintSystem : ITimerEvents
{
    Timer timer;

    float limitedTime = 3;

    Tile tile1, tile2;

    public void Start()
    {
        ResetTimer();
    }

    void ResetTimer()
    {
        if (timer == null)
        {
            timer = SL.Get<TimerManager>().RunTimer(limitedTime, this);
        }
        else
        {
            timer.Reset(limitedTime, this);
        }
    }

    public void TimerIsUpdating(Timer timer)
    {
        //Check if there is any piece on animation -to reset timer
        GridManager gridManager = SL.Get<GridManager>();
        int column = gridManager.column, row = gridManager.row;

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Tile tile = gridManager[i, j];
                Piece piece = tile.piece;

                if (piece != null)
                {
                    if (piece.state != PieceState.Idle)
                    {
                        ResetTimer();
                        return;
                    }
                }
            }
        }
    }

    public void TimerIsDone(Timer timer)
    {
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        Scan();

        //Temporary efect
        tile1.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        tile2.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        Matching.CurrentMatchingData currentMatchingData = SL.Get<Matching>().currentMatchingData;
    }

    //Scan

    void Scan()
    {
        //Check if there is any piece on animation -to reset timer
        GridManager gridManager = SL.Get<GridManager>();
        int column = gridManager.column, row = gridManager.row;

        bool isThereAnyMatching;

        for (int i = 0; i < column - 1; i++)
        {
            for (int j = 0; j < row - 1; j++)
            {
                Tile tile = gridManager[i, j];

                //Check on horizontal
                Tile rightTile = gridManager[i + 1, j];

                if (rightTile != null)
                {
                    Swap(tile, rightTile);

                    isThereAnyMatching = IsThereAnyMatching(tile, rightTile);

                    //Re-Swap
                    Swap(tile, rightTile);

                    if (isThereAnyMatching)
                    {
                        tile1 = tile;
                        tile2 = rightTile;
                        return;
                    }
                }

                //Vertical
                Tile bottomTile = gridManager[i + 1, j];
                if (bottomTile != null)
                {
                    Swap(tile, bottomTile);

                    isThereAnyMatching = IsThereAnyMatching(tile, bottomTile);

                    //Re-Swap
                    Swap(tile, bottomTile);

                    if (isThereAnyMatching)
                    {
                        tile1 = tile;
                        tile2 = bottomTile;
                        return;
                    }
                }

            }
        }
    }

    void Swap(Tile tileOne, Tile tileTwo)
    {
        Piece pieceOne = tileOne.piece, pieceTwo = tileTwo.piece;

        tileOne.SetPiece(pieceTwo);
        tileTwo.SetPiece(pieceOne);
    }

    bool IsThereAnyMatching(Tile tileOne, Tile tileTwo)
    {
        PowerupDesigner powerupDesigner = SL.Get<PowerupDesigner>();
        Matching matching = SL.Get<Matching>();

        bool isTherePowerupDesigning = powerupDesigner.IsThereDesign(tileOne, false) || powerupDesigner.IsThereDesign(tileTwo, false);

        if (isTherePowerupDesigning)
            return true;

        bool isThereNormalMatching = matching.IsMatchingPossible(tileOne, Matching.CheckingType.JustChecking) || matching.IsMatchingPossible(tileTwo, Matching.CheckingType.JustChecking);

        if (isThereNormalMatching)
            return true;

        return false;
    }
}