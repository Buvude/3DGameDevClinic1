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

    private void Start()
    {

        print(1);
    }

    private void Awake()
    {

        selfLaunch();
    }
    private void OnEnable()
    {
        print(3);
      
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
            print("PWOOOSH");
            power = Random.Range(minPower, maxPower);

            LaunchAngle = power * LaunchAngle;
            g.GetComponent<Rigidbody>().AddForce(LaunchAngle, ForceMode.Impulse);
        }
    } 

    public void selfLaunch()
    {
        
        Vector3 LaunchAngle = transform.position - transform.parent.transform.position;

        print("PWOOOSH");

        LaunchAngle = LaunchAngle.normalized;

        float power = Random.Range(0, 2.5f);

        LaunchAngle = power * LaunchAngle;
        transform.GetComponent<Rigidbody>().AddForce(LaunchAngle, ForceMode.VelocityChange);
        

    }


}