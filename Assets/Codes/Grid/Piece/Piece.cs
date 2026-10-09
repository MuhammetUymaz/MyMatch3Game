using UnityEngine;
using AnimationSystem;
using System.Collections.Generic;
using System;

public abstract class Piece : IAnimationEvents
{
    public GameObject gameObject { get; private set; }

    //The value must be changed by JUST "Tile" class. Otherwise there can happen confusion the algorithm
    public Tile myTile;

    public AnimationSystem.Animation animation;

    public PieceState state;

    public int health { get; protected set; }

    public List<PieceComponent> components;


    public Piece(Vector3 initialPosition)
    {
        state = PieceState.Idle;

        health = 1; //Default health

        components = new List<PieceComponent>();
    }

    public virtual void AnimationIsCompleted(AnimationSystem.Animation animation) { }
    public virtual void AnimationIsUpdating(AnimationSystem.Animation animation) { }


    public void RemovePiece()
    {
        gameObject.SetActive(false);

        state = PieceState.Out;
        this.animation = null;
        myTile.SetPiece(null); //Out
    }

    //Note: Below function is temporary
    public void TempGoToMyTile()
    {
        gameObject.transform.position = myTile.position;
    }

    public void ResetForBeingReused(Vector3 initialPosition)
    {
        state = PieceState.Idle;
        myTile = null;

        Transform transform = gameObject.transform;
        transform.localScale = Mathematics.Vector3One() * DefaultPieceScale();
        transform.position = initialPosition;

        gameObject.SetActive(true);

        Reset();
    }

    //Entity Component System
    protected void AddComponent(PieceComponent newComponent)
    {
        //Every component must be added just 1 time to avoid confusing
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i].GetType() == newComponent.GetType())
                return;
        }

        components.Add(newComponent);
    }

    public T GetComponent<T>() where T : PieceComponent
    {
        Type type = typeof(T);

        for (int i = 0; i < components.Count; i++)
        {
            if (type == components[i].GetType())
                return (T)components[i];
        }

        return null;
    }

    protected void SetGameObject(Vector3 position)
    {
        gameObject = MakeGameObject();
        gameObject.transform.position = position;
        gameObject.transform.localScale = Mathematics.Vector3One() * DefaultPieceScale();
    }

    protected virtual float DefaultPieceScale() => TileStyle.pieceScaleCoefficient;

    protected virtual void Reset() {}
    protected abstract GameObject MakeGameObject();
    public abstract void ReduceHealth();

}