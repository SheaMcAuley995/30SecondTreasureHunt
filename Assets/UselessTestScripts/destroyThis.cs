using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyThis : MonoBehaviour {

	void Update () {
        Destroy(this.gameObject, 1f);
	}
}
