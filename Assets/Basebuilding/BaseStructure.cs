using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : MonoBehaviour, Idamagable {

    public float energyCost;
    public string structureName;
    public float personalSpace;
    public float maxHealth;
    public bool isConnector;
    public LineRenderer lr;
    public bool isEnergyGen;
    public float energyPerSecond;

    private float health;
    private List<BaseStructure> connections = new List<BaseStructure>();



    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            BaseManager.Instance.DestroyStructure(this);
            Destroy(gameObject);
        }
        else if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void ConnectToStructure(BaseStructure other, bool initiator = true)
    {
        connections.Add(other);
        if(initiator)
        {
            other.ConnectToStructure(this, false);

            if (lr != null)
            {
                lr.positionCount += 2;
                lr.SetPosition(lr.positionCount - 2, transform.position);
                lr.SetPosition(lr.positionCount - 1, other.transform.position);
            }
        }
    }

    public void DisconnectNeighbors()
    {
        foreach(BaseStructure other in connections)
        {
            other.DisconnectFrom(this);
        }
    }

    public void DisconnectFrom(BaseStructure other)
    {
        connections.Remove(other);

        if(lr != null)
        {
            lr.positionCount = 0;

            foreach (BaseStructure strct in connections)
            {
                lr.positionCount += 2;
                lr.SetPosition(lr.positionCount - 2, transform.position);
                lr.SetPosition(lr.positionCount - 1, strct.transform.position);
            }
        }
    }

}
