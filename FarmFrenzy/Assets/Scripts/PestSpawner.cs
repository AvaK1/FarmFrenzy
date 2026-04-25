using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PestSpawner : MonoBehaviour
{
    [SerializeField] private int numOfPestTypesPerWave = 3;
    [SerializeField] private List<GameObject> pestPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private float startingSpawnInterval;
    [SerializeField] private int startingSpawnCount;
    [SerializeField] private float timeBetweenIntervalDecrease;
    [SerializeField] private float countIncrease;
    private float currentSpawnInterval;
    private int currentPestIndex = 0;
    private float currentPestCount;

    private int changeCounter = 0; //counter will be added to each time enemies spawn, and once it reaches a certain point, it will trigger the increase of enemies, decrease of time between intervals, and change the pests that are spawning
    [SerializeField] private int changeNumber = 8; //when the counter gets to this number, it will trigger the changes

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //start repeating coroutine that spawns pests
        currentPestCount = startingSpawnCount;
        currentSpawnInterval = startingSpawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < currentPestCount; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Count - 1);
            int pestPrefabIndex = Random.Range(currentPestIndex, currentPestIndex + numOfPestTypesPerWave);
            Instantiate(pestPrefabs[pestPrefabIndex], spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
        }

        //check if more pests/shorter intervals should be happening
        changeCounter++;
        if (changeCounter >= changeNumber)
        {
            changeCounter = 0;
            currentSpawnInterval -= timeBetweenIntervalDecrease;
            currentPestCount += countIncrease;
            if (currentPestIndex < pestPrefabs.Count - numOfPestTypesPerWave)
            {
                currentPestIndex++;
            }
        }

        yield return new WaitForSeconds(currentSpawnInterval);
        StartCoroutine(SpawnEnemies());
    }
}
