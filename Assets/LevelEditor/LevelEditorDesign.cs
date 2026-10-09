#if UNITY_EDITOR
using UnityEngine;

public static class LevelEditorDesign
{
    public static Texture2D normalBox;
    public static Texture2D clickedBox;

    static LevelEditorDesign()
    {
        normalBox = new Texture2D(100, 100);
        clickedBox = new Texture2D(100, 100);

        AssignColor(ref normalBox, 100, 100, Color.red);
        AssignColor(ref clickedBox, 100, 100, Color.orange);
    }

    static void AssignColor(ref Texture2D texture, int width, int height, Color color)
    {
        for(int i = 0; i < width; i++)
            for(int j = 0; j < height; j++)
                texture.SetPixel(i, j, color);

        texture.Apply();
    }
}
#endif