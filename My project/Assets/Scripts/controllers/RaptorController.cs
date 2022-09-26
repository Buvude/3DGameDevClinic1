using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorController : MonoBehaviour
{
    public float raptorHealth;// hits it takes for the raptor to die
    public AudioClip hitSound; // sound played when raptor takes damage
    public AudioSource raptorAudio; // source for sound
   

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Meat"))
        {
            
            raptorAudio.clip = hitSound;
            raptorAudio.PlayOneShot(hitSound, 1.0f);
            raptorHealth--;

            if (raptorHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
