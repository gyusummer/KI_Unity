using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);
        if (leftClick || rightClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (!hit.collider) return;

            IClickable target = null;
            if (leftClick && hit.collider.CompareTag("Monster"))
            {
                hit.collider.transform.root.TryGetComponent(out target);
            }
            if (rightClick && hit.collider.CompareTag("Player"))
            {
                hit.collider.transform.TryGetComponent(out target);
            }
            target?.OnClick();
        }
    }
}
