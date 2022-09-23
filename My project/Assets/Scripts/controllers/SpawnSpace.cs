using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpace : MonoBehaviour
{
    public const int RESPAWN_TIMER = 5;
    public Transform spawn;
    public EnemyController EnemyAI;
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
        EnemyAI.self.SetActive(false);
        yield return new WaitForSeconds(RESPAWN_TIMER);
        EnemyAI.self.transform.position.Set(spawn.position.x, spawn.position.y, spawn.position.z);
        EnemyAI.self.SetActive(true);
        EnemyAI.postRespawn();
    }
}
