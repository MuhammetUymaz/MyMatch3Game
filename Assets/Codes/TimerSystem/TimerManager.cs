using System.Collections.Generic;

public class TimerManager: IUpdate
{
    List<Timer> timerList;

    public TimerManager()
    {
        timerList = new List<Timer>();

        SL.Get<UpdateManager>().observers.Add(this);
    }

    public Timer RunTimer(float limitedTime, ITimerEvents events)
    {
        Timer timer = null;

        for(int i = 0; i < timerList.Count; i++)
        {
            if(!timerList[i].isUsed)
            {
                timer = timerList[i];
                timer.Reset(limitedTime, events);
                return timer;
            }
        }

        //Need a new one

        timer = new Timer(limitedTime, events);

        timerList.Add(timer);

        return timer;
    }

    public void UpdateMe()
    {
        for(int i = 0; i < timerList.Count; i++)
        {
            if(timerList[i].isUsed)
                timerList[i].OperateTime();
        }
    }

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }
}