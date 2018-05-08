using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class GizmosManager : MonoBehaviour {

    private class CircleGizmo : MonoBehaviour
    {
        [SerializeField]
        private Transform C_transform;
        [SerializeField]
        private float radius;
    }

    [Header("Circle Gizmo")]

    [SerializeField]
    List<CircleGizmo> c_gizmo;
    
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
