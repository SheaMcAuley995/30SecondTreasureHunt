using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{

    private static BaseManager instance;
    public static BaseManager Instance
    {
        get
        {
            return instance;
        }
    }

    public float BaseEdgeDist
    {
        get
        {
            float ret = core.personalSpace;
            float thisDist;
            foreach(BaseStructure strct in structures)
            {
                thisDist = Vector3.Distance(strct.transform.position, core.transform.position)
                         + strct.personalSpace;
                if(thisDist > ret)
                {
                    ret = thisDist;
                }
            }
            return ret;
        }
    }
    
    public BaseStructure GetCore()
    {
        return core;
    }


    public float connectorReach;
    public BaseStructure core;

    private List<BaseStructure> connectors = new List<BaseStructure>();
    private List<BaseStructure> structures = new List<BaseStructure>();




    private void Awake()
    {
        instance = this;
        connectors.Add(core);
        structures.Add(core);
    }
    
    public BaseStructure GetClosestStructure(Vector3 pos)
    {
        BaseStructure ret = null;
        float minDist = float.MaxValue;
        float thisDist;

        foreach (BaseStructure strct in structures)
        {
            thisDist = Vector3.Distance(strct.transform.position, pos);
            if (thisDist < minDist)
            {
                ret = strct;
                minDist = thisDist;
            }
        }

        return ret;
    }

    public BaseStructure GetClosestConnector(Vector3 pos)
    {
        BaseStructure ret = null;
        float minDist = float.MaxValue;
        float thisDist;

        foreach (BaseStructure strct in connectors)
        {
            thisDist = Vector3.Distance(strct.transform.position, pos);
            if (thisDist < minDist)
            {
                ret = strct;
                minDist = thisDist;
            }
        }

        return ret;
    }

    public bool CanPlaceStructure(Transform obj)
    {
        BaseStructure closest = GetClosestStructure(obj.position);
        if(Vector3.Distance(obj.position, closest.transform.position) <= closest.personalSpace)
        {
            return false;
        }

        closest = GetClosestConnector(obj.position);
        if (Vector3.Distance(obj.position, closest.transform.position) >= connectorReach)
        {
            return false;
        }

        return true;
    }

    public void BuildStructure(GameObject prefab, Vector3 pos)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.position = pos;
        BaseStructure script = obj.GetComponent<BaseStructure>();

        MakeConnections(script, pos);

        structures.Add(script);

        if (script.isConnector)
        {
            connectors.Add(script);
        }
    }

    public void MakeConnections(BaseStructure structure, Vector3 pos)
    {
        if(structure.isConnector)
        {
            foreach (BaseStructure strct in structures)
            {
                if (Vector3.Distance(pos, strct.transform.position) < connectorReach)
                {
                    structure.ConnectToStructure(strct.transform.position);
                }
            }
        }
        else
        {
            foreach (BaseStructure strct in connectors)
            {
                if (Vector3.Distance(pos, strct.transform.position) < connectorReach)
                {
                    strct.ConnectToStructure(pos);
                }
            }
        }
    }

}
