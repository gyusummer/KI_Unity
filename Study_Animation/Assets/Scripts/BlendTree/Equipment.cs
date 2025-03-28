using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    [Range(0f, 1f)]
    public float IKWeight;
}
