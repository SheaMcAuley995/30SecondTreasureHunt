using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoter : MonoBehaviour {

    public float lookRadius = 10f;
    public float speed = 10f;

    public Transform target_Base;
    Transform target_Current;
    Vector3 dir;

    public void Update()
    {

        dir = (BaseManager.Instance.GetClosestStructure(transform.position).transform.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime ,Space.World);
        FaceTarget();
        
    }

    void FaceTarget()
    {
        Vector3 faceDir = (target_Current.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(faceDir.x, 0, faceDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, lookRadius);
       
    }

}
