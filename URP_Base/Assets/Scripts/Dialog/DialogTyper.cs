using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Random = UnityEngine.Random;


public class DialogTyper : MonoBehaviour
{
    public class DialogData
    {
        public string Category { get; set; }
        public string Key { get; set; }
        public string Kor { get; set; }
    }
    
    public TMP_Text nameComponent;
    public TMP_Text textComponent;

    private string[] category = { "방어적 프로그래밍", "멀티 쓰레드", "문서화" };

    private Tween currentTween;
    private IEnumerator currentCoroutine;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentCoroutine != null) StopCoroutine(currentCoroutine);
            
            int randomIndex = Random.Range(0, category.Length);
            DialogData[] dialogs = GetDialogsOrNull(category[randomIndex]);
            nameComponent.SetText(category[randomIndex]);
            currentCoroutine = PlayDialog(dialogs); 
            
            StartCoroutine(currentCoroutine);
        }
    }

    private DialogData[] GetDialogsOrNull(string category)
    {
        DialogData[] dialogs = null;
        
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = true,
            Mode = CsvMode.NoEscape,
            BadDataFound = null
        };

        using (StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "DialogTable.tsv")))
        using (CsvReader cr = new CsvReader(sr,csvConfig))
        {
            var records = cr.GetRecords<DialogData>();
            List<DialogData> dialogList = new List<DialogData>();
            foreach (var record in records)
            {
                if(record.Category.Equals(category)) dialogList.Add(record);
            }
            dialogs = dialogList.ToArray();
        }
        
        return dialogs;
    }
    
    public Tween PlayText(string text)
    {
        textComponent.SetText("");
        var tween = textComponent.DOText(text, text.Length * 0.1f).SetEase(Ease.Linear);
        return tween;
    }

    IEnumerator PlayDialog(DialogData[] dialogs)
    {
        for (int i = 0; i < dialogs.Length; i++)
        {
            var tween = PlayText(dialogs[i].Kor);
            yield return new WaitWhile( () => tween.active);
            yield return new WaitForSeconds(3.0f);
        }
    }
}
