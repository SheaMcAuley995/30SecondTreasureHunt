using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Text strctNameText;
    public Text strctCostText;
    public Text energyText;




    private void Awake()
    {
        instance = this;
    }

    public void SetNameText(string name)
    {
        strctNameText.text = "BUILDING TYPE: " + name;
    }

    public void SetCostText(int cost)
    {
        strctCostText.text = "COST: " + cost;
    }

    public void SetEnergyText(int energy, int energyCapacity)
    {
        energyText.text = "ENERGY: " + energy + " / " + energyCapacity;
    }

}
