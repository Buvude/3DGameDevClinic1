using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFracture : MonoBehaviour
{
   

    public Transform[] allChildren;

    [Header("Launch parameters")]
    public float launchpower;
    public float maxPower;
    public float minPower;

   

    private void Awake()
    {

        selfLaunch();
    }
   

    public void lauchPieces()
    {
        print(4);
        //get launch angle
        Vector3 LaunchAngle;

        float power;
        //calculate launch power

        foreach (Transform g in allChildren){
            LaunchAngle = g.transform.position - transform.position;
           
            power = Random.Range(minPower, maxPower);

            LaunchAngle = power * LaunchAngle;
            g.GetComponent<Rigidbody>().AddForce(LaunchAngle, ForceMode.Impulse);
        }
    } 

    public void selfLaunch()
    {
        
        Vector3 LaunchAngle = transform.position - transform.parent.transform.position;

        

        LaunchAngle = LaunchAngle.normalized;

        float power = Random.Range(0, 2.5f);

        LaunchAngle = power * LaunchAngle;
        transform.GetComponent<Rigidbody>().AddForce(LaunchAngle, ForceMode.VelocityChange);
        

    }


}