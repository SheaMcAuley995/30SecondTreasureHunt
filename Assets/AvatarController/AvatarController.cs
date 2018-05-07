using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour {

    public float moveSpeed;
    public float scrollSpeed;

    public float maxHeight;
    public float minHeight;

    private Vector3 move;
	


	void Update () {
        move.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        move.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        move.y = -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        transform.position += move;
        if(transform.position.y < minHeight)
        {
            transform.position += Vector3.up * (minHeight - transform.position.y);
        }
        else if(transform.position.y > maxHeight)
        {
            transform.position -= Vector3.up * (transform.position.y - maxHeight);
        }
	}

}
