using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEditor.Animations;

public class SpawnStackLocation : MonoBehaviour
{
    public int spawnedStackOrigins;

    [SerializeField] private List <GameObject> spawnedStacks; //all spawns, also nonvalid stacks
    [SerializeField] private List<GameObject> validStacks;

    [SerializeField] private FindSpawnPositions spawnPositions;
    [SerializeField] private AnchorPrefabSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnStackOrigin()
    {
        //populate the scene with possible spawn locations
        spawnPositions.StartSpawn();
        Debug.Log("spawned");

        GetPossibleStackOrigins();
    }

    private void GetPossibleStackOrigins()
    {
        // after spawning, create a list of possible spawn locations
        spawnedStacks = new List<GameObject>();

        foreach (Transform child in spawnPositions.transform)
        {
            spawnedStacks.Add(child.gameObject);
        }

        Debug.Log("Number of children found: " + spawnedStacks.Count);

    }


    public void OnSpawnSceneColliders()
    {
        //the scene is initialized, and the colliders on the furniture are spawned.
        //now we should Spawn Stack origins
        SpawnStackOrigin();
    }
}
