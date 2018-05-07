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
    
    public BaseStructure GetCore()
    {
        return core;
    }


    public BaseStructure core;

    private List<BaseStructure> connectors = new List<BaseStructure>();

    private List<BaseStructure> structures = new List<BaseStructure>();




    private void Awake()
    {
        instance = this;
        connectors.Add(core);
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

}
