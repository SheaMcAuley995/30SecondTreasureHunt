using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnWave : MonoBehaviour {

    public GameObject enemyPrefab;
    
    public Transform spawnPoint;
    public float spawnDist;


    public float timeBetweenWaves = 30f;
    private float countDown = 30f;

    public Text waveCountdownText;

    public int EnemiesPerWave = 2;
    private int waveIndex = 0;

    public bool overTime;

    private void Start()
    {
        countDown = timeBetweenWaves;
    }

    private void Update()
    {
        if(countDown <= 0f)
        {
            StartCoroutine(SpawnNewWave());
            countDown = timeBetweenWaves;
        }
        countDown -= Time.deltaTime;

        waveCountdownText.text = "Next wave in :" + (int)countDown;
        //waveCountdownText.text = Mathf.Round(countDown).ToString();
    }

    IEnumerator SpawnNewWave()
    {
        waveIndex += EnemiesPerWave;
        transform.eulerAngles = Vector3.up * Random.Range(0.0f, 9001.0f);
        float fanSize = 30.0f;
        float angleIncrement = fanSize / waveIndex;

        for (int i = 0; i < waveIndex; i++)
        {
            Debug.DrawLine(transform.position, spawnPoint.position);
            //transform.rotation = new  (transform.rotation.x, transform.rotation.y + 4,transform.rotation.z)
            transform.eulerAngles += Vector3.up * angleIncrement;
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = transform.position + (transform.forward * (BaseManager.Instance.BaseEdgeDist + spawnDist));
            EnemyMoter script = enemy.GetComponent<EnemyMoter>();
            EnemyManager.Instance.enemies.Add(script);
            //SpawnEnemy();
            //yield return new WaitForSeconds(0.1f);
        }
        yield return null;

        //waveIndex += EnemiesPerWave;
        //Instantiate(spawnPoint);
        //spawnPoint.position = (Random.insideUnitCircle).normalized * (BaseManager.Instance.BaseEdgeDist + spawnDist);
        //spawnPoint.position = new Vector3(spawnPoint.position.x, 0, spawnPoint.position.y);
        //spawnPoint.transform.parent = this.transform;
        //for (int i = 0; i < waveIndex; i++)
        //{
        //    Debug.DrawLine(transform.position, spawnPoint.position);
        //    //transform.rotation = new  (transform.rotation.x, transform.rotation.y + 4,transform.rotation.z)
        //    transform.eulerAngles += Vector3.up * 4.0f;
        //    SpawnEnemy();
        //    //yield return new WaitForSeconds(0.1f);
        //    yield return null;
        //}

    }

    void SpawnEnemy()
    {
        
        //Vector3 Spawnpos = (Random.insideUnitCircle).normalized * 5;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position,  spawnPoint.transform.rotation);
    //    enemy.transform.position = spawnPoint.position;
    }
    


}
