using UnityEngine;

public class CollisionRelay : MonoBehaviour
{
    public CollisionAudio.CollisionType objectCollisionType;
    private CollisionAudio collisionAudio;

    void Start()
    {
        collisionAudio = GetComponentInChildren<CollisionAudio>();
        if (collisionAudio == null)
        {
            Debug.LogError("CollisionAudio component not found in children.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CollisionRelay otherRelay = collision.gameObject.GetComponent<CollisionRelay>();

        if (otherRelay != null && collisionAudio != null)
        {
            Debug.Log($"Collision detected between {gameObject.name} and {collision.gameObject.name}");
            collisionAudio.HandleCollision(collision, otherRelay.objectCollisionType);
        }
    }
}