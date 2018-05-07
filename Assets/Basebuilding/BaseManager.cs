using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{

    private static BaseStructure coreStatic;
    public static BaseStructure GetCore()
    {
        return coreStatic;
    }

    private static List<BaseStructure> structures = new List<BaseStructure>();

    public static BaseStructure GetClosestStructure(Vector3 pos)
    {
        BaseStructure ret = null;
        float minDist = float.MaxValue;
        float thisDist;

        foreach(BaseStructure strct in structures)
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


    public BaseStructure core;




    private void Awake()
    {
        coreStatic = core;
    }


}
