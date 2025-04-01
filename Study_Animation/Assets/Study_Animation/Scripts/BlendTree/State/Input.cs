using System;
using UnityEngine;

namespace Mine
{
    public class Input : MonoBehaviour
    {
        public static event Action<Vector2> OnGetMove;
        
        private Vector2 moveInput;
        private bool attackInput;
        private void Update()
        {
            moveInput = new Vector2(GetAxisRaw("Horizontal"), GetAxisRaw("Vertical"));
            attackInput = GetKeyDown(KeyCode.Mouse0);
        }

        private static float GetAxisRaw(String axis)
        {
            return UnityEngine.Input.GetAxisRaw(axis);
        }
        private static bool GetKeyDown(KeyCode key)
        {
            return UnityEngine.Input.GetKeyDown(key);
        }
    }
}