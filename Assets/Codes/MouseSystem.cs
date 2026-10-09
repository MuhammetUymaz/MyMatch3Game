using System;
using AnimationSystem;
using UnityEngine;

public class MouseSystem : IUpdate
{
    Tile clickedTileOne, clickedTileTwo;

    bool hasMouseDown;

    public MouseSystem()
    {
        clickedTileOne = null;
        clickedTileTwo = null;

        SL.Get<UpdateManager>().observers.Add(this);
    }

    public void UpdateMe()
    {
        InputType inputType = GetInputType();

        if (inputType == InputType.None)
            return;

        if(SL.Get<AnimationManager>().IsThereActiveAnimation())
            return;

        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (inputType == InputType.Pressing)
        {
            hasMouseDown = true;
        }
        else if (inputType == InputType.Holding)
        {
            PressingEvent(mousePosition);
        }
        else if (inputType == InputType.Up)
        {
            //Running powerup -if it is
            if (clickedTileOne == clickedTileTwo && clickedTileOne != null && clickedTileOne.piece != null)
            {
                //Do selecting operation -for just powerups
                Piece piece = clickedTileOne.piece;
                Type pieceType = piece.GetType();

                if (pieceType == typeof(PowerupLT) || pieceType == typeof(PowerupI) || pieceType == typeof(PowerupO) || pieceType == typeof(PowerupLargeI))
                {
                    piece.ReduceHealth();
                }
            }
            Zero();
        }
    }

    InputType GetInputType()
    {
        if (Input.GetMouseButtonDown(0))
            return InputType.Pressing;
        else if (Input.GetMouseButton(0))
            return InputType.Holding;
        else if (Input.GetMouseButtonUp(0))
            return InputType.Up;


        return InputType.None;
    }

    void PressingEvent(Vector3 mousePosition)
    {
        if (!hasMouseDown)
            return;

        GridManager gridManager = SL.Get<GridManager>();

        int column = gridManager.column;
        int row = gridManager.row;

        Tile currentClickedTile = null;

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Tile tile = gridManager[i, j];

                if (tile.IsClicked(mousePosition))
                {
                    currentClickedTile = tile;
                    break;
                }
            }
        }

        //First Tile
        if (clickedTileOne == null)
        {
            if (currentClickedTile == null)
            {
                Zero();
                return;
            }

            if (currentClickedTile.piece == null)
            {
                Zero();
                return;
            }

            //For Booster -pre checking
            if(SL.Get<BoosterManager>().IsThereWaitingForBooster())
            {
                SL.Get<BoosterManager>().Run(currentClickedTile);

                return;
            }

            clickedTileOne = currentClickedTile;
        }
        //Second tile
        else
        {
            if (currentClickedTile == null)
                return;

            Vector3 direction = (currentClickedTile.position - clickedTileOne.position).normalized;
            float absX = Mathf.Abs(direction.x);
            float absY = Mathf.Abs(direction.y);

            Tile directedTile;
            int directedX, directedY;

            if (absX > absY)
            {
                directedX = clickedTileOne.x + (direction.x > 0 ? 1 : -1);
                directedY = clickedTileOne.y;

                directedTile = gridManager[directedX, directedY];
            }
            else if (absX < absY)
            {
                directedX = clickedTileOne.x;
                directedY = clickedTileOne.y + (direction.y > 0 ? -1 : 1);

                directedTile = gridManager[directedX, directedY];
            }
            //Selecting same tile -powerup
            else if (currentClickedTile == clickedTileOne)
            {
                //Just Go on
                directedTile = clickedTileOne;
            }
            //In diagonal
            else
            {
                return;
            }

            if (directedTile.piece == null)
                return;

            if (clickedTileOne.piece.state != PieceState.Idle || directedTile.piece.state != PieceState.Idle)
                return;

            clickedTileTwo = directedTile;

            UpEvent(mousePosition);
        }
    }

    void UpEvent(Vector3 mousePosition)
    {
        //Update step
        SL.Get<StepManager>().ReduceStep(1);

        //Do swapping
        if (clickedTileOne != clickedTileTwo)
        {
            SL.Get<Swapping>().StartSwapping(clickedTileOne, clickedTileTwo, true);

            //Zeroing
            Zero();
        }
    }

    void Zero()
    {
        clickedTileOne = null;
        clickedTileTwo = null;
        hasMouseDown = false;
    }

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }

    enum InputType
    {
        None,
        Pressing,
        Holding,
        Up
    }
}