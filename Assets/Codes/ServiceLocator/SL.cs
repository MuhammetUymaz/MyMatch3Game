using System;
using System.Collections.Generic;

public static class SL
{
    /*
        Service Locator design pattern. 
        So regitered objects are reached by other classes with this class
    */
    
    static List<object> services;

    static SL()
    {
        services = new List<object>();
    }

    public static void Register(object newService)
    {
        if(services.Contains(newService))
            return;
        
        services.Add(newService);
    }

    public static T Get<T>() where T: class
    {
        Type type = typeof(T);

        for(int i = 0; i < services.Count; i++)
        {
            if(services[i].GetType() == type)
                return (T)services[i];
        }

        return null;
    }

    public static void Remove<T>() where T: class
    {
        Type type = typeof(T);

        for(int i = 0; i < services.Count; i++)
        {
            if(services[i].GetType() == type)
            {
                services.RemoveAt(i);
                return;
            }
        }
    }
}