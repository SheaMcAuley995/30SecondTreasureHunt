using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPoint : MonoBehaviour {

    [SerializeField]
    private float radius = 0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
