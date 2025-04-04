using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BossMonster
{
    public static BossMonster CrrentSceneBossMonster;

    public static readonly int SCRATCH = Animator.StringToHash("Scratch");
    public static readonly int BREATH = Animator.StringToHash("Breath");
    public static readonly int DEAD = Animator.StringToHash("Dead");
    public static readonly int HIT = Animator.StringToHash("Hit");
    
    private static Collider[] s_bossMonsterColliders;
    
    public static bool IsBossMonster(Collider collider)
    {
        for (int i = 0; i < s_bossMonsterColliders.Length; i++)
        {
            if (s_bossMonsterColliders[i] == collider) return true;
        }
        return false;
    }
}
