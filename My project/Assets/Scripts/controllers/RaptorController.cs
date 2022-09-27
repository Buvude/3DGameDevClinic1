using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorController : MonoBehaviour
{
    public float raptorHealth;// hits it takes for the raptor to die
    public float curHealth;
    public AudioClip hitSound; // sound played when raptor takes damage
    public AudioClip deathSound; // sound played when raptor takes damage
    public AudioSource raptorAudio; // source for sound
    public bool isDead;

    private void Start()
    {
        curHealth = raptorHealth;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Meat") && isDead == false)
        {

            Destroy(other.gameObject);
            raptorAudio.clip = hitSound;
            curHealth--;
            if(curHealth >= 1)
			{
                raptorAudio.Play();
            }

            if (curHealth <= 0)
            {
                gameObject.GetComponent<EnemyController>().raptorDied();
                isDead = true;
                raptorAudio.clip = deathSound;
                raptorAudio.Play();
                transform.rotation = Quaternion.Euler(new Vector3 (transform.rotation.x, transform.rotation.y + 90, transform.rotation.z));
            }
        }
    }

   public void raptorRespawn()
    {
        curHealth = raptorHealth;
        isDead = false;
    }


}
