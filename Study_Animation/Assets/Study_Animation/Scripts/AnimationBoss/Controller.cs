using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // int layerMask = (-1) - (1 << LayerMask.NameToLayer("Skill"));
            // Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
            Physics.Raycast(ray, out hit);
            if (!hit.collider) return;
            if (hit.collider.transform.root.TryGetComponent(out IClickable chara))
            {
                chara.OnClick();
            }
        }
    }
}
