using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnWave : MonoBehaviour {

    public Transform enemyPrefab;

    private Transform spawnPoint;

    public float timeBetweenWaves = 30f;
    private float countDown = 30f;

    public Text waveCountdownText;

    private int waveIndex = 0;

    private void Update()
    {
        if(countDown <= 0f)
        {
            StartCoroutine(SpawnNewWave());
            countDown = timeBetweenWaves;
        }
        countDown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(countDown).ToString();
    }

    IEnumerator SpawnNewWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
