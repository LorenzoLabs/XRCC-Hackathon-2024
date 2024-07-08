using UnityEngine;

public class CollisionRelay : MonoBehaviour
{
    private CollisionAudioManager _manager;
    private CollisionAudioManager.CollisionType _objectCollisionType;

    public void Initialize(CollisionAudioManager manager, CollisionAudioManager.CollisionType objectCollisionType)
    {
        _manager = manager;
        _objectCollisionType = objectCollisionType;
        Debug.Log($"CollisionRelay initialized for {gameObject.name} with collision type {_objectCollisionType}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        string colliderName = gameObject.name;
        string otherObjectName = collision.gameObject.name;

        Debug.Log($"CollisionRelay: OnCollisionEnter triggered for {colliderName} with {otherObjectName}");
        
        var audioManager = FindAudioManager();
        if (audioManager != null)
        {
            Debug.Log($"Collision detected between {colliderName} and {otherObjectName}, relayed to {audioManager.gameObject.name}");
            audioManager.HandleCollision(collision, _objectCollisionType);
        }
        else
        {
            Debug.LogWarning($"No CollisionAudioManager found in hierarchy for {colliderName}");
        }
    }

    private CollisionAudioManager FindAudioManager()
    {
        // Find the "audio" child object that has the CollisionAudioManager component
        var parent = transform.root; // Assuming the "audio" object is within the root of this prefab
        foreach (Transform child in parent)
        {
            if (child.name == "audio")
            {
                var audioManager = child.GetComponent<CollisionAudioManager>();
                if (audioManager != null)
                {
                    return audioManager;
                }
            }
        }
        return null;
    }
}