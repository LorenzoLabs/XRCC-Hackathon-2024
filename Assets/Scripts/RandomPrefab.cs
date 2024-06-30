using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPrefab : MonoBehaviour
{
    public GameObject[] prefabs;
    public float lifeTime;

    private void Awake()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject prefab = prefabs[randomIndex];
        Vector3 position = transform.position;
        GameObject instantiatedPrefab = Instantiate(prefab, position, Quaternion.identity);
        Destroy(instantiatedPrefab, lifeTime);
    }
}
