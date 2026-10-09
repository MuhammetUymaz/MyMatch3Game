using AnimationSystem;
using UnityEngine;

public class GridManager : MonoBehaviour, IAnimationEvents, IUpdate
{
    public int column;
    public int row;

    Tile[,] grid;

    GameObject spriteMaskGameObject;

    public Tile this[int i, int j]
    {
        get
        {
            return grid[i, j];
        }
    }

    void Start()
    {
        SL.Register(this);

        GenerateBoard();
        FillGrid();

        spriteMaskGameObject = GameObject.Find("SpriteMask");

        Vector3 spriteMaskPosition = spriteMaskGameObject.transform.position;
        spriteMaskPosition.y = grid[0,0].position.y + 0.75f;
        spriteMaskGameObject.transform.position = spriteMaskPosition;

        SL.Get<TaskManager>().StartTask();
        SL.Get<UpdateManager>().observers.Add(this);

        SL.Get<HintSystem>().Start();
    }

    void GenerateBoard()
    {
        grid = new Tile[column, row];

        Vector2 tileSize = TileStyle.size;

        float gap = TileStyle.gap;
        float scaleCoefficient = TileStyle.scaleCoefficient;
        float reversedPPU = 1f / TileStyle.ppu;

        Vector2 tileGameSize = new Vector2();
        tileGameSize.x = tileSize.x * reversedPPU * scaleCoefficient;
        tileGameSize.y = tileSize.y * reversedPPU * scaleCoefficient;

        float totalWidth = column * tileGameSize.x + (column-1) * gap;
        float totalHeight = row * tileGameSize.y + (row-1) * gap;

        Vector3 origin = new Vector3(totalWidth * -0.5f + tileGameSize.x * 0.5f, totalHeight * 0.5f + tileGameSize.y * 0.5f, 0);
        

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {

                Vector3 tilePosition = new Vector3();
                tilePosition.x = i * ((tileSize.x * reversedPPU * scaleCoefficient) + gap);
                tilePosition.y = -j * ((tileSize.y * reversedPPU * scaleCoefficient) + gap);
                tilePosition.z = 0;
                Vector3 position = origin + tilePosition;
                bool isUnusable = false;

                Tile tile = new Tile(i, j, isUnusable, position);

                grid[i, j] = tile;
            }
        }
    }

    void FillGrid()
    {
        PiecePool piecePool = SL.Get<PiecePool>();
        PowerupDesigner powerupDesigner = SL.Get<PowerupDesigner>();
        Matching matching = SL.Get<Matching>();

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Tile tile = grid[i, j];
                Vector3 tilePosition = tile.position;

                if (!tile.isUnusable)
                {
                    //Specific piece
                    if (i == 2 && j == 2)
                    {
                        Obstacle.obstacleIDOfLastGeneratingOne = 0;
                        Piece piece = piecePool.GetPieceRandomPiece<Obstacle>(tilePosition);
                        tile.SetPiece(piece);
                    }
                    //Normal Piece
                    else
                    {
                        Piece piece;
                        bool isMatchingPossible;
                        do
                        {
                            piece = piecePool.GetPieceRandomPiece<NormalPiece>(tilePosition);
                            tile.SetPiece(piece);

                            isMatchingPossible = powerupDesigner.IsThereDesign(tile, false) ||
                                                    matching.IsMatchingPossible(tile, Matching.CheckingType.JustChecking);

                            if (isMatchingPossible)
                                piecePool.ThrowPieceDirectly(piece);
                        }
                        while (isMatchingPossible);
                    }
                }
            }
        }
    }

    public void ClickingTiles(Vector3 mousePosition)
    {
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Tile tile = grid[i, j];
                tile.IsClicked(mousePosition);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || x >= column || y < 0 || y >= row)
            return null;

        return grid[x, y];
    }

    public void AnimationIsCompleted(AnimationSystem.Animation animation)   {   }

    public void AnimationIsUpdating(AnimationSystem.Animation animation) { }

    //Updating
    public void UpdateMe()
    {
        //Holding Mouse
        if (Input.GetMouseButtonDown(0))
        {
            GridManager gridManager = FindFirstObjectByType<GridManager>();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            gridManager.ClickingTiles(mousePosition);
        }
    }

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }
}