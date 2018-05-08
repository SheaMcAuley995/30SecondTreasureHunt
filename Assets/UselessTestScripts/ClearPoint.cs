using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPoint : MonoBehaviour {

	void Update () {
        Destroy(this.gameObject, 3f);
	}
}
