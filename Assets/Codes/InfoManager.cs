using UnityEngine;
using TMPro;

public class InfoManager
{
    TextMeshProUGUI infoText;
    
    public InfoManager()
    {
        infoText = GameObject.Find("InfoLabel").GetComponent<TextMeshProUGUI>();

        SL.Register(this);
    }

    public void ResetInfo()
    {
        infoText.text = "Reach goal match count";
    }

    public void SetInfo(string message)
    {
        infoText.text = message;
    }
}