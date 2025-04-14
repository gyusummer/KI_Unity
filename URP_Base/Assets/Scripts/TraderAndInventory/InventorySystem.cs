using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    #region 이렇게 하시면 안됩니다~ 저는 스크립트 늘리기 싫어서 그냥 하는거에요

    [FormerlySerializedAs("gameResourceTable")] [SerializeField] private TableSAO spriteTable;
    private List<Item> itemTable;

    private List<Item> userDataItems = new List<Item>();
    
    #endregion
    
    public static InventorySystem Instance { get; private set; }

    private InventorySlot SourceSlot { get; set; }
    [SerializeField] private InventorySlot dragSlot;

    private GraphicRaycaster raycaster;

    [SerializeField] private Inventory traderInventory;
    [SerializeField] private Inventory userInventory;
    
    
    
    private void Awake()
    {
        Instance = this;
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        InitTableData();
        traderInventory.Initialize(itemTable);
        
        
        userInventory.Initialize(null);
    }

    private void OnDestroy()
    {
       
    }

    /// <summary>
    /// 따로 관리자 만들어서 하시는게 좋아요
    /// </summary>
    private void InitTableData()
    {
        using (StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "ItemTable.csv")))
        using (CsvReader cr = new CsvReader(sr,CultureInfo.CurrentCulture))
        {
            List<ItemData> itemDatas = cr.GetRecords<ItemData>().ToList();
            List<Item> items = new List<Item>();
            
            for (int i = 0; i < itemDatas.Count; i++)
            {
                Item item = new Item();
                item.Data = itemDatas[i];
                item.Sprite = spriteTable.GetItemSprite(item.Data.Key).sprite;
                items.Add(item);
            }

            itemTable = items;
        }
    }

    private void InitUserItemsData()
    {
        
    }
    
    public void StartDrag(InventorySlot source)
    {
        // source를 캐싱을 해준다 
        SourceSlot = source;
        dragSlot.SetSlot(SourceSlot.Item);
    }

    //드래그 중일때 드래그 중인 슬롯의 위치를 갱신하기 위한 함수 
    public void UpdatePosition(Vector2 position)
    {
        dragSlot.transform.position = position;
    }

    //Drag가 끝날때의 이벤트 데이터를 사용해서 어느 인벤토리인지, 어느슬롯인지 판별
    public void EndDrag(PointerEventData eventData)
    {
        dragSlot.ClearSlot();
        
        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            InventorySlot targetSlot = 
                results[i].gameObject.GetComponent<InventorySlot>();
            if (targetSlot != null && 
                targetSlot != SourceSlot && targetSlot != dragSlot)
            {
                Inventory from = FindInventory(SourceSlot);
                Inventory to = FindInventory(targetSlot);

                if (from.Equals(to))
                {
                    //아무일도 일어나지 않음.
                }
                else
                {
                    if (from.Equals(traderInventory))
                    {
                        //Trader -> User (구매)
                        Debug.Log($"아이템 구매 {SourceSlot.gameObject.name}");
                    }
                    else
                    {
                        //User -> Trader (판매)
                        Debug.Log($"아이템 판매 {SourceSlot.gameObject.name}");
                    }
                }
                
                SwapItem(SourceSlot, targetSlot);
            }
        }
    }

    private void SwapItem(InventorySlot a, InventorySlot b)
    {
        var temp = a.Item;
        a.SetSlot(b.Item);
        b.SetSlot(temp);
    }

    private Inventory FindInventory(InventorySlot slot)
    {
        return traderInventory.IsIn(slot) ? traderInventory : userInventory;
    }
}
