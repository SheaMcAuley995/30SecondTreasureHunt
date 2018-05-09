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
        Instantiate(spawnPoint);
        spawnPoint.position = (Random.insideUnitCircle).normalized * (BaseManager.Instance.BaseEdgeDist + spawnDist);
        spawnPoint.position = new Vector3(spawnPoint.position.x, 0, spawnPoint.position.y);
        for (int i = 0; i < waveIndex; i++)
        {
            Debug.DrawLine(transform.position, spawnPoint.position);
            transform.rotation = new Quaternion.  (transform.rotation.x, transform.rotation.y + 4,transform.rotation.z)
            SpawnEnemy();
            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }

    }

    void SpawnEnemy()
    {
        
        //Vector3 Spawnpos = (Random.insideUnitCircle).normalized * 5;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position,  spawnPoint.transform.rotation);
    //    enemy.transform.position = spawnPoint.position;
        EnemyMoter script = enemy.GetComponent<EnemyMoter>();

        EnemyManager.Instance.enemies.Add(script);
    }
    


}
