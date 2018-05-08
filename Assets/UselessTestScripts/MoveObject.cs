using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

	void Update () {
        transform.Translate(Vector3.forward);
	}
}
