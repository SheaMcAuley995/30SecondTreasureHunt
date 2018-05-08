using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour {

    public int starCount;
    public float fieldSize;
    public float starMinHeight;
    public float starMaxHeight;
    public float starMaxSize;
    public float starMinSize;
    public GameObject starPrefab;
    public Transform camTrans;

    private List<Transform> stars = new List<Transform>();


    private void Start()
    {
        transform.position = Vector3.zero;

        stars.Capacity = starCount;
        for(int i = 0; i < starCount; ++i)
        {
            GameObject star = Instantiate(starPrefab);
            star.transform.parent = transform;
            star.transform.position = new Vector3(Random.Range(-fieldSize, fieldSize),
                                                  Random.Range(starMinHeight, starMaxHeight),
                                                  Random.Range(-fieldSize, fieldSize));
            float starSize = Random.Range(starMinSize, starMaxSize);
            star.transform.localScale = new Vector3(starSize, starSize, starSize);
            stars.Add(star.transform);
        }
    }

    private void Update()
    {
        transform.position = new Vector3(camTrans.position.x, 0, camTrans.position.z);
    }

}
