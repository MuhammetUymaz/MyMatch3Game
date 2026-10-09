using UnityEngine;
using AnimationSystem;

public class NormalPiece : Piece
{
    public int id { get; private set; }
    FallingComponent falling;

    public NormalPiece(int id, Vector3 initialPosition) : base(initialPosition)
    {
        this.id = id;

        AddComponent(new FallingComponent(this));

        SetGameObject(initialPosition);
    }

    protected override GameObject MakeGameObject()
    {
        GameObject sampleGameObject = null;

        switch (id)
        {
            case 0:
                sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/CandyA");
                break;

            case 1:
                sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/CandyB");
                break;

            case 2:
                sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/CandyC");
                break;
        }

        return UnityEngine.Object.Instantiate(sampleGameObject);
    }

    public override void ReduceHealth()
    {        
        if(state != PieceState.Idle)
            return;
            
        health--;

        if (health <= 0)
        {
            state = PieceState.Exploding;

            animation = SL.Get<AnimationManager>().StartScaleAnimation
            (
                gameObject.transform,
                Mathematics.Vector3One() * TileStyle.pieceScaleCoefficient,
                Mathematics.Vector3One() * 0,
                0.2f,
                AnimationType.Scale,
                EaseTypes.OutSine,
                this
            );
        }
    }

    public override void AnimationIsCompleted(AnimationSystem.Animation animation)
    {
        if (state == PieceState.Exploding)
        {
            RemovePiece();

            //Affect obstacles
            int x = myTile.x;
            int y = myTile.y;

            GridManager gridManager = SL.Get<GridManager>();
            TryReduceHealthOfObstacles(gridManager.GetTile(x+1, y));
            TryReduceHealthOfObstacles(gridManager.GetTile(x-1, y));
            TryReduceHealthOfObstacles(gridManager.GetTile(x, y-1));
            TryReduceHealthOfObstacles(gridManager.GetTile(x, y+1));

            //Reduce on task -if this normal piece is a task
        }
    }

    void TryReduceHealthOfObstacles(Tile tile)
    {
        if(tile == null || tile.piece == null)
            return;
        
        TriggerByNeighbourComponent component = tile.piece.GetComponent<TriggerByNeighbourComponent>();

        if(component == null)
            return;
                
        SL.Get<TriggerByNeighbour>().Run(component);
    }

}