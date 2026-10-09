using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    /*
        This is the main class to update the classes which should be updated.
        The classes which should be updated are updated in 1 cycle respecitely.
    */

    public List<IUpdate> observers;

    void Start()
    {
        observers = new List<IUpdate>();

        SL.Register(this);
    }
    
    void Update()
    {
        for(int i = 0; i < observers.Count; i++)
            observers[i].UpdateMe();
    }
}
