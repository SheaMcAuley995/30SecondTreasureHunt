using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoter : MonoBehaviour, Idamagable {

    public float lookRadius = 10f;
    public float speed = 10f;


    public Transform target_Base;
    Transform target_Current;
    Vector3 dir;

    float distFromTarget;

    public float attackDist;

    [SerializeField]
    private float damage;
    [SerializeField]
    private float health;


    public GameObject effectPrefab;
    public LayerMask whatToHit;

    public void Update()
    {
        distFromTarget = Vector3.Distance(transform.position, target_Current.position);
        if (target_Current != null)
        {
            target_Current = target_Base;
        }
        if (distFromTarget <= attackDist)
        {
            attackStruct(damage);
        }

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

    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    public void attackStruct(float dmg)
    {
        RaycastHit hit; 
        Physics.Raycast(transform.position, dir, out hit, 15f);

        if (hit.collider.tag == "Building")
        {
            Idamagable attempt = hit.collider.GetComponent<Idamagable>();
            if (attempt != null)
            {
                attempt.TakeDamage(damage);
            }
        }


    }
}
