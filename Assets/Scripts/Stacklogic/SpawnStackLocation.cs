using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SpawnStackLocation : MonoBehaviour
{
    public int spawnedStackOrigins;

    [SerializeField] private FindSpawnPositions spawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void SpawnStackOrigin()
    {
        spawnPositions.StartSpawn();
    }
}
