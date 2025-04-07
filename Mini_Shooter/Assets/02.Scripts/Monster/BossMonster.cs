using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class BossMonster : MonoBehaviour, IDamageAble
{
    public static readonly int SCRATCH = Animator.StringToHash("Scratch");
    public static readonly int BREATH = Animator.StringToHash("Breath");

    public class BossMonsterStat
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
    }
    
    public Collider MainCollider => BossPartsGroup.spineCollider;
    public GameObject GameObject => gameObject;
    public BossMonsterStat Stat { get; private set; }
    public BossState CurrentState { get; private set; }
    
    public BossPartsGroup BossPartsGroup;
    public Animator animator;

    private Dictionary<BossState.StateName, BossState> statesDic = new Dictionary<BossState.StateName, BossState>();
    
    private void Awake()
    {
        CurrentSceneBossMonster = this;
        Stat = new BossMonsterStat();

        Stat.HP = 5000;
        Stat.MaxHP = 5000;
    }

    private void Update()
    {
        Debug.Log(CurrentSceneBossMonster);
    }

    private void Start()
    {
        BossPartsGroup.Initialize();
        var bossPartsArray = BossPartsGroup.BossPartsArray;
        
        for (int i = 0; i < bossPartsArray.Length; i++)
            CombatSystem.Instance.RegisterMonster(bossPartsArray[i].Collider, this);
        
        var states = GetComponentsInChildren<BossState>();
        for (int i = 0; i < states.Length; i++)
        {
            statesDic.Add(states[i].Name, states[i]);
            states[i].Initialize(this);
            states[i].gameObject.SetActive(false);
        }

        ChangeState(BossState.StateName.IdleState);
    }

    public void ChangeState(BossState.StateName newState)
    {
        if (CurrentState != null)
        {
            var prev = CurrentState;
            prev.Exit();
            prev.gameObject.SetActive(false);
        }
        
        var target = statesDic[newState];
        CurrentState = target;        
        target.Enter();
        target.gameObject.SetActive(true);
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        Stat.HP -= combatEvent.Damage;
        
        if (Stat.HP <= 0)
        {
            //죽음 이벤트 (승리)
        }
    }

    public void TakeHeal(HealEvent healEvent)
    {
        Stat.HP += healEvent.Heal;
        Stat.HP = Mathf.Min(Stat.HP, Stat.MaxHP);
    }
}
