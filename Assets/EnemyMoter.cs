using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoter : MonoBehaviour {

    public float lookRadius = 10f;

    public Transform target_Base;
    Transform target_Closest;
    Vector3 dir;

    public void Update()
    {
        dir = target_Base.position - transform.position;
        dir = new Vector3.


        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawLine(transform.position, dir);
    }

}
