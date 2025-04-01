using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public GameObject breathJudge;
    public GameObject swingJudge;

    // private void Awake()
    // {
    //     breathJudge.SetActive(false);
    //     swingJudge.SetActive(false);
    // }
    public void OnBreathJudge()
    {
        breathJudge.SetActive(true);
    }
    public void OffBreathJudge()
    {
        breathJudge.SetActive(false);
    }
    public void OnSwingJudge()
    {
        swingJudge.SetActive(true);
    }
    public void OffSwingJudge()
    {
        swingJudge.SetActive(false);
    }
}
