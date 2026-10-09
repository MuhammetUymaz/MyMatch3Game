using UnityEngine;
using AnimationSystem;

public class Obstacle : Piece
{
    public static int obstacleIDOfLastGeneratingOne; //When make an obstacle, the id is used

    public Obstacle(Vector3 initialPosition) : base(initialPosition)
    {
        AddComponent(new TriggerByNeighbourComponent(this));

        SetGameObject(initialPosition);

        ConfigureObstacle();
    }

    public override void ReduceHealth()
    {        
        if (state != PieceState.Idle)
            return;

        health--;

        if (health <= 0)
        {
            state = PieceState.Exploding;

            animation = SL.Get<AnimationManager>().StartScaleAnimation
            (
                gameObject.transform,
                Mathematics.Vector3One() * DefaultPieceScale(),
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
        }
    }

    void ConfigureObstacle()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = null;

        switch (obstacleIDOfLastGeneratingOne)
        {
            case 0:
                health = 1;
                sprite = Resources.Load<Sprite>("Obstacles/ObstacleA");
                break;
            case 1:
                health = 2;
                sprite = Resources.Load<Sprite>("Obstacles/ObstacleB");

                break;
            case 2:
                health = 3;
                sprite = Resources.Load<Sprite>("Obstacles/ObstacleC");

                break;
        }

        spriteRenderer.sprite = sprite;
    }

    protected override float DefaultPieceScale() => 0.25f;

    protected override GameObject MakeGameObject()
    {
        GameObject sampleGameObject = Resources.Load<GameObject>("Grid/Pieces/Obstacle");

        return UnityEngine.Object.Instantiate(sampleGameObject);
    }
}