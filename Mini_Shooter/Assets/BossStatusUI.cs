using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStatusUI : StatusUI
{
    public TMP_Text nameText;
    private BossMonster boss;
    
    // Start is called before the first frame update
    private void Start()
    {
        boss = BossMonster.CurrentSceneBossMonster;
        nameText.text = boss.name;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFillImage(boss.Stat.HP, boss.Stat.MaxHP);
    }
}
