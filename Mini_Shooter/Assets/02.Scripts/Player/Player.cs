using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static LocalPlayer LocalPlayer;


    public class PlayerStat
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
    }
}
