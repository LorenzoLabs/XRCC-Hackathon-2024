using Meta.XR.MRUtilityKit;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkedObjectsManager : MonoBehaviour
{
    [SerializeField] private Transform _networkedObjectPool;
    [SerializeField] private Transform _stackOrigintPool;
    [SerializeField] private int _minNumberOfNetworkedObjects;

    // Control variable to ensure spawning only happens once.
    private bool _isSpawned = false;

    void Awake()
    {
        foreach (Transform networkedObject in _networkedObjectPool.transform)
        {
            networkedObject.gameObject.SetActive(false);
        }

        foreach (Transform stackOrigin in _stackOrigintPool.transform)
        {
            stackOrigin.gameObject.SetActive(false);
        }
    }

    public void SpawnNetworkedObjects()
    {
        if (_isSpawned) return;
        _isSpawned = true;

        int numberOfNetworkedObjectsToSpawn = _networkedObjectPool.childCount; /* all */ //Random.Range(Math.Max(_minNumberOfNetworkedObjects, _networkedObjectPool.childCount), _networkedObjectPool.childCount);
        foreach (Transform networkedObject in _networkedObjectPool.transform)
        // for (int i = 0; i < numberOfNetworkedObjectsToSpawn; ++i)
        {
            // Transform networkedObject = _networkedObjectPool.GetChild(i);
            // Tuple<Vector3, Quaternion> validObjectSpawnPoint = GetValidObjectSpawnPoint(networkedObject.gameObject);
            // Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            // networkedObject.SetPositionAndRotation(validObjectSpawnPoint.Item1, randomRotation * validObjectSpawnPoint.Item2);
            networkedObject.gameObject.SetActive(true);
        }

        // foreach (Transform stackOrigin in _stackOrigintPool.transform)
        // {
        //    Tuple<Vector3, Quaternion> validStackSpawnPoint = GetValidStackOriginSpawnPoint(stackOrigin.gameObject);
        //
        //      stackOrigin.SetPositionAndRotation(validStackSpawnPoint.Item1, validStackSpawnPoint.Item2);
        //     stackOrigin.gameObject.SetActive(true);
        // }

    }

    private Tuple<Vector3, Quaternion> GetValidObjectSpawnPoint(GameObject prefab)
    {
        Tuple<Vector3, Quaternion>[] validPositions = GetSpawnPositions(
            objectBounds: Utilities.GetPrefabBounds(prefab),
            positionCount: 1,
            spawnLocation: FindSpawnPositions.SpawnLocation.HangingDown,
            labels: MRUKAnchor.SceneLabels.CEILING);
        Debug.Assert(validPositions.Length > 0, $"[{nameof(NetworkedObjectsManager)}] {nameof(GetValidObjectSpawnPoint)} error: invalid {nameof(validPositions)} array.");
        return validPositions[0];
    }

    private Tuple<Vector3, Quaternion> GetValidStackOriginSpawnPoint(GameObject prefab)
    {
        Tuple<Vector3, Quaternion>[] validPositions = GetSpawnPositions(
            objectBounds: Utilities.GetPrefabBounds(prefab),
            positionCount: 1,
            spawnLocation: FindSpawnPositions.SpawnLocation.OnTopOfSurfaces,
            labels: MRUKAnchor.SceneLabels.FLOOR);
        Debug.Assert(validPositions.Length > 0, $"[{nameof(NetworkedObjectsManager)}] {nameof(GetValidObjectSpawnPoint)} error: invalid {nameof(validPositions)} array.");
        return validPositions[0];
    }

    private static Tuple<Vector3, Quaternion>[] GetSpawnPositions(
        Bounds? objectBounds,
        int positionCount = 8,
        FindSpawnPositions.SpawnLocation spawnLocation = FindSpawnPositions.SpawnLocation.Floating,
        MRUKAnchor.SceneLabels labels = ~(MRUKAnchor.SceneLabels)0,
        bool checkOverlaps = true,
        float overrideBounds = -1f,
        int layerMask = ~0,
        float surfaceClearanceDistance = 0.1f,
        int maxIterations = 1000)
    {
        Tuple<Vector3, Quaternion>[] spawnPositions = new Tuple<Vector3, Quaternion>[positionCount];

        var room = MRUK.Instance.GetCurrentRoom();
        float minRadius = 0.0f;
        const float clearanceDistance = 0.01f;
        float baseOffset = -objectBounds?.min.y ?? 0.0f;
        float centerOffset = objectBounds?.center.y ?? 0.0f;
        Bounds adjustedBounds = new();

        if (objectBounds.HasValue)
        {
            minRadius = Mathf.Min(-objectBounds.Value.min.x, -objectBounds.Value.min.z, objectBounds.Value.max.x, objectBounds.Value.max.z);
            if (minRadius < 0f)
            {
                minRadius = 0f;
            }
            var min = objectBounds.Value.min;
            var max = objectBounds.Value.max;
            min.y += clearanceDistance;
            if (max.y < min.y)
            {
                max.y = min.y;
            }
            adjustedBounds.SetMinMax(min, max);
            if (overrideBounds > 0)
            {
                Vector3 center = new Vector3(0f, clearanceDistance, 0f);
                Vector3 size = new Vector3(overrideBounds * 2f, clearanceDistance * 2f, overrideBounds * 2f); // OverrideBounds represents the extents, not the size
                adjustedBounds = new Bounds(center, size);
            }
        }

        for (int i = 0; i < positionCount; ++i)
        {
            for (int j = 0; j < maxIterations; ++j)
            {
                Vector3 spawnPosition = Vector3.zero;
                Vector3 spawnNormal = Vector3.zero;
                if (spawnLocation == FindSpawnPositions.SpawnLocation.Floating)
                {
                    var randomPos = room.GenerateRandomPositionInRoom(minRadius, true);
                    if (!randomPos.HasValue)
                    {
                        break;
                    }

                    spawnPosition = randomPos.Value;
                }
                else
                {
                    MRUK.SurfaceType surfaceType = 0;
                    switch (spawnLocation)
                    {
                        case FindSpawnPositions.SpawnLocation.AnySurface:
                            surfaceType |= MRUK.SurfaceType.FACING_UP;
                            surfaceType |= MRUK.SurfaceType.VERTICAL;
                            surfaceType |= MRUK.SurfaceType.FACING_DOWN;
                            break;
                        case FindSpawnPositions.SpawnLocation.VerticalSurfaces:
                            surfaceType |= MRUK.SurfaceType.VERTICAL;
                            break;
                        case FindSpawnPositions.SpawnLocation.OnTopOfSurfaces:
                            surfaceType |= MRUK.SurfaceType.FACING_UP;
                            break;
                        case FindSpawnPositions.SpawnLocation.HangingDown:
                            surfaceType |= MRUK.SurfaceType.FACING_DOWN;
                            break;
                    }
                    if (room.GenerateRandomPositionOnSurface(surfaceType, minRadius, LabelFilter.Included(labels), out var pos, out var normal))
                    {
                        spawnPosition = pos + normal * baseOffset;
                        spawnNormal = normal;
                        var center = spawnPosition + normal * centerOffset;
                        // In some cases, surfaces may protrude through walls and end up outside the room
                        // check to make sure the center of the prefab will spawn inside the room
                        if (!room.IsPositionInRoom(center))
                        {
                            continue;
                        }
                        // Ensure the center of the prefab will not spawn inside a scene volume
                        if (room.IsPositionInSceneVolume(center))
                        {
                            continue;
                        }
                        // Also make sure there is nothing close to the surface that would obstruct it
                        if (room.Raycast(new Ray(pos, normal), surfaceClearanceDistance, out _))
                        {
                            continue;
                        }
                    }
                }

                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnNormal);
                if (checkOverlaps && objectBounds.HasValue)
                {
                    if (Physics.CheckBox(spawnPosition + spawnRotation * adjustedBounds.center, adjustedBounds.extents, spawnRotation, layerMask, QueryTriggerInteraction.Ignore))
                    {
                        continue;
                    }
                }

                spawnPositions[i] = new(spawnPosition, spawnRotation);
                break;
            }
        }
        return spawnPositions;
    }
}
