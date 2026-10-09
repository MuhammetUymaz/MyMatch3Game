public class StepManager
{
    public int step { get; private set; }
    
    public void ReduceStep(int amount)
    {
        step -= amount;
    }

    public void IncreaseStep(int amount)
    {
        step += amount;
    }
}