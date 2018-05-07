using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour {

    public float moveSpeed;
    public float scrollSpeed;

    public float maxHeight;
    public float minHeight;

    public GameObject buildPlane;
    public GameObject prefabToBuild;

    private Vector3 move;

    private BaseStructure ghostStructure;



    private void Awake()
    {
        ghostStructure = Instantiate(prefabToBuild).GetComponent<BaseStructure>();
    }

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

        buildPlane.transform.position = transform.position - (Vector3.up * transform.position.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000.0f, LayerMask.GetMask("BuildPlane")))
        {
            ghostStructure.transform.position = hit.point;
            if(BaseManager.Instance.CanPlaceStructure(ghostStructure.transform))
            {
                ghostStructure.gameObject.SetActive(true);
            }
            else
            {
                ghostStructure.gameObject.SetActive(false);
            }
        }

        if(Input.GetMouseButtonDown(0) && ghostStructure.gameObject.activeInHierarchy)
        {
            BaseManager.Instance.BuildStructure(prefabToBuild, ghostStructure.transform.position);
        }
    }

}
