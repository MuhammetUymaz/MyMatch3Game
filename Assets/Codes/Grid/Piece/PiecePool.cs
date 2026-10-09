using System;
using System.Collections.Generic;
using UnityEngine;

public class PiecePool
{
    List<Piece> pieces;

    public PiecePool()
    {
        pieces = new List<Piece>();
    }

    //Factory
    public Piece GetPieceRandomPiece<T>(Vector3 initialPosition) where T : Piece
    {
        Piece piece = null;
        Type pieceType = typeof(T);

        bool isNormalPiece = pieceType == typeof(NormalPiece);
        int id = UnityEngine.Random.Range(0, 3); //For Normal Piece

        for (int i = 0; i < pieces.Count; i++)
        {
            Piece examinedPiece = pieces[i];

            if (examinedPiece.state != PieceState.Out)
                continue;

            if(examinedPiece.GetType() != typeof(T))
                continue;

            if (isNormalPiece)
            {
                if (((NormalPiece)examinedPiece).id != id)
                    continue;
            }

            //Has found an appropriate one
            piece = examinedPiece;
            break;
        }

        if (piece != null)
        {
            piece.ResetForBeingReused(initialPosition);
        }
        else
        {
            //piece = new Piece(id, initialPosition);
            piece = GetNewPiece<T>(initialPosition, id);
            pieces.Add(piece);
        }

        return piece;
    }

    Piece GetNewPiece<T>(Vector3 initialPosition, int id) where T : Piece
    {
        Type type = typeof(T);

        if (type == typeof(NormalPiece))
        {
            return new NormalPiece(id, initialPosition);
        }
        else if(type == typeof(PowerupLT))
        {
            return new PowerupLT(initialPosition);
        }
        else if(type == typeof(PowerupO))
        {
            return new PowerupO(initialPosition);
        }
        else if(type == typeof(PowerupI))
        {
            return new PowerupI(initialPosition);
        }
        else if(type == typeof(PowerupLargeI))
        {
            return new PowerupLargeI(initialPosition);
        }
        else if(type == typeof(Obstacle))
        {
            return new Obstacle(initialPosition);
        }

        return null;
    }

    public void ThrowPieceDirectly(Piece piece)
    {
        piece.state = PieceState.Out;
        piece.gameObject.SetActive(false);
    }

    //Statistics
    public int GetCountOfNormalPieceOnId(int determinedId)
    {
        //There are Normal Piece along 0, 1, 2
        if(determinedId >= 3)
            return -1;

        int count = 0;
        Type pieceType = typeof(NormalPiece);

        for(int i = 0; i < pieces.Count; i++)
        {
            Piece examinedPiece = pieces[i];

            if (examinedPiece.state != PieceState.Out)
                continue;

            if(examinedPiece.GetType() != pieceType)
                continue;

            if (((NormalPiece)examinedPiece).id != determinedId)
                    continue;

            count++;
        }

        return count;
    }
}
