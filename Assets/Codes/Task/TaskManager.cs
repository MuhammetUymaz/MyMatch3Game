using UnityEngine;

public class TaskManager
{
    public TaskBase currentTask { get; private set; }

    public TaskManager()
    {

    }

    public void StartTask()
    {
        GameMode mode = SL.Get<GameManager>().mode;

        if (mode == GameMode.StepBased)
        {
            currentTask = new TaskStepBased();
        }
        else if (mode == GameMode.TimeBased)
        {
            currentTask = new TaskTimeBased();
        }
        else if (mode == GameMode.MatchingGoalBased)
        {
            currentTask = new TaskMatchingGoalBased(20, 20);
        }
    }
}