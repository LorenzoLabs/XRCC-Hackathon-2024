using UnityEngine;

/// <summary>
/// Destroys object after a few seconds
/// </summary>
public class DestroyObject : MonoBehaviour
{
    

    //"Time before destroying in seconds"
    public float lifeTime = 5.0f;

    private void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }
}
