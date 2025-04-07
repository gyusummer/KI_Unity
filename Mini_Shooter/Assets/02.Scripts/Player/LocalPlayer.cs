using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LocalPlayer : Player, IDamageAble
{
    public class CallBacks
    {
        public Action<TouchItemEvent> OnTouchedItem;
        public Action OnStartChangedWeapon;
        public Action OnEndedChangedWeapon;
        public Action OnStartedReload;
        public Action OnEndedReload;
        
        public Action<int, int, int> OnTakenDamage; 
    }
    
    public Collider MainCollider { get; }
    public GameObject GameObject { get; }
    public Weapon CurrentWeapon { get; set; }
    public PlayerStat Stat { get; private set; }
    
    public CallBacks Events = new CallBacks();

    [SerializeField] private Transform weaponParent;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private AnimStateEventListener characterAnimatorListener;
    
    private void Awake()
    {
        Player.LocalPlayer = this;
        
        Stat = new PlayerStat();
        Stat.MaxHP = 100;
        Stat.HP = Stat.MaxHP;
    }

    private void Start()
    {
        var changeTarget = weaponController.GetWeaponOrNull(Weapon.WeaponType.AssaultRifle); 
        ChangeWeapon(changeTarget);
    }

    private void OnEnable()
    {
        characterAnimatorListener.OnOccursAnimStateEvent += OnAnimEvent;
    }

    private void OnDisable()
    {
        characterAnimatorListener.OnOccursAnimStateEvent -= OnAnimEvent;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        Stat.HP -= combatEvent.Damage;

        if (Stat.HP <= 0)
        {
            //죽음 이벤트 (패배)
        }
        
        Events.OnTakenDamage?.Invoke(combatEvent.Damage, Stat.HP, Stat.MaxHP);
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var triggerItem = other.GetComponent<TriggerItem>();
        if (triggerItem == null) return;

        TouchItemEvent touchItemEvent = new TouchItemEvent();
        touchItemEvent.Type = triggerItem.type;
        touchItemEvent.Position = other.ClosestPoint(transform.position);
        Events.OnTouchedItem?.Invoke(touchItemEvent);

        if (TriggerItem.TriggerItemType.Unknown < triggerItem.type && triggerItem.type < TriggerItem.TriggerItemType.HealPack)
        {
            var weaponType = Mapper.GetWeaponType(triggerItem.type);
            var changeTarget = weaponController.GetWeaponOrNull(weaponType);
            if(CurrentWeapon.Equals(changeTarget) == false) ChangeWeapon(changeTarget); 
            else Events.OnStartedReload?.Invoke();
        }
        
        Destroy(triggerItem.gameObject);
    }

    private void ChangeWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
        {
            var prevWeapon = CurrentWeapon;
            prevWeapon.UpdateTransform(weaponController.transform);
            prevWeapon.gameObject.SetActive(false);
        }
        
        CurrentWeapon = weapon;
        CurrentWeapon.gameObject.SetActive(true);
        CurrentWeapon.UpdateTransform(weaponParent);
        Events.OnStartChangedWeapon?.Invoke();
        CurrentWeapon.Reload();
    }

    private void OnAnimEvent(string eventName, string parameter)
    {
        switch (eventName)
        {
            case "ReloadComplete":
                CurrentWeapon.Reload();
                Events.OnEndedReload?.Invoke();
                break;
            case "SwapComplete":
                Events.OnEndedChangedWeapon?.Invoke();
                break;
            default:
                break;
        }
    }
}
