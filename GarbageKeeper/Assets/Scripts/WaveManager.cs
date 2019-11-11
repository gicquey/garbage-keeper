using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Ennemi> AliveEnnemies;
    public Wave[] waves;

    public Transform spawn;

    public float timeBetweenWaves;
    private float countdown;

    private int waveIndex;

    private static WaveManager _instance = null;
    public static WaveManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        AliveEnnemies = new List<Ennemi>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
    }

    IEnumerator SpawnWave()
    {
        if (waveIndex >= waves.Length)
        {
            var lastWave = waves[waves.Length - 1];
            lastWave.waveContent.AddRange(lastWave.waveContent);
            waveIndex = waves.Length - 1;
        }
        Wave wave = waves[waveIndex];


        for (int i = 0; i < wave.count; i++)
        {

            SpawnEnemy(wave.waveContent[i]);
            yield return new WaitForSeconds(wave.timeBetweenSpawn);
        }

        waveIndex++;
    }

    private void SpawnEnemy(Ennemi e)
    {
        AliveEnnemies.Add(Instantiate(e));
    }

    void Update()
    {
        if (AliveEnnemies.Count > 0)
        {
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }


    public void RemoveEnnemy(Ennemi ennemy)
    {
        WaveManager.Instance.AliveEnnemies.Remove(ennemy);
    }
}
