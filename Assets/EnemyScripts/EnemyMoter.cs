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


    void start()
    {
        BaseManager.Instance.onStructureAdded += OnBuildFindTarget;
    }

    public void Update()
    {
       

        if(target_Current != null)
        {
            distFromTarget = Vector3.Distance(transform.position, target_Current.position);

            if (distFromTarget >= 2)
            {
                transform.Translate(dir * speed * Time.deltaTime, Space.World);
                FaceTarget();
            }
            else
            {
                attackStruct(damage);
            }
        }
        else
        {
            FindTarget();
        }

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

    public void FindTarget()
    {
        target_Current = BaseManager.Instance.GetClosestStructure(transform.position).transform;
        dir = (BaseManager.Instance.GetClosestStructure(transform.position).transform.position - transform.position).normalized;
    }

    public void OnBuildFindTarget(BaseStructure building)
    {
        if(Vector3.Distance(building.transform.position,transform.position) < distFromTarget)
        {
            target_Current = building.transform;
            dir = (building.transform.position - transform.position).normalized;
        }
    }

    public void attackStruct(float dmg)
    {

        Idamagable attempt = target_Current.GetComponent<Idamagable>();
            if (attempt != null)
            {
                attempt.TakeDamage(damage);
            }
    }
}

