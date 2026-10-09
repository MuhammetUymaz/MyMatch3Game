using UnityEngine;
using TMPro;

public class TaskMatchingGoalBased : TaskBase, ITimerEvents
{
    public Timer timer { get; private set; }
    public int matchingGoal { get; private set; }

    //UI
    TextMeshProUGUI goalCountText;
    TextMeshProUGUI timeText;

    public TaskMatchingGoalBased(float limitedTime, int matchingGoal)
    {
        situation = Situation.Continuing;

        this.matchingGoal = matchingGoal;

        timer = SL.Get<TimerManager>().RunTimer(limitedTime, this);

        GameObject textsParent = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        goalCountText = textsParent.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        timeText = textsParent.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        textsParent.SetActive(true);

        RefreshGoal();
    }

    public void TimerIsUpdating(Timer timer)
    {        
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        if (situation != Situation.Continuing)
            return;

        timeText.text = ((int)(timer.limitedTime - timer.time)).ToString();
    }

    public void TimerIsDone(Timer timer)
    {
        if(SL.Get<GameManager>().state != GameState.Playing)
            return;

        if (situation != Situation.Continuing)
            return;

        if(matchingGoal > 0)
        {
            //Game has been losed!
            SL.Get<InfoManager>().SetInfo("Game Over!");
            SL.Get<GameManager>().state = GameState.GameOver;
        }
    }

    public override void OperateTask()
    {
        //This function is not used to decrease the goal
    }

    void RefreshGoal()
    {
        goalCountText.text = matchingGoal.ToString();
    }

    public override void NormalPieceDecreasedAsThatCount(int count)
    {
        matchingGoal--;

        if(matchingGoal < 0)
            matchingGoal = 0;

        RefreshGoal();

        if(matchingGoal <= 0)
        {
            //Game has ben won!
            SL.Get<InfoManager>().SetInfo("You Win!");
            SL.Get<GameManager>().state = GameState.GameOver;
        }
    }
}