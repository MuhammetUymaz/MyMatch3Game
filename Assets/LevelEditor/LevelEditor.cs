#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    static LevelEditor window;

    int column;
    int row;

    bool isGenerated;

    List<BoxInfo> boxInfoList = new List<BoxInfo>();

    BoxInfo clickedBox;


    [MenuItem("Windows/LevelEditor")]
    public static void OpenWindow()
    {
        window = GetWindow<LevelEditor>();
    }

    void OnEnable()
    {

    }

    void OnGUI()
    {
        ShowProperties();

        bool isGeneratingRequest = GUILayout.Button("Generate");
        if (isGeneratingRequest)
        {
            //Clear before previous ones
            boxInfoList.Clear();
            clickedBox = null;

            isGenerated = true;

            Vector2 offset = new Vector2(50, 100);

            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < column; i++)
                {
                    boxInfoList.Add(new BoxInfo(i, j, new Vector2(offset.x + i * (BoxInfo.WIDTH + BoxInfo.GAP), offset.y + j * (BoxInfo.HEIGHT + BoxInfo.GAP))));
                }
            }
        }

        Event currentEvent = Event.current;

        if (isGenerated)
        {
            DealWithButtons();
        }

    }

    void ShowProperties()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Column");
            column = EditorGUILayout.IntField(column);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Row");
            row = EditorGUILayout.IntField(row);
        }
        GUILayout.EndHorizontal();
    }

    void DealWithButtons()
    {
        //Draw
        for (int i = 0; i < boxInfoList.Count; i++)
        {
            BoxInfo boxInfo = boxInfoList[i];

            //Draw
            GUI.skin.box.normal.background = LevelEditorDesign.normalBox;
            GUI.Box(new Rect(boxInfo.position.x, boxInfo.position.y, BoxInfo.WIDTH, BoxInfo.HEIGHT), "");
        }

        //Clicking
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown)
        {
            for (int i = 0; i < boxInfoList.Count; i++)
            {
                BoxInfo boxInfo = boxInfoList[i];

                if (boxInfo.IsClicked(currentEvent.mousePosition))
                {
                    clickedBox = boxInfo;
                }
            }
        }
    }
    
    class BoxInfo
    {
        public Vector2 position;
        public const float WIDTH = 30, HEIGHT = 30, GAP = 5;
        public bool isSelected { get; private set; }
        public int x;
        public int y;

        public BoxInfo(int x, int y, Vector2 position)
        {
            this.x = x;
            this.y = y;

            this.position = position;
        }

        public Texture2D GetTexture()
        {
            if (isSelected)
            {
                return LevelEditorDesign.clickedBox;
            }
            else
            {
                return LevelEditorDesign.normalBox;
            }
        }

        public bool IsClicked(Vector2 mousePosition)
        {
            Rect rect = new Rect(position.x, position.y, WIDTH, HEIGHT);

            bool returning = rect.Contains(mousePosition);

            isSelected = returning;

            return returning;
        }
    }
}
#endif