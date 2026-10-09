using UnityEngine;

public class Timer
{
    public bool isUsed { get; private set; }
    public float limitedTime { get; private set; }
    public float time { get; private set; }
    public ITimerEvents events { get; private set; }

    public Timer(float limitedTime, ITimerEvents events)
    {
        Reset(limitedTime, events);
    }

    public void Reset(float limitedTime, ITimerEvents events)
    {
        this.limitedTime = limitedTime;
        time = 0;
        
        this.events = events;
        this.isUsed = true;
    }

    public void OperateTime()
    {
        time += Time.deltaTime;

        events.TimerIsUpdating(this);

        if (time >= limitedTime)
        {
            isUsed = false;
            events.TimerIsDone(this);
        }
    }
}