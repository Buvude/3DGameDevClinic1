using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawn : MonoBehaviour
{
    int despawnTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DespawnBullet()
	{
        yield return new WaitForSeconds(despawnTime);
		Destroy(gameObject);
    }
}
