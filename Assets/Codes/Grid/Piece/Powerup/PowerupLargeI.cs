using System;
using UnityEngine;

public class PowerupLargeI : Piece
{
    public PowerupLargeI(Vector3 initialPosition) : base(initialPosition)
    {
        AddComponent(new FallingComponent(this));

        SetGameObject(initialPosition);
    }

    public override void ReduceHealth()
    {
        if(state != PieceState.Idle)
            return;

        state = PieceState.Exploding;

        GridManager gridManager = SL.Get<GridManager>();
        int column = gridManager.column;
        int row = gridManager.row;

        int targetId = GetTargetId();

        Type pieceType = typeof(NormalPiece);

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Tile tile = gridManager[i, j];

                if (
                    tile != null && tile.piece != null
                    && tile.piece.GetType() == pieceType
                    && tile.piece.state == PieceState.Idle
                    && ((NormalPiece)tile.piece).id == targetId
                    )
                {
                    tile.piece.ReduceHealth();
                }
            }
        }

        RemovePiece();
    }

    protected override GameObject MakeGameObject()
    {
        GameObject sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/Magnet");

        return UnityEngine.Object.Instantiate(sampleGameObject);
    }

    int GetTargetId()
    {
        //Select the most repeated one
        PiecePool piecePool = SL.Get<PiecePool>();

        int type0 = piecePool.GetCountOfNormalPieceOnId(0);
        int type1 = piecePool.GetCountOfNormalPieceOnId(1);
        int type2 = piecePool.GetCountOfNormalPieceOnId(2);

        if (type0 >= type1 && type0 >= type2)
            return 0;
        else if (type1 >= type0 && type1 >= type2)
            return 1;
        else
            return 2;
    }
}