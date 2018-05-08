using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : MonoBehaviour, Idamagable {

    [Header("Basics")]
    public float energyCost;
    public string structureName;
    public float personalSpace;
    public float maxHealth;
    [Header("Connector")]
    public bool isConnector;
    public LineRenderer lr;
    [Header("Generator")]
    public bool isEnergyGen;
    public float energyPerSecond;
    [Header("Gun")]
    public bool isGun;
    public float fireRate;
    public float range;
    public float damage;
    public float shotEnergyCost;
    public LineRenderer shotRenderer;
    public float shotRenderTime;
    [Header("Core")]
    public bool isCore;
    [Header("Other")]
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

    private float health;
    private float gunHeat = 0;
    private List<BaseStructure> connections = new List<BaseStructure>();



    private void Awake()
    {
        health = maxHealth;
        shotRenderer.enabled = false;
        shotRenderer.positionCount = 2;
        shotRenderer.SetPosition(0, transform.position);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            activated = false;
            foreach (BaseStructure strct in connections)
            {
                checkedForCoreConnection = true;
                if (isCore || (strct.Activated && !strct.IsConnectedToCore()))
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

    public bool IsConnectedToCore()
    {
        if(!checkedForCoreConnection)
        {
            checkedForCoreConnection = true;
        }
        else
        {
            return false;
        }

        if(isCore)
        {
            return true;
        }

        foreach(BaseStructure strct in connections)
        {
            if(!strct.checkedForCoreConnection && strct.isCore)
            {
                return true;
            }
        }

        foreach (BaseStructure strct in connections)
        {
            if (strct.IsConnectedToCore())
            {
                return true;
            }
        }

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

    public void GunUpdate(float dt)
    {
        if(gunHeat <= 0)
        {
            //shoot
            shotRenderer.SetPosition(1, Vector3.zero); //will be enemy pos
            shotRenderer.enabled = true;
            CancelInvoke();
            Invoke("ShutoffShotRenderer", shotRenderTime);
        }
    }

    private void ShutoffShotRenderer()
    {
        shotRenderer.enabled = false;
    }

}
