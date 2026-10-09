using UnityEngine;

public class PowerupLT: Piece
{
    public PowerupLT(Vector3 initialPosition): base(initialPosition)
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

        //Explode pieces
        int x = myTile.x;
        int y = myTile.y;

        for(int i = x-2; i < x+2; i++)
        {
            for(int j = y-2; j < y+2; j++)
            {
                Tile tile = gridManager.GetTile(i, j);
                
                if(tile != null && tile.piece != null)
                {
                    tile.piece.ReduceHealth();
                }
            }
        }

        RemovePiece();
    }

    protected override GameObject MakeGameObject()
    {
        GameObject sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/Dynamite");

        return UnityEngine.Object.Instantiate(sampleGameObject);
    }
}