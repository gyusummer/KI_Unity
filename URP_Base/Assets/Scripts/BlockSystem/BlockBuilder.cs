using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    private Camera mainCam;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                bool isSuccess = BlockSystem.Instance.PlaceBlock(hit.point);
                
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                bool isSuccess = BlockSystem.Instance.RemoveBlock(hit.point);
                Debug.Log($"Remove Block : {isSuccess}, {hit.point}");
            }
        }

    }
}
