using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Parts = BossMonster.Parts;

public class BossParts
{
    public Parts Part;
    public Collider Collider;
}

[System.Serializable]
public class BossPartsGroup
{
    public Collider spineCollider;
    public Collider headCollider;
    public Collider[] leftArmCollider;
    public Collider[] rightArmCollider;

    public BossParts[] BossPartsArray;

    public void Initialize()
    {
        IEnumerable<Collider> colliders = 
            new[] { spineCollider, headCollider }.Concat(rightArmCollider).Concat(leftArmCollider);

        var colliderArray = colliders.ToArray();
        
        List<Parts> parts = new List<Parts>();
        parts.Add(Parts.Spine);
        parts.Add(Parts.Head);
        parts.AddRange(Enumerable.Repeat(Parts.RightArm, rightArmCollider.Length));
        parts.AddRange(Enumerable.Repeat(Parts.LeftArm, leftArmCollider.Length));
        
        BossPartsArray = new BossParts[colliders.Count()];
        
        for (int i = 0; i < BossPartsArray.Length; i++)
        {
            BossPartsArray[i] = new BossParts();
            BossPartsArray[i].Collider = colliderArray[i];
            BossPartsArray[i].Part = parts[i];
        }
    }
    
    public Parts GetBossPart(Collider collider)
    {
        for (int i = 0; i < BossPartsArray.Length; i++)
        {
            if(BossPartsArray[i].Collider == collider) return BossPartsArray[i].Part;
        }

        return Parts.Unknown;
    }
}
