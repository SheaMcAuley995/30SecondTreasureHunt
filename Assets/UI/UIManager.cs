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

    public Text energyText;




    private void Awake()
    {
        instance = this;
    }

    public void SetEnergyText(int energy)
    {
        energyText.text = "ENERGY: " + energy;
    }

}
