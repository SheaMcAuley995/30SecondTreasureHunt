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
    public bool isCore;

    public bool Activated
    {
        get
        {
            return activated;
        }
    }
    private bool activated = true;

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
            foreach (BaseStructure strct in connections)
            {
                if (strct.Activated && !strct.IsConnectedToCore(this))
                {
                    strct.Deactivate();
                }
            }
            BaseManager.Instance.DestroyStructure(this);
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

    public bool IsConnectedToCore(BaseStructure caller)
    {
        if(isCore)
        {
            return true;
        }

        foreach(BaseStructure strct in connections)
        {
            if(strct.isCore)
            {
                return true;
            }
        }

        float lastClosest = float.MinValue;
        BaseStructure toCheck = null;
        float currentClosest = float.MaxValue;
        int connsChecked = 0;
        while(connsChecked < connections.Count)
        {
            foreach (BaseStructure strct in connections)
            {
                if(strct == caller)
                {
                    continue;
                }

                float dist = Vector3.Distance(strct.transform.position,
                                              BaseManager.Instance.GetCore().transform.position);
                if(dist > lastClosest && dist < currentClosest)
                {
                    toCheck = strct;
                    currentClosest = dist;
                }
            }

            if(toCheck.IsConnectedToCore(this))
            {
                return true;
            }

            lastClosest = currentClosest;
            ++connsChecked;
        }

        return false;
    }

    public void Deactivate()
    {
        activated = false;
        foreach(BaseStructure strct in connections)
        {
            if(strct.Activated)
            {
                strct.Deactivate();
            }
        }
    }

    public void Activate()
    {
        activated = true;
        foreach(BaseStructure strct in connections)
        {
            if(!strct.Activated)
            {
                strct.Activate();
            }
        }
    }

}
