using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour {

    public float moveSpeed;
    public float scrollSpeed;

    public float maxHeight;
    public float minHeight;

    public GameObject buildPlane;
    public GameObject[] buildingPrefabs;
    private int prefabIdx = 0;

    [Header("Circle Drawers")]
    public Transform circleDrawers;
    public CircleDrawer connectRange;
    public CircleDrawer connectToConnectorRange;
    public CircleDrawer shootRange;

    private Vector3 move;

    private BaseStructure ghostStructure;



    private void Awake()
    {
        ghostStructure = Instantiate(buildingPrefabs[prefabIdx]).GetComponent<BaseStructure>();
    }

    private void Start()
    {
        UIManager.Instance.SetNameText(ghostStructure.structureName);
        UIManager.Instance.SetCostText((int)ghostStructure.energyCost);
        ConfigureCircleDrawrers();
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



        if(Input.GetKeyDown(KeyCode.Q))
        {
            --prefabIdx;
            if(prefabIdx < 0)
            {
                prefabIdx = buildingPrefabs.Length - 1;
            }
            SetGhostStructure();
            ConfigureCircleDrawrers();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            ++prefabIdx;
            if(prefabIdx >= buildingPrefabs.Length)
            {
                prefabIdx = 0;
            }
            SetGhostStructure();
            ConfigureCircleDrawrers();
        }

        buildPlane.transform.position = transform.position - (Vector3.up * transform.position.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000.0f, LayerMask.GetMask("BuildPlane")))
        {
            ghostStructure.transform.position = hit.point;
            circleDrawers.position = hit.point;
            if (BaseManager.Instance.CanPlaceStructure(ghostStructure.transform))
            {
                ghostStructure.gameObject.SetActive(true);
            }
            else
            {
                ghostStructure.gameObject.SetActive(false);
            }
            ConfigureCircleDrawrers();
        }

        if(Input.GetMouseButtonDown(0) && ghostStructure.gameObject.activeInHierarchy)
        {
            BaseManager.Instance.BuildStructure(buildingPrefabs[prefabIdx], ghostStructure.transform.position);
        }
    }

    public void SetGhostStructure()
    {
        Destroy(ghostStructure.gameObject);
        ghostStructure = Instantiate(buildingPrefabs[prefabIdx]).GetComponent<BaseStructure>();
        UIManager.Instance.SetNameText(ghostStructure.structureName);
        UIManager.Instance.SetCostText((int)ghostStructure.energyCost);
    }

    public void ConfigureCircleDrawrers()
    {
        if(!ghostStructure.gameObject.activeInHierarchy || !ghostStructure.isConnector)
        {
            connectRange.gameObject.SetActive(false);
        }
        else
        {
            connectRange.xradius = BaseManager.Instance.connectorReach;
            connectRange.yradius = BaseManager.Instance.connectorReach;
            connectRange.CreatePoints();
            connectRange.gameObject.SetActive(true);
        }

        if (!ghostStructure.gameObject.activeInHierarchy || ghostStructure.isConnector)
        {
            connectToConnectorRange.gameObject.SetActive(false);
        }
        else
        {
            connectToConnectorRange.xradius = BaseManager.Instance.connectorReach;
            connectToConnectorRange.yradius = BaseManager.Instance.connectorReach;
            connectToConnectorRange.CreatePoints();
            connectToConnectorRange.gameObject.SetActive(true);
        }

        if (!ghostStructure.gameObject.activeInHierarchy || !ghostStructure.isGun)
        {
            shootRange.gameObject.SetActive(false);
        }
        else
        {
            shootRange.xradius = ghostStructure.range;
            shootRange.yradius = ghostStructure.range;
            shootRange.CreatePoints();
            shootRange.gameObject.SetActive(true);
        }
    }

}
