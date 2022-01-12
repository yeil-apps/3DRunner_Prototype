using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner: MonoBehaviour
{
    // генерация окружения с указанным шансом

    [SerializeField] private List<GameObject> _gemsGameObjects;
    [SerializeField] private int _chanceToSpawnGem = 50;

    [SerializeField] private List<GameObject> _enemys;
    [SerializeField] private int _chanceToSpawnEnemy = 80;

    void Start()
    {
        SpawnListWithChance(_enemys, _chanceToSpawnEnemy);
        SpawnListWithChance(_gemsGameObjects, _chanceToSpawnGem);
    }

    private void SpawnListWithChance(List<GameObject> spawnList, int chance)
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            float rndChance = Random.Range(0, 101);
            if (rndChance < chance)
                spawnList[i].SetActive(true);
            else
                spawnList[i].SetActive(false);
        }
    }
}
