using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STEAKSCRIPT : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        print(collision.gameObject.name);
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
