using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectCollision
    : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, 5f);  Wenn ohne // wird gameObject nach 5 sekunden zerstört

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

   
    }
 

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Duck") // if(also wenn) Kollision mit allen gameObjects die den Tag "Enemy" (oder was du sonst willst) haben, wird gameObject zerstört
        {
            //Destroy(gameObject);
            Destroy(collision.gameObject); // nimmt man "Destroy(collision.gameObject);" anstelle von "Destroy(gameObject);" wird Enemy zerstört. Gut als Bullet Destroy z.B.

        }
    }
}
