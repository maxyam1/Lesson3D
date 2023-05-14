using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventBus
{
    private static Dictionary<Type, SubscribersList<IGlobalSubscriber>> s_Subscribers
    = new Dictionary<Type, SubscribersList<IGlobalSubscriber>>();

    private static Dictionary<Type, List<Type>> s_CashedSubscriberTypes =
        new Dictionary<Type, List<Type>>();

    public static void Subscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = GetSubscribersTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (!s_Subscribers.ContainsKey(t))
                s_Subscribers[t] = new SubscribersList<IGlobalSubscriber>();
            s_Subscribers[t].Add(subscriber);
        }
    }



    public static List<Type> GetSubscribersTypes(
        IGlobalSubscriber globalSubscriber)
    {
        Type type = globalSubscriber.GetType();
        if (s_CashedSubscriberTypes.ContainsKey(type))
            return s_CashedSubscriberTypes[type];

        List<Type> subscriberTypes = type
            .GetInterfaces()
            .Where(it =>
                    //it  is IGlobalSubscriber &&
                    typeof(IGlobalSubscriber).IsInstanceOfType(globalSubscriber) &&
                    //it.Implements<IGlobalSubscriber>() &&
                    it != typeof(IGlobalSubscriber))
            .ToList();

        s_CashedSubscriberTypes[type] = subscriberTypes;
        return subscriberTypes;

    }



    public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : class, IGlobalSubscriber
    {
        SubscribersList<IGlobalSubscriber> subscribers = null;
        if (s_Subscribers.ContainsKey(typeof(TSubscriber)))
        {
            subscribers = s_Subscribers[typeof(TSubscriber)];
        }

        if (subscribers == null)
        {
            Debug.Log("No subscribers");
            return;
        }

        subscribers.Executing = true;
        foreach (IGlobalSubscriber subscriber in subscribers.List)
        {
            try
            {
                action.Invoke(subscriber as TSubscriber);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        subscribers.Executing = false;
        subscribers.Cleanup();
    }



    public static void Unsubscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = GetSubscribersTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (s_Subscribers.ContainsKey(t))
                s_Subscribers[t].Remove(subscriber);
        }
    }

    public static bool Implements<UInterface>(this Type t) =>
        t.IsAssignableFrom(typeof(UInterface));

    //public static bool Implements<UInterface>(this object Object) =>
    //    typeof(UInterface).IsInstanceOfType(Object);
}


public class SubscribersList<TSubscriber> where TSubscriber : class
{
    private bool m_NeedsCleanUp = false;

    public bool Executing;

    public readonly List<TSubscriber> List = new List<TSubscriber>();

    public void Add(TSubscriber subscriber)
    {
        List.Add(subscriber);
    }

    public void Remove(TSubscriber subscriber)
    {
        if (Executing)
        {
            var i = List.IndexOf(subscriber);
            if (i >= 0)
            {
                m_NeedsCleanUp = true;
                List[i] = null;
            }
        }
        else
        {
            List.Remove(subscriber);
        }
    }

    public void Cleanup()
    {
        if (!m_NeedsCleanUp)
        {
            return;
        }

        List.RemoveAll(s => s == null);
        m_NeedsCleanUp = false;
    }

}


public interface IGlobalSubscriber{}