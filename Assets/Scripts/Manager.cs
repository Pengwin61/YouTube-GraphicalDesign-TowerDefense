using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Loader<Manager>
{

    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private GameObject[] enemies;

    [SerializeField] private int maxEnemiesOnScreen;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesPerSpawn;

    [SerializeField] private int enemiesOnScreen = 0;
    private const float spawnDelay = 0.5f;


    private void Start()
    {
        StartCoroutine(Spawn());
    }


    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (enemiesOnScreen < maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    enemiesOnScreen += 1;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void removeEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }

}
