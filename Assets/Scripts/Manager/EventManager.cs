using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = GetComponent<EventManager>();
    }

    public enum EventFlags
    {
        DefaultAttack,
        LaserAttack,
        MissileExplosion,
        LaserHit,
        RaidStart,
        RaidEnd,
        GameOver,
        RaidFailed,
    }

    private Dictionary<EventFlags, Action> eventDictionary = new Dictionary<EventFlags, Action>();

    public void AddListener(EventFlags flag, Action listener)
    {
        Action thisEvent;
        if (eventDictionary.TryGetValue(flag, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[flag] = thisEvent;
        }
        else
        {
            eventDictionary.Add(flag, listener);
        }
    }

    public void StopListening(EventFlags flag, Action listener)
    {
        Action thisEvent;
        if (eventDictionary.TryGetValue(flag, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[flag] = thisEvent;
        }
        else
        {
            eventDictionary.Remove(flag);
        }
    }

    public void TriggerEvent(EventFlags flag)
    {
        if (eventDictionary[flag] == null)
        {
            Debug.Log(flag + "not Existing");
            return;
        }
        eventDictionary[flag]?.Invoke();
    }
}
