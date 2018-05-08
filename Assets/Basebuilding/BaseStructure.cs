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

    public Material connectionOnMat;
    public Material connectionOffMat;

    public bool Activated
    {
        get
        {
            return activated;
        }
    }
    private bool activated = true;

    private bool checkedForCoreConnection = false;
    private bool savedCoreConnectAnswer = false;

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
            activated = false;
            foreach (BaseStructure strct in connections)
            {
                if (strct.Activated && !strct.IsConnectedToCore(this))
                {
                    strct.Deactivate();
                }
                BaseManager.Instance.ResetCoreChecks();
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
        if(!checkedForCoreConnection)
        {
            checkedForCoreConnection = true;
        }
        else
        {
            return savedCoreConnectAnswer;
        }

        if(isCore)
        {
            savedCoreConnectAnswer = true;
            return true;
        }

        foreach(BaseStructure strct in connections)
        {
            if(strct.isCore)
            {
                savedCoreConnectAnswer = true;
                return true;
            }
        }

        if (connections.Count == 1)
        {
            savedCoreConnectAnswer = true;
            return false;
        }

        float lastClosest = float.MinValue;
        BaseStructure toCheck = null;
        float currentClosest = float.MaxValue;
        int connsChecked = 0;
        while(connsChecked < connections.Count)
        {
            currentClosest = float.MaxValue;
            
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
                savedCoreConnectAnswer = true;
                return true;
            }

            lastClosest = currentClosest;
            ++connsChecked;
        }

        savedCoreConnectAnswer = true;
        return false;
    }

    public void ResetCoreCheck()
    {
        checkedForCoreConnection = false;
    }

    public void Deactivate()
    {
        activated = false;
        if(lr != null)
        {
            lr.material = connectionOffMat;
        }
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
        if (lr != null)
        {
            lr.material = connectionOnMat;
        }
        foreach (BaseStructure strct in connections)
        {
            if(!strct.Activated)
            {
                strct.Activate();
            }
        }
    }

}
