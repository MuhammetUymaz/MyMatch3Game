using System.Collections.Generic;
using AnimationSystem;
using UnityEngine;

public class Falling : PieceSystem, IUpdate, IAnimationEvents
{
    GridManager gridManager;
    int column, row;

    List<FallingAnimation> fallingAnimationChunk;

    bool hasGridInfoCached;

    public Falling()
    {
        fallingAnimationChunk = new List<FallingAnimation>();

        SL.Get<UpdateManager>().observers.Add(this);
    }

    public void UpdateMe()
    {
        if (!hasGridInfoCached)
        {
            gridManager = SL.Get<GridManager>();
            column = gridManager.column;
            row = gridManager.row;

            hasGridInfoCached = true;
        }

        StartToFall(FallingPoint.CurrentTile);
    }

    public void StartToFall(FallingPoint fallingPoint)
    {
        for (int i = 0; i < column; i++)
        {
            int fallingRowDelay = 0;

            for (int j = row - 1; j >= 0; j--)
            {
                Tile tile = gridManager[i, j];

                if (
                    tile.piece != null
                    && (tile.piece.state == PieceState.Idle || tile.piece.state == PieceState.Falling)
                    && tile.piece.GetComponent<FallingComponent>() != null
                )
                {
                    if (IsFallen(tile.piece, fallingPoint, fallingRowDelay))
                        fallingRowDelay++;
                    else
                        fallingRowDelay = 0;
                }
                else
                {
                    fallingRowDelay = 0;
                }
            }
        }
    }

    public bool IsFallen(Piece piece, FallingPoint fallingPoint, int fallingRowDelay)
    {
        //Add condition that if the piece can fall down -for later, if we wanna improve the game (ECS)

        int x = piece.myTile.x;
        int y = piece.myTile.y;

        Tile lastEmptyTile = null;

        for (int j = y+1; j < row; j++)
        {
            Tile tile = gridManager[x, j];

            //Note: Add the condition that the tile can be able to not be usable

            if(tile.isUnusable)
                continue;

            //Old Method
            if (tile.piece != null)
                break;

            //Look at left
            Tile leftTile = gridManager.GetTile(x-1, j);
            Tile rightTile = gridManager.GetTile(x+1, j);

            bool isLeftRightMoving = false;

            //Diagonal Movement -Must be improved
            /*
            if(leftTile != null && leftTile.piece != null)
            {
                FallingComponent examinedComponent = leftTile.piece.GetComponent<FallingComponent>();

                if(examinedComponent == null)
                {
                    x -= 1;
                    lastEmptyTile = tile;
                    isLeftRightMoving = true;
                }

            }
            else if(rightTile != null && rightTile.piece != null)
            {
                FallingComponent examinedComponent = rightTile.piece.GetComponent<FallingComponent>();

                if(examinedComponent == null)
                {
                    x += 1;
                    lastEmptyTile = tile;
                    isLeftRightMoving = true;
                }
            }
            */

            if(!isLeftRightMoving)
                lastEmptyTile = tile;
        }

        if (lastEmptyTile == null)
            return false;

        Tile currentTile = piece.myTile;

        currentTile.SetPiece(null);
        lastEmptyTile.SetPiece(piece);

        if (piece.state == PieceState.Idle)
        {
            int length = lastEmptyTile.y - currentTile.y;

            DoAnimationState1(
                    piece,
                    fallingPoint == FallingPoint.CurrentTile ? currentTile.position : piece.gameObject.transform.position,
                    //currentTile.position, 
                    lastEmptyTile.position,
                    length,
                    fallingRowDelay
            );
        }

        return true;
    }

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }

    //Animation
    public void DoAnimationState1(Piece piece, Vector3 firstPosition, Vector3 lastPosition, int length, int fallingRowDelay)
    {
        GridManager gridManager = SL.Get<GridManager>();
        Tile firstTile = gridManager[0, 0], lastTile = gridManager[0, gridManager.row - 1];
        float distance = lastTile.position.y - firstTile.position.y;
        float posY = lastTile.position.y + distance;

        lastPosition.y = posY;

        piece.animation = SL.Get<AnimationManager>().StartScaleAnimation
        (
            piece.gameObject.transform,
            firstPosition,
            lastPosition,

            1.1f,
            AnimationType.Position,
            EaseTypes.InSine,
            this,
            (fallingRowDelay + 1) * 0.05f
        );

        piece.state = PieceState.Falling;

        FallingAnimation chunk = new FallingAnimation(piece);
        chunk.state = 1;
        fallingAnimationChunk.Add(chunk);
    }

    void DoAnimationState2(FallingAnimation chunk)
    {
        Piece piece = chunk.piece;

        //Method II
        piece.animation = SL.Get<AnimationManager>().StartPositionAnimation
        (
            piece.gameObject.transform,
            piece.myTile.position,
            piece.myTile.position + new Vector3(0, -0.1f, 0),
            0.3f,
            AnimationType.Position,
            EaseTypes.SinWave,
            this
        );

        chunk.state = 2;
    }

    public void AnimationIsCompleted(AnimationSystem.Animation animation)
    {
        for (int i = 0; i < fallingAnimationChunk.Count; i++)
        {
            FallingAnimation chunk = fallingAnimationChunk[i];

            if (chunk.piece.animation != animation)
                continue;

            if (chunk.state == 1)
            {
                DoAnimationState2(chunk);
            }
            else if (chunk.state == 2)
            {
                chunk.piece.state = PieceState.Idle;

                chunk.piece.animation = null;
                //chunk.piece.TempGoToMyTile();
                fallingAnimationChunk.RemoveAt(i);
                return;
            }
        }
    }

    public void AnimationIsUpdating(AnimationSystem.Animation animation)
    {
        for (int i = 0; i < fallingAnimationChunk.Count; i++)
        {
            FallingAnimation chunk = fallingAnimationChunk[i];

            if (chunk.piece.animation != animation)
                continue;

            Piece piece = chunk.piece;

            float lerp = AnimationSystem.Animation.Lerp(animation.initialV3.y, piece.myTile.position.y, piece.gameObject.transform.position.y);

            if (lerp >= 1)
            {
                animation.Stop(false);
                piece.gameObject.transform.position = piece.myTile.position;
            }

            return;
        }
    }

    class FallingAnimation
    {
        public Piece piece;
        public int state;

        public FallingAnimation(Piece piece)
        {
            this.piece = piece;
        }
    }

    public enum 
    FallingPoint
    {
        PiecePosition,
        CurrentTile
    }
}