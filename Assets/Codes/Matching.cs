using System.Collections.Generic;
using AnimationSystem;

public class Matching : IAnimationEvents, IUpdate
{
    List<Piece> matchingPieces;
    public CurrentMatchingData currentMatchingData { get; private set; }
    PowerupDesigner powerupDesigner;


    public Matching()
    {
        matchingPieces = new List<Piece>();
        currentMatchingData = new CurrentMatchingData();

        powerupDesigner = SL.Get<PowerupDesigner>();
        SL.Get<UpdateManager>().observers.Add(this);
    }

    public bool IsMatchingPossible(Tile tile, CheckingType checkingType)
    {
        if (tile.piece == null)
            return false;

        if (tile.piece.GetType() != typeof(NormalPiece))
            return false;

        GridManager gridManager = SL.Get<GridManager>();

        //Horizontal
        return Horizontal(tile, gridManager, checkingType) || Vertical(tile, gridManager, checkingType);
    }

    bool Horizontal(Tile tile, GridManager gridManager, CheckingType checkingType)
    {
        return
            CheckPossibilities(tile, tile.x + 1, tile.y, tile.x + 2, tile.y, gridManager, checkingType) || //In most left
            CheckPossibilities(tile, tile.x - 1, tile.y, tile.x + 1, tile.y, gridManager, checkingType) || //In between
            CheckPossibilities(tile, tile.x - 1, tile.y, tile.x - 2, tile.y, gridManager, checkingType);   //In most right
    }

    bool Vertical(Tile tile, GridManager gridManager, CheckingType checkingType)
    {
        return
            CheckPossibilities(tile, tile.x, tile.y + 1, tile.x, tile.y + 2, gridManager, checkingType) || //In top
            CheckPossibilities(tile, tile.x, tile.y - 1, tile.x, tile.y + 1, gridManager, checkingType) || //In between
            CheckPossibilities(tile, tile.x, tile.y - 1, tile.x, tile.y - 2, gridManager, checkingType);   //In bottom
    }

    bool CheckPossibilities(Tile tile, int otherX1, int otherY1, int otherX2, int otherY2, GridManager gridManager, CheckingType checkingType)
    {
        Tile otherTile1 = gridManager.GetTile(otherX1, otherY1);
        Tile otherTile2 = gridManager.GetTile(otherX2, otherY2);

        if (otherTile1 == null || otherTile2 == null)
            return false;

        if (otherTile1.piece == null || otherTile2.piece == null)
            return false;

        if (otherTile1.piece.GetType() != typeof(NormalPiece) || otherTile2.piece.GetType() != typeof(NormalPiece))
            return false;

        //Note: Add "being on Animation" state (checking)

        if (tile.piece.state != PieceState.Idle || otherTile1.piece.state != PieceState.Idle || otherTile2.piece.state != PieceState.Idle)
            //The matching is done via swapping. So base tile must be on swapping ,while other tiles must be on Idle
            return false;

        bool result = AreTheyOnSameType((NormalPiece)tile.piece, (NormalPiece)otherTile1.piece, (NormalPiece)otherTile2.piece);

        if (result && checkingType == CheckingType.ToMatchDirectly)
        {
            currentMatchingData.tile1 = tile;
            currentMatchingData.tile2 = otherTile1;
            currentMatchingData.tile3 = otherTile2;
        }

        return result;
    }

    bool AreTheyOnSameType(NormalPiece pieceFirst, NormalPiece pieceSecond, NormalPiece pieceThird)
    {
        return pieceFirst.id == pieceSecond.id && pieceFirst.id == pieceThird.id;
    }

    //Match Completely -Note: This method should be seperated
    void Match()
    {
        if(SL.Get<AnimationManager>().IsThereActiveAnimation())
            return;

        GridManager gridManager = SL.Get<GridManager>();
        int column = gridManager.column;
        int row = gridManager.row;

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Tile tile = gridManager[i, j];

                //First look at for powerup designer, then look at for normal matching
                if (!powerupDesigner.IsThereDesign(tile, true))
                {
                    currentMatchingData.Reset();

                    if (IsMatchingPossible(tile, CheckingType.ToMatchDirectly))
                    {
                        DoAnimation(currentMatchingData.tile1.piece);
                        DoAnimation(currentMatchingData.tile2.piece);
                        DoAnimation(currentMatchingData.tile3.piece);
                    }
                }
            }
        }
    }

    public void MatchSpecificTile(Tile tile)
    {
        //First look at for powerup designer, then look at for normal matching
        if (!powerupDesigner.IsThereDesign(tile, true))
        {
            currentMatchingData.Reset();

            if (IsMatchingPossible(tile, CheckingType.ToMatchDirectly))
            {
                DoAnimation(currentMatchingData.tile1.piece);
                DoAnimation(currentMatchingData.tile2.piece);
                DoAnimation(currentMatchingData.tile3.piece);
            }
        }
    }

    void DoAnimation(Piece piece)
    {
        piece.animation = SL.Get<AnimationManager>().StartScaleAnimation
        (
            piece.gameObject.transform,
            Mathematics.Vector3One() * TileStyle.pieceScaleCoefficient,
            Mathematics.Vector3One() * 0,
            0.2f,
            AnimationType.Scale,
            EaseTypes.OutSine,
            this
        );

        matchingPieces.Add(piece);

        piece.state = PieceState.Matching;
    }

    public void AnimationIsCompleted(AnimationSystem.Animation animation)
    {
        for (int i = 0; i < matchingPieces.Count; i++)
        {
            Piece matchedPiece = matchingPieces[i];

            if (matchedPiece.animation == animation)
            {
                animation.transform.gameObject.SetActive(false);

                matchedPiece.state = PieceState.Out;
                matchedPiece.myTile.SetPiece(null); //Out

                SL.Get<TaskManager>().currentTask.NormalPieceDecreasedAsThatCount(1);

                matchedPiece.animation = null;
                matchingPieces.RemoveAt(i);
                return;
            }
        }
    }

    public void AnimationIsUpdating(AnimationSystem.Animation animation) { }

    public void UpdateMe()
    {
        Match();
    }

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }

    public class CurrentMatchingData
    {
        public Tile tile1;
        public Tile tile2;
        public Tile tile3;

        public void Reset()
        {
            tile1 = tile2 = tile3 = null;
        }

        //Note: Below function is for debugging -So Temporary
        public string TempInfo()
        {
            string info = "";
            info += "Tile 1 is: " + ((tile1 == null) ? "null" : tile1.gameObject.name);
            info += "Tile 2 is: " + ((tile2 == null) ? "null" : tile1.gameObject.name);
            info += "Tile 2 is: " + ((tile3 == null) ? "null" : tile1.gameObject.name);

            return info;
        }
    }

    public enum CheckingType
    {
        ToMatchDirectly,
        JustChecking
    }
}