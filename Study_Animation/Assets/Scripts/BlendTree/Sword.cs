using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Sword : MonoBehaviour
{
    public ParticleSystem effect;
    // private List<Vector3> gizmoPoints;


    private void OnCollisionEnter(Collision other)
    {
        effect.transform.position = other.contacts[0].point;
        effect.transform.forward = other.contacts[0].normal;
        effect.Play();
        
        // Debug.Log(other.contacts.Length);
        // foreach (ContactPoint contact in other.contacts)
        // {
        //     gizmoPoints.Add(contact.point);
        // }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Debug.Log(gizmoPoints.Count);
    //     foreach (Vector3 v in gizmoPoints)
    //     {
    //         Gizmos.DrawSphere(v, 0.1f);
    //     }
    // }
}
