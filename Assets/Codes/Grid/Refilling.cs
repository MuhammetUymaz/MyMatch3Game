using UnityEngine;

public class Refilling : IUpdate
{
    public Refilling()
    {
        SL.Get<UpdateManager>().observers.Add(this);
    }

    public void UpdateMe()
    {
        //Refill
        GridManager gridManager = SL.Get<GridManager>();
        PiecePool piecePool = SL.Get<PiecePool>();
        Falling falling = SL.Get<Falling>();
        
        int column = gridManager.column;
        int row = gridManager.row;

        for (int i = 0; i < column; i++)
        {
            int need = GetNeedCount(gridManager, i, row);

            if(need == 0)
                continue;
            
            PutPieces(gridManager, piecePool, falling, need, i);
        }
    }

    int GetNeedCount(GridManager gridManager, int determinedColumn, int row)
    {
        int need = 0;

        for (int j = 0; j < row; j++)
        {
            Tile tile = gridManager[determinedColumn, j];

            if (tile.piece == null)
                need++;
            else
            //Note: Add some condition to improve playing game experience
                break;
        }

        return need;
    }

    void PutPieces(GridManager gridManager, PiecePool piecePool, Falling falling, int need, int determinedColumn)
    {
        //With Animation Method -it must be improved
        Vector3 refillingPosition = gridManager[determinedColumn, 0].position + new Vector3(0, 1, 0);

        for(int j = 0; j < need; j++)
        {
            Piece newPiece = piecePool.GetPieceRandomPiece<NormalPiece>(refillingPosition);
            Tile tile = gridManager[determinedColumn, (need-1) - j];

            tile.SetPiece(newPiece);

            falling.DoAnimationState1(
                    newPiece,
                    refillingPosition,
                    tile.position,
                    1,
                    j * 2
            );
        }
    }

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }
}
