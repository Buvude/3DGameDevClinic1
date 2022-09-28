using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureOne : MonoBehaviour
{
    public float breakForce = 100;
    public GameObject fracturedObject;

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.impulse.magnitude > breakForce)
        {
            fracturedObject.SetActive(true);
            fracturedObject.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
