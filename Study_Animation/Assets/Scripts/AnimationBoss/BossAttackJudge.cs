using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackJudge : MonoBehaviour
{
    private List<Dummy> hit = new List<Dummy>();

    private void OnEnable()
    {
        hit.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Dummy dum = other.GetComponent<Dummy>();
            if (!hit.Contains(dum))
            {
                hit.Add(dum);
                dum.GetDamage();
            }
        }
    }
}
