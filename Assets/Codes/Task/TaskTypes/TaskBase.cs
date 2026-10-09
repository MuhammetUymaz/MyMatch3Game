public abstract class TaskBase
{
    protected Situation situation;

    public abstract void OperateTask();
    public abstract void NormalPieceDecreasedAsThatCount(int count);

    protected enum Situation
    {
        Continuing,
        GameLosed,
        GameWon,
    }
}