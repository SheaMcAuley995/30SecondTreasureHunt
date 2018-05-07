using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : MonoBehaviour, Idamagable {

    public float personalSpace;
    public bool isConnector;

    private float health;

	public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

}
