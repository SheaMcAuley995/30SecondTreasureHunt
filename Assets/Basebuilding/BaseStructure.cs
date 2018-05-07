using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : MonoBehaviour, Idamagable {

    private float health;

	public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

}
