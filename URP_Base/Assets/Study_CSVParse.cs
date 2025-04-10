using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using CsvHelper;

using StringWriter = System.IO.StringWriter;

public class Study_CSVParse : MonoBehaviour
{
    [System.Serializable]
    public class Player
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position {get; set; }
        public string Club { get; set; }
        public string MinutesPlayed { get; set; }
        public string Nation { get; set; }
        public int Appearances { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }

        //원래 이렇게 하면 안되는데 전 그냥 할게요 - 재훈쌤-
        public override string ToString()
        {
            string result = "";

            result += Name + ",";
            result += Age + ",";
            result += Position + ",";
            result += Club + ",";
            result += MinutesPlayed + ",";
            result += Nation + ",";
            result += Appearances + ",";
            result += Goals + ",";
            result += Assists;

            return result;
        }
    }
    
    //클래스 명은 Player
    
    private string rawData = 
        "Aaron Cresswell,32,Defender,West Ham United,1589,England,20,0,1\nAaron Lennon,35,Midfielder,Burnley,1217,England,16,1,1\nAaron Mooy,32,Midfielder,Huddersfield Town,2327,Australia,29,3,1";
    
    // Start is called before the first frame update
    void Start()
    {
       string[] playerRows = rawData.Split('\n');

       Player[] players = new Player[playerRows.Length];

       for (int i = 0; i < playerRows.Length; i++)
       {
           players[i] = ParsePlayer(playerRows[i]);
       }
       //----------여기까지가 메모리에 올려본 내용-------------

       players[0].Age = 23;
       players[1].Age = 53;
       players[2].Age = 23;
       
       //

       StreamWriter writer = new StreamWriter("Players.txt");

       for (int i = 0; i < players.Length; i++)
       {
           writer.WriteLine(players[i].ToString());
       }
       
       writer.Close();
       //----------여기까지가 드라이브에 올려본 내용-------------
       
       StreamReader reader = new StreamReader("Players.txt");

       List<Player> playerList = new List<Player>();
       while (!reader.EndOfStream)
       {
           string line = reader.ReadLine();
           playerList.Add(ParsePlayer(line));
       }
       
       // CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
       // var playerList = csvReader.GetRecords<Player>().ToList();
  
       reader.Close();

       for (int i = 0; i < playerList.Count; i++)
       {
           Debug.Log(playerList[i].ToString());
       }
       
       //----------드라이브 -> 메모리로 올려본 내용-------------
       
       //유니티에서 사용가능한 경로 4가지

       Debug.Log(Application.persistentDataPath); // 앱의 고유 경로(읽기/쓰기 가능)
       Debug.Log(Application.temporaryCachePath); // 앱의 임시 경로(읽기/쓰기 가능. 근데 종료 시 없어짐)
       
       Debug.Log(Application.streamingAssetsPath);
       
    }

    private Player ParsePlayer(string playerRow)
    {
        Player p = new Player();
        string[] playerData = playerRow.Split(',');
        
        p.Name = playerData[0];
        p.Age = int.Parse(playerData[1]);
        p.Position = playerData[2];
        p.Club = playerData[3];
        p.MinutesPlayed = playerData[4];
        p.Nation = playerData[5];
        p.Appearances = int.Parse(playerData[6]);
        p.Goals = int.Parse(playerData[7]);
        p.Assists = int.Parse(playerData[8]);
        return p;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private bool Quiz1(Player[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Name.Equals("Aaron Lennon"))
            {
                // return players[i].Goal == 16; 위에서 아래로 바꿔주세여
                return players[i].Appearances == 16;
            }
        }

        return false;
    }
    
    private bool Quiz2(Player[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Name.Equals("Aaron Mooy"))
            {
                return players[i].Position.Equals("Midfielder");
            }
        }

        return false;
    }
    
}
