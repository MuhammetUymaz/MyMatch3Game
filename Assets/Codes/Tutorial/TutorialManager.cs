using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager: ITimerEvents
{
    Timer tempTimer;
    
    Transform tutorialParent;
    Image tutorialOverlayImage;
    TextMeshProUGUI tutorialText;

    public TutorialManager()
    {
        tempTimer = SL.Get<TimerManager>().RunTimer(0.5f, this);

        tutorialParent = GameObject.Find("CanvasOverlay").transform.GetChild(0);
        tutorialOverlayImage = tutorialParent.GetChild(0).GetComponent<Image>();
        tutorialText = tutorialParent.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        tutorialParent.gameObject.SetActive(true);
        //tutorialText.text = "Temporary!";
        //tutorialOverlayImage.enabled = true;

        Material material = tutorialOverlayImage.material;

        //Tile tile = SL.Get<GridManager>()[0, 2];
        Tile tile = SL.Get<GridManager>()[3, 3];

        material.SetVector("_RightTop", GetUV(Camera.main.WorldToScreenPoint(tile.rightTop)));
        material.SetVector("_LeftTop", GetUV(Camera.main.WorldToScreenPoint(tile.leftTop)));
        material.SetVector("_LeftBottom", GetUV(Camera.main.WorldToScreenPoint(tile.leftBottom)));
        material.SetVector("_RightBottom", GetUV(Camera.main.WorldToScreenPoint(tile.rightBottom)));
    }
    
    Vector2 GetUV(Vector2 position)
    {
        float width = Screen.width;
        float height = Screen.height;

        return new Vector2(position.x / width, position.y / height);
    }

    //Note: Below code area is temporary
    /////
    public void TimerIsUpdating(Timer timer)
    {

    }

    public void TimerIsDone(Timer timer)
    {
        return;

        Material material = tutorialOverlayImage.material;

        //Tile tile = SL.Get<GridManager>()[0, 2];
        GridManager gridManager = SL.Get<GridManager>();
        Tile tile = gridManager[Random.Range(0, gridManager.column), Random.Range(0, gridManager.column)];

        material.SetVector("_RightTop", GetUV(Camera.main.WorldToScreenPoint(tile.rightTop)));
        material.SetVector("_LeftTop", GetUV(Camera.main.WorldToScreenPoint(tile.leftTop)));
        material.SetVector("_LeftBottom", GetUV(Camera.main.WorldToScreenPoint(tile.leftBottom)));
        material.SetVector("_RightBottom", GetUV(Camera.main.WorldToScreenPoint(tile.rightBottom)));

        tempTimer.Reset(0.5f, this);
    }
    /////
}