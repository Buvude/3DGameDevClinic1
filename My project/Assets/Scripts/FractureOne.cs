using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureOne : MonoBehaviour
{
    public float breakForce = 100;
    public GameObject fracturedObject;

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag=="Meat"|| collision.gameObject.tag =="Enemy")
        {
            fracturedObject.SetActive(true);
            fracturedObject.transform.parent = null;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        fracturedObject.SetActive(true);
        fracturedObject.transform.parent = null;
        Destroy(gameObject);
    }
}
