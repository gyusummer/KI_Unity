using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Slider slider;
    public GameObject Target;
    private IDamageAble target;

    private void Start()
    {
        if (Target == null) return;
        SetTarget(Target.GetComponent<IDamageAble>());
    }

    private void UpdateHp()
    {
        slider.value = (float)target.Stat.CurrentHp / target.Stat.MaxHp;
    }

    public void SetTarget(IDamageAble target)
    {
        this.target = target;
        this.target.OnHpChanged += UpdateHp;
        UpdateHp();
    }

    public void OnDisable()
    {
        target.OnHpChanged -= UpdateHp;
    }
}
