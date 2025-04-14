using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Sprite Sprite; //아이콘이나 출력될 이미지
    public ItemData Data { get; set; }
}

public class ItemData // csv나 json등으로 직렬화할 대상
{
    public string Name { get; set; }
    
    //key(UserID같은거)는 일반적으로 대부분 uint나 long을 사용
    //플랫폼에 따라 string을 key값으로 쓰는곳도 있음. uint나 long
    //전세계적으로 Node.js나 Next.js 같은걸로 서버를 개발하기 때문에
    //대부분 js나 db에 사용되는 bigint와 대응 되는 자료형을 사용. 
    //근데 게임은 그정도로 데이터 량이 많지 않기 때문에 int나 byte를 사용함
    public int Key { get; set; }
    
    //특수한 문자열로 아이템의 기능 매개변수들을 넣음
    //ex) Heal_50 => 50만큼 Heal하라!
    //ex) Buff_BF003 => 003번의 버프를 시전
    public string Parameter { get; set; }  
}