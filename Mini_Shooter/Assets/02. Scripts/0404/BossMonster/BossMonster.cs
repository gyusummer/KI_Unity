using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public partial class BossMonster : MonoBehaviour, IDamageAble
{
    public event Action OnHpChanged;
    public Collider MainCollider => spineCollider;
    public GameObject GameObject => gameObject;
    public Stat Stat => bossStat;

    public Collider spineCollider;
    public Collider headCollider;
    public Collider[] leftArmCollider;
    public Collider[] rightArmCollider;
    public class Events
    {
        /// <summary>
        /// int currentHp, int maxHp
        /// </summary>
        public Action<int,int> OnDamage;
        public Action<BossState.StateName> OnChangedState;
    }
    
    public Events Event = new Events();
    private Stat bossStat = new Stat(){MaxHp =  1000, CurrentHp = -1};
    
    public Animator Animator { get; private set; }
    private Dictionary<BossState.StateName, BossState> stateDic 
        = new Dictionary<BossState.StateName, BossState>();
    
    private BossState previousState;
    private BossState currentState;
    
    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();

        bossStat.CurrentHp = bossStat.MaxHp;
        CrrentSceneBossMonster = this;
        s_bossMonsterColliders = GetComponentsInChildren<Collider>();

        BossState[] myState =  gameObject.GetComponentsInChildren<BossState>(true);
        for (int i = 0; i < myState.Length; i++)
        {
            var state = myState[i];
            state.Initialize(this);
            stateDic.Add(state.Name, state);
        }
        ChangeState(BossState.StateName.IdleState);
    }
    private void Start()
    {
        CombatSystem.Instance.RegisterMonster(spineCollider,this);
        CombatSystem.Instance.RegisterMonster(headCollider,this);
        foreach (Collider collider in leftArmCollider)
        {
            CombatSystem.Instance.RegisterMonster(collider,this);
        }
        foreach (Collider collider in rightArmCollider)
        {
            CombatSystem.Instance.RegisterMonster(collider,this);
        }
    }
    
    public void ChangeState(BossState.StateName state)
    {
        previousState = currentState;
        
        if (previousState != null)
        {
            previousState.Exit();
            previousState.gameObject.SetActive(false);
        }
        
        currentState = stateDic[state];
        currentState.Enter();
        currentState.gameObject.SetActive(true);
        
        Event.OnChangedState?.Invoke(state);
    }
    
    public void ChangeHp(int amount)
    {
        if (bossStat.CurrentHp <= 0) return;
        bossStat.CurrentHp += amount;
        bossStat.CurrentHp = Mathf.Clamp(bossStat.CurrentHp, 0, bossStat.MaxHp);
        Event.OnDamage?.Invoke(bossStat.CurrentHp, bossStat.MaxHp);
        
        if (bossStat.CurrentHp <= 0)
        {
            Animator.ResetTrigger(SCRATCH);
            Animator.ResetTrigger(BREATH);
            Animator.ResetTrigger(HIT);
            Animator.SetTrigger(DEAD);
        }
        // else
        // {
        //     BossStat.CurrentHitCount++;
        //     if (BossStat.CurrentHitCount >= BaseStat.HIT_COUNT)
        //     {
        //         BossStat.CurrentHitCount = 0;
        //         Animator.SetTrigger(HIT);
        //     }
        // }
        OnHpChanged?.Invoke();
    }
    public void TakeDamage(CombatEvent combatEvent)
    {
        ChangeHp(combatEvent.Damage * -1);
        Debug.Log($"{combatEvent.Collider.gameObject.name} {combatEvent.Damage} damage");
    }
    public float HowPainful(Collider collider)
    {
        if (collider == headCollider)
        {
            return 2;
        }
        else if (collider == spineCollider)
        {
            return 1;
        }
        else
        {
            return 0.5f;
        }
    }
}


