using System;
using System.Collections.Generic;
using AnimationSystem;
using UnityEngine;

public class Swapping : IAnimationEvents
{
    List<SwappingPair> swappingPairs;

    public Swapping()
    {
        swappingPairs = new List<SwappingPair>();
    }

    public void StartSwapping(Tile tileOne, Tile tileTwo, bool shouldTryToMatch)
    {
        int dX = Mathf.Abs(tileOne.x - tileTwo.x);
        int dY = Mathf.Abs(tileOne.y - tileTwo.y);

        bool areTheyNeighbour = (dX == 1 && dY == 0) || (dX == 0 && dY == 1);

        if (!areTheyNeighbour)
            return;

        swappingPairs.Add(new SwappingPair(tileOne.piece, tileTwo.piece, shouldTryToMatch, SwappingPair.Type.Forwarding));

        Swap(tileOne, tileTwo);
    }

    void Swap(Tile tileOne, Tile tileTwo)
    {
        Piece pieceOne = tileOne.piece, pieceTwo = tileTwo.piece;

        tileOne.SetPiece(pieceTwo);
        tileTwo.SetPiece(pieceOne);

        pieceOne.state = PieceState.Swapping;
        pieceTwo.state = PieceState.Swapping;

        DoAnimation(pieceOne);
        DoAnimation(pieceTwo);
    }

    void DoAnimation(Piece piece)
    {
        piece.animation = SL.Get<AnimationManager>().StartPositionAnimation
        (
            piece.gameObject.transform,
            piece.gameObject.transform.position,
            piece.myTile.position,
            0.2f,
            AnimationType.Position,
            EaseTypes.InSine,
            this
        );
    }

    public void AnimationIsCompleted(AnimationSystem.Animation animation)
    {
        for (int i = 0; i < swappingPairs.Count; i++)
        {
            SwappingPair pair = swappingPairs[i];

            if (pair.pieceOne.animation == animation || pair.pieceTwo.animation == animation)
            {
                pair.arrivedPieceCount++;

                if (pair.arrivedPieceCount == 2)
                {
                    if (pair.type == SwappingPair.Type.Forwarding)
                    {
                        Tile tileOne = pair.pieceOne.myTile;
                        Tile tileTwo = pair.pieceTwo.myTile;

                        pair.pieceOne.state = PieceState.Idle;
                        pair.pieceTwo.state = PieceState.Idle;

                        if (ShouldContinueForSwapping(tileOne, tileTwo))
                        {
                            pair.pieceOne.animation = null;
                            pair.pieceTwo.animation = null;

                            swappingPairs.RemoveAt(i);

                            if (pair.shouldTryToMatch)
                            {
                                SL.Get<Matching>().MatchSpecificTile(tileOne);
                                SL.Get<Matching>().MatchSpecificTile(tileTwo);
                            }

                            SL.Get<TaskManager>().currentTask.OperateTask();

                        }
                        else
                        {
                            //Reverse
                            pair.arrivedPieceCount = 0;
                            pair.type = SwappingPair.Type.Backwarding;
                            Swap(tileOne, tileTwo);
                        }

                    }
                    else //Backwarding
                    {
                        pair.pieceOne.animation = null;
                        pair.pieceTwo.animation = null;

                        pair.pieceOne.state = PieceState.Idle;
                        pair.pieceTwo.state = PieceState.Idle;

                        swappingPairs.RemoveAt(i);
                    }
                }

                return;
            }
        }
    }

    public void AnimationIsUpdating(AnimationSystem.Animation animation) { }

    bool ShouldContinueForSwapping(Tile tileOne, Tile tileTwo)
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

    class SwappingPair
    {
        public Piece pieceOne;
        public Piece pieceTwo;

        public int arrivedPieceCount = 0;
        public bool shouldTryToMatch;
        public Type type;

        public SwappingPair(Piece pieceOne, Piece pieceTwo, bool shouldTryToMatch, Type type)
        {
            this.pieceOne = pieceOne;
            this.pieceTwo = pieceTwo;
            this.shouldTryToMatch = shouldTryToMatch;
            this.type = type;
        }

        public enum Type
        {
            Forwarding,
            Backwarding
        }
    }
}