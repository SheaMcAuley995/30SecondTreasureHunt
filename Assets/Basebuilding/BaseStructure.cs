using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : MonoBehaviour, Idamagable {

    public float personalSpace;
    public bool isConnector;
    public LineRenderer lr;

    private float health;

	public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    public void ConnectToStructure(Vector3 pos)
    {
        Debug.Log(lr);
        if(lr != null)
        {
            lr.positionCount += 2;
            lr.SetPosition(lr.positionCount - 2, transform.position);
            lr.SetPosition(lr.positionCount - 1, pos);
        }
    }

}
