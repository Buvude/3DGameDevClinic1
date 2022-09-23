using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileForward : MonoBehaviour
{
    public float speed = 40;
    PlayerController playerMovement;
    Transform playerTransform;
    Quaternion playerRot;
    GameObject player;
    public GameObject projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        playerRot = Quaternion.Euler(0, playerMovement.rot.y + 90, 0);
        if(Input.GetKeyDown(KeyCode.Mouse0))
		{
            //InstantiateProjectile();
		}
    }

	private void InstantiateProjectile()
	{
        Object.Instantiate(projectile, player.gameObject.transform.position, playerMovement.cameraTransform.rotation);
	}
}
