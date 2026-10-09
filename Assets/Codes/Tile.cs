using UnityEngine;

public class Tile
{
    public int x;
    public int y;
    public Vector3 position { get; private set; }

    public GameObject gameObject { get; private set; }

    public Vector2 rightTop, leftTop, leftBottom, rightBottom;

    public Piece piece { get; private set; }
    public bool isUnusable { get; private set; }

    public Tile(int x, int y, bool isUnusable, Vector3 position)
    {
        this.x = x;
        this.y = y;
        this.position = position;
        this.isUnusable = isUnusable;

        GameObject sampleGameObject = Resources.Load<GameObject>("Grid/Tile");
        gameObject = Object.Instantiate(sampleGameObject, position, Quaternion.identity);
        gameObject.transform.localScale *= TileStyle.scaleCoefficient;

        gameObject.name = "Tile (" + x + ", " + y + ")";
        SetBounds();

        if(isUnusable)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;    
        }
    }

    void SetBounds()
    {
        float scaleCoeffiecient = TileStyle.scaleCoefficient;
        float reversedPPU = 1f / TileStyle.ppu;
        Vector2 size = TileStyle.size;
        //Right
        rightTop.x = position.x + (size.x * 0.5f) * reversedPPU * scaleCoeffiecient;
        leftTop.x = position.x - (size.x * 0.5f) * reversedPPU * scaleCoeffiecient;

        rightTop.y = position.y + (size.y * 0.5f) * reversedPPU * scaleCoeffiecient;
        leftTop.y = position.y + (size.y * 0.5f) * reversedPPU * scaleCoeffiecient;

        leftBottom.x = leftTop.x;
        rightBottom.x = rightTop.x;

        leftBottom.y = position.y - (size.y * 0.5f) * reversedPPU * scaleCoeffiecient;
        rightBottom.y = position.y - (size.y * 0.5f) * reversedPPU * scaleCoeffiecient;
    }

    public void SetPiece(Piece piece)
    {
        if(isUnusable)
            return;

        if (piece == null)
        {
            this.piece = null;
            return;
        }

        if (piece.myTile != null && piece.myTile.piece == piece)
            piece.myTile.SetPiece(null);

        piece.myTile = this;
        this.piece = piece;
    }

    public bool IsClicked(Vector3 mousePosition)
    {
        float posX = mousePosition.x;
        float posY = mousePosition.y;

        bool isClicked = posX >= leftTop.x && posX <= rightTop.x && posY >= leftBottom.y && posY <= leftTop.y;

        return isClicked;
    }
}