using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private const int MAX_EVENT_PROCESS_COUNT = 10;

    public class Callbacks
    {
        // when combat event is completed
        public Action<CombatEvent> OnCombatEvent;
    }
    
    
    public static CombatSystem Instance;
    
    private Dictionary<Collider, IDamageAble> monsterDic = new Dictionary<Collider, IDamageAble>();
    private Queue<CombatEvent> combatEventQueue = new Queue<CombatEvent>();
    
    public readonly Callbacks Events = new Callbacks();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        int processCount = 0;
        while (combatEventQueue.Count > 0 && processCount < MAX_EVENT_PROCESS_COUNT)
        {
            CombatEvent combatEvent = combatEventQueue.Dequeue();
            combatEvent.Receiver.TakeDamage(combatEvent);
            Events.OnCombatEvent?.Invoke(combatEvent);
            processCount++;
        }
    }

    // public void RegisterMonster(IDamageAble monster)
    // {
    //     if (monsterDic.TryAdd(monster.MainCollider, monster) == true) return;
    //     Debug.LogWarning($"{monster.GameObject.name} is already registered" +
    //                      $"{monsterDic[monster.MainCollider]} will be overwritten");
    //     monsterDic[monster.MainCollider] = monster;
    // }

    public IDamageAble GetMonsterOrNull(Collider collider)
    {
        if (monsterDic.ContainsKey(collider) == true)
        {
            return monsterDic[collider];
        }
        return null;
    }

    public void AddCombatEvent(CombatEvent combatEvent)
    {
        combatEventQueue.Enqueue(combatEvent);
    }
    
    public void RegisterMonster(Collider collider, IDamageAble monster)
    {
        if (monsterDic.TryAdd(collider, monster) == true) return;
        Debug.LogWarning($"{monster.GameObject.name} is already registered" +
                         $"{monsterDic[collider]} will be overwritten");
        monsterDic[collider] = monster;
    }
}
