using UnityEngine;

public class PowerupO: Piece
{
    public PowerupO(Vector3 initialPosition): base(initialPosition)
    {
        AddComponent(new FallingComponent(this));

        SetGameObject(initialPosition);
    }

    public override void ReduceHealth()
    {
        if(state != PieceState.Idle)
            return;

        GridManager gridManager = SL.Get<GridManager>();

        //Explode pieces
        int x = myTile.x;
        int y = myTile.y;

        state = PieceState.Exploding;

        //Explode along 4 sides
        ExplodePiece(gridManager.GetTile(x+1, y)); //Right
        ExplodePiece(gridManager.GetTile(x-1, y)); //Left
        ExplodePiece(gridManager.GetTile(x, y-1)); //Top
        ExplodePiece(gridManager.GetTile(x, y+1)); //Bottom

        RemovePiece();
    }

    void ExplodePiece(Tile tile)
    {
        if(tile != null && tile.piece != null)
            tile.piece.ReduceHealth();
    }

    protected override GameObject MakeGameObject()
    {
        GameObject sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/Helicopter");

        return UnityEngine.Object.Instantiate(sampleGameObject);
    }
}