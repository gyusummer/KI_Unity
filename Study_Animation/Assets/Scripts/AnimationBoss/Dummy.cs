using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Dummy : MonoBehaviour, IClickable
{
    private static readonly int BASE_COLOR = Shader.PropertyToID("_BaseColor");
    private Material material;
    private int maxHp = 3;
    private int currentHp;

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = value;
            switch (value)
            {
                case 3:
                    material.SetColor(BASE_COLOR,Color.gray);
                    break;
                case 2:
                    material.SetColor(BASE_COLOR,Color.yellow);
                    break;
                case 1:
                    material.SetColor(BASE_COLOR,Color.red);
                    break;
            }

            if (currentHp <= 0)
            {
                Object.Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
        CurrentHp = maxHp;
    }

    public void OnClick()
    {
        GetHeal();
    }

    public void GetDamage()
    {
        CurrentHp--;
    }

    private void GetHeal()
    {
        CurrentHp = maxHp;
    }
}
