using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;
using nickmaltbie.OpenKCC.Character;
using nickmaltbie.OpenKCC.Utils;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    //GroundMovement
    public float ScaleOfCharacter;
    public float maxMoveSpeed;
    public float anglePower = .5f;
    public float maxWalkingAngle;
    // how close to the ground the player is stopped
    private float groundDist = .01f;

    [Tooltip("Action with two axis to rotate player camera around.")]
    public InputActionReference lookAround;

    [Tooltip("Action with two axis used to move the player around.")]
    public InputActionReference movePlayer;

    [Header("Jumping WIP")]
    //Jumping WIP
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Tooltip("Transform holding camera position.")]
    [SerializeField]
    public Transform cameraTransform;

    [Header("Gun Mechanic")]
    public Transform bulletPoint;
    public GameObject bullet;
    public float bulletForce;
    private bool gunReady;

    //Kinematic variables
    internal Vector3 velocity;
    internal Vector3 currentInput;
    internal bool collided;
    private CapsuleCollider capsuleCollider;
    private float maxBounces = 3;
    private Vector2 cameraAngle;
    private float rotateSpeed = 45f;
    private const float minPitch = -90;
    private const float maxPitch = 90;

    public GameObject projectilePrefab;
    public Transform bulletSpawner;
    public Vector3 rot;
    Quaternion steakRotation;

    
   
    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        gunReady = true;
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    public void Update()
    {
        steakRotation = Quaternion.Euler(cameraTransform.localRotation.x, cameraTransform.localRotation.y + 90, cameraTransform.localRotation.z);
        // Read input values from player
        Vector2 cameraMove = lookAround.action.ReadValue<Vector2>();
        Vector2 playerMove = movePlayer.action.ReadValue<Vector2>();
      
        // If player is not allowed to move, stop player input
        if (PlayerInputUtils.playerMovementState == PlayerInputState.Deny)
        {
            playerMove = Vector2.zero;
            cameraAngle = Vector2.zero;
        }

        // Camera move on x (horizontal movement) controls the yaw (look left or look right)
        // Camera move on y (vertical movement) controls the pitch (look up or look down)
        cameraAngle.x += -cameraMove.y * rotateSpeed * Time.deltaTime;
        cameraAngle.y += cameraMove.x * rotateSpeed * Time.deltaTime;

        cameraAngle.x = Mathf.Clamp(cameraAngle.x, minPitch, maxPitch);
        cameraAngle.y %= 360;

        //Rotate player based on mouse input, ensure pitch is bounded to not overshoot
        transform.rotation = Quaternion.Euler(0, cameraAngle.y, 0);
        // Only rotate camera pitch
        cameraTransform.rotation = Quaternion.Euler(cameraAngle.x, cameraAngle.y, 0);
        rot = cameraTransform.localRotation.eulerAngles;

        // Check if the player is falling
        bool falling = !CheckGrounded(out RaycastHit groundHit);

        // If falling, increase falling speed, otherwise stop falling.
        if (falling)
        {
            velocity += Physics.gravity * Time.deltaTime;
        }
        else
        {
            velocity = Vector3.zero;
        }

        // Read player input movement
        var inputVector = new Vector3(playerMove.x, 0, playerMove.y);

        // Rotate movement by current viewing angle
        var viewYaw = Quaternion.Euler(0, cameraAngle.y, 0);
        Vector3 rotatedVector = viewYaw * inputVector;
        Vector3 normalizedInput = rotatedVector.normalized * Mathf.Min(rotatedVector.magnitude, 1.0f);

        // Scale movement by speed and time
        Vector3 movement = normalizedInput * maxMoveSpeed * Time.deltaTime;

        // If the player is standing on the ground, project their movement onto that plane
        // This allows for walking down slopes smoothly.
        if (!falling)
        {
            movement = Vector3.ProjectOnPlane(movement, groundHit.normal);
        }

        // Attempt to move the player based on player movement
        transform.position = KinematicMovePlayer(movement);

        // Move player based on falling speed
        transform.position = KinematicMovePlayer(velocity * Time.deltaTime);
    }


    private bool CheckGrounded(out RaycastHit groundHit)
    {
        bool onGround = CastSelf(transform.position, transform.rotation, Vector3.down, groundDist, out groundHit);
        float angle = Vector3.Angle(groundHit.normal, Vector3.up);
        return onGround && angle < maxWalkingAngle;
    }


    //updated solution to player movement 
    public Vector3 KinematicMovePlayer(Vector3 movement)
    {

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Vector3 remaining = movement;

        int bounces = 0;

        while (bounces < maxBounces && remaining.magnitude > KCCUtils.Epsilon)
        {
            //cast collider checking for any collision during this bounce
            float distance = remaining.magnitude;
            if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit))
            {
                // If there is no hit, move to desired position
                position += remaining;

                // Exit as we are done bouncing
                break;
            }

            // If we are overlapping with something, just exit.
            if (hit.distance == 0)
            {
                break;
            }

            float fraction = hit.distance / distance;
            // Set the fraction of remaining movement (minus some small value)
            position += remaining * (fraction);
            // Push slightly along normal to stop from getting caught in walls
            position += hit.normal * KCCUtils.Epsilon * 2;
            // Decrease remaining movement by fraction of movement remaining
            remaining *= (1 - fraction);

            // Plane to project rest of movement onto
            Vector3 planeNormal = hit.normal;

            // Only apply angular change if hitting something
            // Get angle between surface normal and remaining movement
            float angleBetween = Vector3.Angle(hit.normal, remaining) - 90.0f;

            // Normalize angle between to be between 0 and 1
            // 0 means no angle, 1 means 90 degree angle
            angleBetween = Mathf.Min(KCCUtils.MaxAngleShoveRadians, Mathf.Abs(angleBetween));
            float normalizedAngle = angleBetween / KCCUtils.MaxAngleShoveRadians;

            // Reduce the remaining movement by the remaining movement that ocurred
            remaining *= Mathf.Pow(1 - normalizedAngle, anglePower) * 0.9f + 0.1f;

            // Rotate the remaining movement to be projected along the plane 
            // of the surface hit (emulate pushing against the object)
            Vector3 projected = Vector3.ProjectOnPlane(remaining, planeNormal).normalized * remaining.magnitude;

            // If projected remaining movement is less than original remaining movement (so if the projection broke
            // due to float operations), then change this to just project along the vertical.
            if (projected.magnitude + KCCUtils.Epsilon < remaining.magnitude)
            {
                remaining = Vector3.ProjectOnPlane(remaining, Vector3.up).normalized * remaining.magnitude;
            }
            else
            {
                remaining = projected;
            }

            // Track number of times the character has bounced
            bounces++;
        }
        
        return position;

    }

    // Checks in a direction of self to see if collision is iminent
    public bool CastSelf(Vector3 pos, Quaternion rot, Vector3 dir, float dist, out RaycastHit hit)
    {
        // Get Parameters associated with the KCC
        Vector3 center = rot * capsuleCollider.center + pos;
        float radius = capsuleCollider.radius*ScaleOfCharacter;
        float height = capsuleCollider.height*ScaleOfCharacter;

        // Get top and bottom points of collider
        Vector3 bottom = center + rot * Vector3.down * ((height) / 2 - radius);
        Vector3 top = center + rot * Vector3.up * ((height) / 2 - radius);
       // bottom = bottom*ScaleOfCharacter;
        //top = top * ScaleOfCharacter;

        // Check what objects this collider will hit when cast with this configuration excluding itself
        IEnumerable<RaycastHit> hits = Physics.CapsuleCastAll(
            top, bottom, radius, dir, dist, ~0, QueryTriggerInteraction.Ignore)
            .Where(hit => hit.collider.transform != transform);
        bool didHit = hits.Count() > 0;

        // Find the closest objects hit
        float closestDist = didHit ? Enumerable.Min(hits.Select(hit => hit.distance)) : 0;
        IEnumerable<RaycastHit> closestHit = hits.Where(hit => hit.distance == closestDist);

        // Get the first hit object out of the things the player collides with
        hit = closestHit.FirstOrDefault();

        // Return if any objects were hit
        return didHit;
    }

    //called when the left mouse button is pressed
    public void GetShootButton()
    {

        if (gunReady)
        {//spawn steak, launch steak
            GameObject steak = GameObject.Instantiate(bullet, bulletPoint.position, cameraTransform.rotation);
            //apply a foce to the newly created bullet after it is born
            steak.GetComponent<Rigidbody>().AddForce(bulletPoint.forward * bulletForce, ForceMode.VelocityChange);

            //steak.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), 0, Random.Range(0, 360)));
            gunReady = false;
            Invoke("gunCooldown", .25f);
        }

       
    }

    public void gunCooldown()
    {
        gunReady = true;
    }

    
}