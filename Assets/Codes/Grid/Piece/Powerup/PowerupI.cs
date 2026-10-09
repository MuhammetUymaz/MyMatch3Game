using UnityEngine;

public class PowerupI: Piece
{
    public PowerupI(Vector3 initialPosition) : base(initialPosition)
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

        int x = myTile.x;

        for(int j = 0; j < gridManager.row; j++)
        {
            Tile tile = gridManager[x, j];
            if(tile != null && tile.piece != null)
                tile.piece.ReduceHealth();
        }

        RemovePiece();
    }

    protected override GameObject MakeGameObject()
    {
        GameObject sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/Projectile");

        return UnityEngine.Object.Instantiate(sampleGameObject);
    }
}
