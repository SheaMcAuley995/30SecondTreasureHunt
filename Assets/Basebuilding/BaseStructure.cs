using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float gunCooldown;
    public float gunRange;
    public float damage;
    public float shotEnergyCost;
    public LineRenderer shotRenderer;
    public float shotRenderTime;
    [Header("Repair")]
    public bool isRepair;
    public float repairCooldown;
    public float repairRange;
    public float repairAmt;
    public float repairEnergyCost;
    public LineRenderer repairRenderer;
    public float repairRenderTime;
    [Header("Core")]
    public bool isCore;
    [Header("Other")]
    public Material connectionOnMat;
    public Material connectionOffMat;
    public GameObject healthCanvas;
    public RectTransform greenHealth;

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
    public float Health
    {
        get
        {
            return health;
        }
    }
    private float gunHeat = 0;
    private float repairHeat = 0;
    private List<BaseStructure> connections = new List<BaseStructure>();
    private EnemyMoter gunTarget = null;
    private BaseStructure repairTarget = null;



    private void Awake()
    {
        health = maxHealth;
        healthCanvas.SetActive(false);
    }

    private void Start()
    {
        if (shotRenderer != null)
        {
            shotRenderer.enabled = false;
            shotRenderer.positionCount = 2;
            shotRenderer.SetPosition(0, transform.position);
        }
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
        else if(health >= maxHealth)
        {
            health = maxHealth;
            healthCanvas.SetActive(false);
        }
        else
        {
            healthCanvas.SetActive(true);
            greenHealth.sizeDelta = new Vector2(health / maxHealth, greenHealth.sizeDelta.y);
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
        if(gunHeat <= 0 && activated)
        {
            if(gunTarget == null
            || Vector3.Distance(gunTarget.transform.position, transform.position) > gunRange)
            {
                EnemyMoter enemy = EnemyManager.Instance.getClosestEnemy(transform.position);
                if (enemy != null
                && Vector3.Distance(enemy.transform.position, transform.position) <= gunRange)
                {
                    gunTarget = enemy;
                }
                else
                {
                    gunTarget = null;
                }
            }
            
            if(gunTarget != null && BaseManager.Instance.Energy >= shotEnergyCost)
            {
                gunTarget.TakeDamage(damage);
                BaseManager.Instance.DrainEnergy(shotEnergyCost);
                gunHeat = gunCooldown;
                shotRenderer.SetPosition(1, gunTarget.transform.position);
                shotRenderer.enabled = true;
                CancelInvoke();
                Invoke("ShutoffShotRenderer", shotRenderTime);
            }
        }
        else if(gunHeat > 0)
        {
            gunHeat -= dt;
        }
    }

    private void ShutoffShotRenderer()
    {
        shotRenderer.enabled = false;
    }

    public void RepairUpdate(float dt)
    {
        if (repairHeat <= 0 && activated)
        {
            if (repairTarget == null
            || repairTarget.Health >= repairTarget.maxHealth
            || Vector3.Distance(repairTarget.transform.position, transform.position) > repairRange)
            {
                BaseStructure friend = BaseManager.Instance.GetClosestDamagedStructure(transform.position);
                if (friend != null
                && Vector3.Distance(friend.transform.position, transform.position) <= repairRange)
                {
                    repairTarget = friend;
                }
                else
                {
                    repairTarget = null;
                }
            }
            
            if (repairTarget != null && BaseManager.Instance.Energy >= repairEnergyCost)
            {
                repairTarget.TakeDamage(-repairAmt);
                BaseManager.Instance.DrainEnergy(repairEnergyCost);
                repairHeat = repairCooldown;
                repairRenderer.SetPosition(1, repairTarget.transform.position);
                repairRenderer.enabled = true;
                CancelInvoke();
                Invoke("ShutoffRepairRenderer", repairRenderTime);
            }
        }
        else if (repairHeat > 0)
        {
            repairHeat -= dt;
        }
    }

    private void ShutoffRepairRenderer()
    {
        repairRenderer.enabled = false;
    }

}
