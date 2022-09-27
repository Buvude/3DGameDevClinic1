using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpace : MonoBehaviour
{
    public const int RESPAWN_TIMER = 5;
    public Transform spawn;
    public EnemyController EnemyAI;
    public GameObject DeathSoundObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator deadRespawn()
    {
        //instantiate a sound object to play a raptor death sound
        GameObject.Instantiate(DeathSoundObject, EnemyAI.transform.position,Quaternion.identity);

        EnemyAI.self.SetActive(false);
        
        yield return new WaitForSeconds(RESPAWN_TIMER);
        EnemyAI.self.SetActive(true);
        EnemyAI.gameObject.transform.position = spawn.position;
        EnemyAI.gameObject.GetComponent<RaptorController>().raptorRespawn();
        
        EnemyAI.postRespawn();
    }
}
