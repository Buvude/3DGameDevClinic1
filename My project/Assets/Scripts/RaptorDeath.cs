using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorDeath : MonoBehaviour
{
    public float raptorHealth;
    public AudioClip hitSound;
    public AudioSource raptorAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Detect Collisions
        //Raptor Dies
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Meat"))
        {
            //Destroy(other.gameObject);
            print("Hit");
            raptorAudio.clip = hitSound;
            raptorAudio.PlayOneShot(hitSound, 1.0f);
            raptorHealth--;

            if(raptorHealth <= 0)
			{
                Destroy(gameObject); 
            }
        }
    }
}
