using UnityEngine;

public static class TileStyle
{
    public static Vector3 origin { get; private set; }
    public static Vector2 size { get; private set; }
    public static float gap { get; private set; }
    public static float scaleCoefficient { get; private set; }
    public static float ppu { get; private set; }

    public static float pieceScaleCoefficient { get; private set; }

    static TileStyle()
    {
        size = new Vector2(300, 303);
        scaleCoefficient = 0.2f;

        pieceScaleCoefficient = 0.15f;

        origin = new Vector3(-1.8f, 5, 0);

        gap = 0.05f;

        ppu = 100f;
    }
}