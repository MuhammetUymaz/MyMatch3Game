using System.Collections.Generic;
using AnimationSystem;
using UnityEngine;

public class PowerupDesigner : IAnimationEvents
{
    GridManager gridManager;

    bool hasGridInfoCached;

    bool shouldOperate;

    List<DesignChunk> chunks;

    public PowerupDesigner()
    {
        chunks = new List<DesignChunk>();
    }

    public bool IsThereDesign(Tile tile, bool shouldOperate)
    {
        if (!hasGridInfoCached)
        {
            gridManager = SL.Get<GridManager>();
            hasGridInfoCached = true;
        }

        this.shouldOperate = shouldOperate;

        if (tile.piece == null)
            return false;

        if (tile.piece.GetType() != typeof(NormalPiece))
            return false;

        return IsThereLargeI1(tile) || IsThereLargeI2(tile) || //"Large I"
                IsThereI1(tile) || IsThereI2(tile) || IsThereI3(tile) || IsThereI4(tile) || //"I"
                IsThereT1(tile) || IsThereT2(tile) || IsThereT3(tile) || IsThereT4(tile) || //"T"
                IsThereL1(tile) || IsThereL2(tile) || IsThereL3(tile) || IsThereL4(tile) || //"L"
                IsThereO1(tile) || IsThereO2(tile) || IsThereO3(tile) || IsThereO4(tile); //"O"

        //return false;
    }

    //"T"
    bool IsThereT1(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x + 1, y);
        Tile tile3 = gridManager.GetTile(x, y - 1);
        Tile tile4 = gridManager.GetTile(x, y - 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    bool IsThereT2(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y + 1);
        Tile tile2 = gridManager.GetTile(x, y - 1);
        Tile tile3 = gridManager.GetTile(x + 1, y);
        Tile tile4 = gridManager.GetTile(x + 2, y);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    bool IsThereT3(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y + 1);
        Tile tile2 = gridManager.GetTile(x, y - 1);
        Tile tile3 = gridManager.GetTile(x - 1, y);
        Tile tile4 = gridManager.GetTile(x - 2, y);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    bool IsThereT4(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x + 1, y);
        Tile tile3 = gridManager.GetTile(x, y + 1);
        Tile tile4 = gridManager.GetTile(x, y + 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    //"L"
    bool IsThereL1(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y + 1);
        Tile tile2 = gridManager.GetTile(x, y + 2);
        Tile tile3 = gridManager.GetTile(x + 1, y);
        Tile tile4 = gridManager.GetTile(x + 2, y);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    bool IsThereL2(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x - 2, y);
        Tile tile3 = gridManager.GetTile(x, y - 1);
        Tile tile4 = gridManager.GetTile(x, y - 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    bool IsThereL3(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x + 1, y);
        Tile tile2 = gridManager.GetTile(x + 2, y);
        Tile tile3 = gridManager.GetTile(x, y - 1);
        Tile tile4 = gridManager.GetTile(x, y - 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    bool IsThereL4(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x - 2, y);
        Tile tile3 = gridManager.GetTile(x, y + 1);
        Tile tile4 = gridManager.GetTile(x, y + 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LT);
            }

            return true;
        }

        return false;
    }

    //"Large I"
    bool IsThereLargeI1(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x - 2, y);
        Tile tile3 = gridManager.GetTile(x + 1, y);
        Tile tile4 = gridManager.GetTile(x + 2, y);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LargeI);
            }

            return true;
        }

        return false;
    }

    bool IsThereLargeI2(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y - 1);
        Tile tile2 = gridManager.GetTile(x, y - 2);
        Tile tile3 = gridManager.GetTile(x, y + 1);
        Tile tile4 = gridManager.GetTile(x, y + 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3, tile4);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, tile4, PowerupType.LargeI);
            }

            return true;
        }

        return false;
    }

    //"I"
    bool IsThereI1(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x - 2, y);
        Tile tile3 = gridManager.GetTile(x + 1, y);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.I);
            }

            return true;
        }

        return false;
    }

    bool IsThereI2(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x + 1, y);
        Tile tile3 = gridManager.GetTile(x + 2, y);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.I);
            }

            return true;
        }

        return false;
    }

    bool IsThereI3(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y + 1);
        Tile tile2 = gridManager.GetTile(x, y + 2);
        Tile tile3 = gridManager.GetTile(x, y - 1);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.I);
            }

            return true;
        }

        return false;
    }

    bool IsThereI4(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y + 1);
        Tile tile2 = gridManager.GetTile(x, y - 1);
        Tile tile3 = gridManager.GetTile(x, y - 2);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.I);
            }

            return true;
        }

        return false;
    }

    //"O"
    bool IsThereO1(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x - 1, y + 1);
        Tile tile3 = gridManager.GetTile(x, y + 1);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.O);
            }

            return true;
        }

        return false;
    }

    bool IsThereO2(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x + 1, y);
        Tile tile2 = gridManager.GetTile(x + 1, y + 1);
        Tile tile3 = gridManager.GetTile(x, y + 1);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.O);
            }

            return true;
        }

        return false;
    }

    bool IsThereO3(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x, y - 1);
        Tile tile2 = gridManager.GetTile(x + 1, y);
        Tile tile3 = gridManager.GetTile(x + 1, y - 1);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.O);
            }

            return true;
        }

        return false;
    }

    bool IsThereO4(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        Tile tile1 = gridManager.GetTile(x - 1, y);
        Tile tile2 = gridManager.GetTile(x, y - 1);
        Tile tile3 = gridManager.GetTile(x - 1, y - 1);

        bool areTilesAppropriate = AreTilesAppropriate(tile, tile1, tile2, tile3);

        if (areTilesAppropriate)
        {
            //Temporary
            if (shouldOperate)
            {
                StartCombining(tile, tile1, tile2, tile3, null, PowerupType.O);
            }

            return true;
        }

        return false;
    }

    //When other tiles are 4
    bool AreTilesAppropriate(Tile baseTile, Tile tile1, Tile tile2, Tile tile3, Tile tile4)
    {
        if (tile1 == null || tile2 == null || tile3 == null || tile4 == null)
            return false;

        NormalPiece basePiece = baseTile.piece == null ? null : baseTile.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)baseTile.piece,
                    piece1 = tile1.piece == null ? null : tile1.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile1.piece,
                    piece2 = tile2.piece == null ? null : tile2.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile2.piece,
                    piece3 = tile3.piece == null ? null : tile3.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile3.piece,
                    piece4 = tile4.piece == null ? null : tile4.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile4.piece;

        /*
        NormalPiece basePiece   = baseTile.piece == null ? null : (NormalPiece) baseTile.piece,
                    piece1      = tile1.piece == null ? null : (NormalPiece) tile1.piece, 
                    piece2      = tile2.piece == null ? null : (NormalPiece) tile2.piece, 
                    piece3      = tile3.piece == null ? null : (NormalPiece) tile3.piece, 
                    piece4      = tile4.piece == null ? null : (NormalPiece) tile4.piece;
        */

        if (piece1 == null || piece2 == null || piece3 == null || piece4 == null)
            return false;

        if (basePiece.state != PieceState.Idle || piece1.state != PieceState.Idle || piece2.state != PieceState.Idle || piece3.state != PieceState.Idle || piece4.state != PieceState.Idle)
            return false;

        //Note: Add that mechanic -QUEUE for Matching (Request)

        bool areTheySame = basePiece.id == piece1.id && piece1.id == piece2.id && piece3.id == piece4.id && piece1.id == piece3.id;

        if (!areTheySame)
            return false;

        return true;
    }

    //When other tiles are 3
    bool AreTilesAppropriate(Tile baseTile, Tile tile1, Tile tile2, Tile tile3)
    {
        if (tile1 == null || tile2 == null || tile3 == null)
            return false;

        NormalPiece basePiece = baseTile.piece == null ? null : baseTile.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)baseTile.piece,
                    piece1 = tile1.piece == null ? null : tile1.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile1.piece,
                    piece2 = tile2.piece == null ? null : tile2.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile2.piece,
                    piece3 = tile3.piece == null ? null : tile3.piece.GetType() != typeof(NormalPiece) ? null : (NormalPiece)tile3.piece;

        // NormalPiece basePiece   = baseTile.piece == null ? null : (NormalPiece) baseTile.piece,
        //             piece1      = tile1.piece == null ? null : (NormalPiece) tile1.piece, 
        //             piece2      = tile2.piece == null ? null : (NormalPiece) tile2.piece, 
        //             piece3      = tile3.piece == null ? null : (NormalPiece) tile3.piece;

        if (piece1 == null || piece2 == null || piece3 == null)
            return false;

        if (basePiece.state != PieceState.Idle || piece1.state != PieceState.Idle || piece2.state != PieceState.Idle || piece3.state != PieceState.Idle)
            return false;

        //Note: Add that mechanic -QUEUE for Matching (Request)

        bool areTheySame = basePiece.id == piece1.id && piece1.id == piece2.id && piece1.id == piece3.id;

        if (!areTheySame)
            return false;

        return true;
    }

    //Animation
    void StartCombining(Tile baseTile, Tile tile1, Tile tile2, Tile tile3, Tile tile4, PowerupType powerupType)
    {
        chunks.Add(new DesignChunk(baseTile, tile1, tile2, tile3, tile4, powerupType));

        Vector3 destinationPosition = baseTile.position;

        DoAnimation(tile1.piece, destinationPosition);
        DoAnimation(tile2.piece, destinationPosition);
        DoAnimation(tile3.piece, destinationPosition);

        if (tile4 != null)
            DoAnimation(tile4.piece, destinationPosition);

        //Vector3 destinationPosition
        //DoAnimation(tile.piece, )
    }

    void DoAnimation(Piece piece, Vector3 destinationPosition)
    {
        piece.animation = SL.Get<AnimationManager>().StartPositionAnimation
        (
            piece.gameObject.transform,
            piece.myTile.position,
            destinationPosition,
            0.2f, //0.3f, //0.1f,
            AnimationType.Position,
            EaseTypes.OutSine,
            this
        );

        piece.state = PieceState.PowerupDesigning;
    }

    public void AnimationIsCompleted(AnimationSystem.Animation animation)
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            DesignChunk chunk = chunks[i];

            Tile tileOnAnimation = chunk.GetTileOnTheAnimation(animation);
            if (tileOnAnimation == null)
                continue;

            chunk.TileIsArrived();

            if (chunk.otherTilesCount == 0)
            {
                chunk.baseTile.piece.RemovePiece();
                chunk.tile1.piece.RemovePiece();
                chunk.tile2.piece.RemovePiece();
                chunk.tile3.piece.RemovePiece();

                if (chunk.tile4 != null)
                {
                    chunk.tile4.piece.RemovePiece();
                    SL.Get<TaskManager>().currentTask.NormalPieceDecreasedAsThatCount(5);
                }
                else
                {
                    SL.Get<TaskManager>().currentTask.NormalPieceDecreasedAsThatCount(4);     
                }
                    
                chunks.RemoveAt(i);

                Tile baseTile = chunk.baseTile;
                Piece piece = null;

                if (chunk.powerupType == PowerupType.LT)
                    piece = SL.Get<PiecePool>().GetPieceRandomPiece<PowerupLT>(baseTile.position);
                else if (chunk.powerupType == PowerupType.O)
                    piece = SL.Get<PiecePool>().GetPieceRandomPiece<PowerupO>(baseTile.position);
                else if (chunk.powerupType == PowerupType.LargeI)
                    piece = SL.Get<PiecePool>().GetPieceRandomPiece<PowerupLargeI>(baseTile.position);
                else if (chunk.powerupType == PowerupType.I)
                    piece = SL.Get<PiecePool>().GetPieceRandomPiece<PowerupI>(baseTile.position);

                baseTile.SetPiece(piece);
            }

            return;
        }
    }

    public void AnimationIsUpdating(AnimationSystem.Animation animation) { }

    class DesignChunk
    {
        public Tile baseTile { get; private set; }
        public Tile tile1 { get; private set; }
        public Tile tile2 { get; private set; }
        public Tile tile3 { get; private set; }
        public Tile tile4 { get; private set; }
        public PowerupType powerupType { get; private set; }

        public int otherTilesCount { get; private set; }

        public DesignChunk(Tile baseTile, Tile tile1, Tile tile2, Tile tile3, Tile tile4, PowerupType powerupType)
        {
            this.baseTile = baseTile;
            this.tile1 = tile1;
            this.tile2 = tile2;
            this.tile3 = tile3;
            this.tile4 = tile4;
            this.powerupType = powerupType;

            otherTilesCount = tile4 == null ? 3 : 4;
        }

        public Tile GetTileOnTheAnimation(AnimationSystem.Animation animation)
        {
            if (baseTile.piece.animation == animation)
                return baseTile;
            else if (tile1.piece.animation == animation)
                return tile1;
            else if (tile2.piece.animation == animation)
                return tile2;
            else if (tile3.piece.animation == animation)
                return tile3;
            else if (tile4 != null && tile4.piece.animation == animation)
                return tile4;

            return null;
        }

        public void TileIsArrived()
        {
            otherTilesCount--;
        }
    }
}