using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class NotePad : MonoBehaviour
{
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


        public override string ToString()
        {
            string result;

            result = string.Concat(Name, ",") ;
            result += string.Concat(Age.ToString(), ",");
            result += string.Concat(Position, ",");
            result += string.Concat(Club, ",");
            result += string.Concat(MinutesPlayed, ",");
            result += string.Concat(Nation, ",");
            result += string.Concat(Appearances.ToString(), ",");
            result += string.Concat(Goals.ToString(), ",");
            result += string.Concat(Assists.ToString());
            return result;
        }


    }
    
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
        
        StreamWriter sw = new StreamWriter(Path.Combine(Application.streamingAssetsPath,"Players.txt") );

        for (int i = 0; i < players.Length; i++)
        {
            sw.WriteLine(players[i].ToString());
        }
     
        sw.Close();
        
        StreamReader sr = new StreamReader("Players.txt");

        List<Player> playerList = new List<Player>();


        while (sr.EndOfStream == false)
        {
            playerList.Add(ParsePlayer(sr.ReadLine()));
        }
        
        sr.Close();


        foreach (var player in players)
        {
            Debug.Log(player.ToString());
        }

        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);
    }

    private Player ParsePlayer(string rawData)
    {
        Player p = new Player();
        string[] playerRawData = rawData.Split(',');
        p.Name = playerRawData[0];
        p.Age = int.Parse(playerRawData[1]);
        p.Position = playerRawData[2];
        p.Club = playerRawData[3];
        p.MinutesPlayed = playerRawData[4];
        p.Nation = playerRawData[5];
        p.Appearances = int.Parse(playerRawData[6]);
        p.Goals = int.Parse(playerRawData[7]);
        p.Assists = int.Parse(playerRawData[8]);
        return p;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
        }
    }


    private bool Quiz1(Player[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Name.Equals("Aaron Lennon"))
            {
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
