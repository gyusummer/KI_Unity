using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextPool : MonoBehaviour
{
    public GameObject damageTextPrefab;

    private IObjectPool<DamageText> m_pool;
    public IObjectPool<DamageText> Pool
    {
        get
        {
            if (m_pool == null)
            {
                m_pool = new ObjectPool<DamageText>(
                    CreateDamageText,
                    OnGetDamageText,
                    OnReleaseDamageText,
                    OnDestroyDamageText,
                    false,
                    10,
                    100
                );
                return m_pool;
            }
            else
            {
                return m_pool;
            }
        }
    }

    private DamageText CreateDamageText()
    {
        GameObject gameObject = Instantiate(damageTextPrefab, transform);
        DamageText text = gameObject.GetComponent<DamageText>();
        text.pool = Pool;
        text.gameObject.SetActive(false);

        return text;
    }
    private void OnGetDamageText(DamageText text)
    {
        text.gameObject.SetActive(true);
    }
    private void OnReleaseDamageText(DamageText text)
    {
        text.gameObject.SetActive(false);
    }
    private void OnDestroyDamageText(DamageText text)
    {
        Destroy(text.gameObject);
    }
    private void Start()
    {
        CombatSystem.Instance.Events.OnCombatEvent += ShowDamage;
        for (int i = 0; i < 10; i++)
        {
            // Pool.Get();
            Pool.Release(CreateDamageText());
        }
    }
    private void OnDestroy()
    {
        CombatSystem.Instance.Events.OnCombatEvent -= ShowDamage;
    }
    private void ShowDamage(CombatEvent combatEvent)
    {
        Pool.Get().Initialize(combatEvent.HitPosition, combatEvent.Damage);
    }
}
