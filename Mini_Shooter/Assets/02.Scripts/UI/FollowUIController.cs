using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUIController : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        CombatSystem.Instance.Events.OnCombatEvent += PrintDamageText;
    }

    private void OnDestroy()
    {
        CombatSystem.Instance.Events.OnCombatEvent -= PrintDamageText;
    }
    
    private void PrintDamageText(CombatEvent combatEvent)
    {
        float textDuration = 3.0f;
        
        Tweener tweener = ObjectPoolManager.Instance.GetObjectOrNull("Tweener") as Tweener;
        tweener.transform.position = combatEvent.HitPosition;
        tweener.gameObject.SetActive(true);
        tweener.Set(textDuration);
        
        var parts = BossMonster.GetBossPart(combatEvent.Collider);
        Color color = GetPartsColor(parts);
        
        FollowUI followUI = ObjectPoolManager.Instance.GetObjectOrNull("DamageText") as FollowUI;
        followUI.gameObject.SetActive(true);
        followUI.Set(tweener.transform, $"{combatEvent.Damage}", textDuration, color);
    }

    private Color GetPartsColor(BossMonster.Parts parts)
    {
        Color reVal = Color.clear;
        
        switch (parts)
        {
            case BossMonster.Parts.Spine:
                reVal = Color.magenta;
                break;
            case BossMonster.Parts.Head:
                reVal = Color.red;
                break;
            default:
                reVal = Color.white;
                break;
        }
        
        return reVal;
    }
}


