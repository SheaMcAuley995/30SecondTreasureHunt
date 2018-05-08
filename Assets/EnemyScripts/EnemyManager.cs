using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public List<EnemyMoter> enemies = new List<EnemyMoter>();

    public EnemyMoter getClosestEnemy(Vector3 pos) 
    {
        EnemyMoter ret = null;
        float minDist = float.MaxValue;
        float thisDist;

        foreach(EnemyMoter enemy in enemies)
        {
            thisDist = Vector3.Distance(enemy.transform.position, pos);
            if(thisDist < minDist)
            {
                ret = enemy;
                minDist = thisDist;
            }
        }
        return ret;
    }

}
