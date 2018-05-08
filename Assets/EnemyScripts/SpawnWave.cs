using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnWave : MonoBehaviour {

    public GameObject enemyPrefab;
    
    public Transform spawnPoint;
    public float spawnDist;


    public float timeBetweenWaves = 3f;
    private float countDown;

    //public Text waveCountdownText;

    private int waveIndex = 0;

    private void Update()
    {
        if(countDown <= 0f)
        {
            StartCoroutine(SpawnNewWave());
            countDown = timeBetweenWaves;
        }
        countDown -= Time.deltaTime;

        //waveCountdownText.text = Mathf.Round(countDown).ToString();
    }

    IEnumerator SpawnNewWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            Instantiate(spawnPoint);
            spawnPoint.position = (Random.insideUnitCircle).normalized * (BaseManager.Instance.BaseEdgeDist + spawnDist);
            spawnPoint.position = new Vector3(spawnPoint.position.x, 0, spawnPoint.position.y);
            Debug.DrawLine(transform.position, spawnPoint.position);
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

    }

    void SpawnEnemy()
    {
       
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    //    enemy.transform.position = spawnPoint.position;
        EnemyMoter script = enemy.GetComponent<EnemyMoter>();

        EnemyManager.Instance.enemies.Add(script);
    }



}
