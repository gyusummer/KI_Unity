using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BossMonster
{
    public enum Parts
    {
        Unknown,
        Spine,
        Head,
        LeftArm,
        RightArm,
    }

    public static Parts GetBossPart(Collider collider)
    {
        return CurrentSceneBossMonster.BossPartsGroup.GetBossPart(collider);
    }
    
    public static BossMonster CurrentSceneBossMonster;
}
