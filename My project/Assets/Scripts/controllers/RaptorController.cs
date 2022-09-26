using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorController : MonoBehaviour
{
    public float raptorHealth;// hits it takes for the raptor to die
    public AudioClip hitSound; // sound played when raptor takes damage
    public AudioSource raptorAudio; // source for sound
    public bool isDead;
   

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Meat") && isDead == false)
        {
            
            raptorAudio.clip = hitSound;
            raptorAudio.PlayOneShot(hitSound, 1.0f);
            raptorHealth--;

            if (raptorHealth <= 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3 (transform.rotation.x, transform.rotation.y + 90, transform.rotation.z));
                isDead = true;

            }
        }
    }
}
