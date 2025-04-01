using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvent
{
    public IDamageAble Sender { get; set; }
    public IDamageAble Receiver { get; set; }
    
    public int Damage { get; set; }
    public Vector3 HitPosition { get; set; }
}
