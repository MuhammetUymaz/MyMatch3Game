using AnimationSystem;
using UnityEngine;

public class SLInstantiator: MonoBehaviour
{
    void Start()
    {
        //Instantiate some objects which are instantiated directly
        SL.Register(new InfoManager());
        SL.Register(new GameManager());
        SL.Register(new MouseSystem());
        SL.Register(new AnimationManager());
        SL.Register(new Swapping());
        SL.Register(new HintSystem());
        SL.Register(new PowerupDesigner());
        SL.Register(new Matching());
        SL.Register(new Falling());
        SL.Register(new Refilling());
        SL.Register(new PiecePool());
        SL.Register(new StepManager());
        SL.Register(new TimerManager());
        SL.Register(new TaskManager());
        SL.Register(new TutorialManager());
        SL.Register(new TriggerByNeighbour());
    }
}