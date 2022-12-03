using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : NetworkBehaviour
{
    public float thrustPowerIncrease = 0.5f;
    public float steerSpeed = 20;
    public float thrustPower;
    public float enginePower = 1000;
    public float crouchConstant = 190; //light cruiser -> 190, heavy cruiser->150
    public float maxReverseThrustPower= 30;   
    public Camera cameraPrefab;

    [SerializeField]
    private float damping = 10;

    [SerializeField]
    float foamParticleMultiplier;

    [SerializeField]
    float foamParticleBase;
    
    ParticleSystem.EmissionModule motorFoam;
    Rigidbody rb;
    Camera playerCamera;
    public float maxSpeed;
    
    private void Awake() {
        if(isLocalPlayer)
            playerCamera = Instantiate(cameraPrefab,transform.position,Quaternion.identity);
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        motorFoam = transform.Find("FoamParticle").GetComponent<ParticleSystem>().emission;
        motorFoam.rateOverTime = 0;
        maxSpeed = Mathf.Sqrt(enginePower / rb.mass)  * crouchConstant;
        
        if(!isLocalPlayer){
            playerCamera.gameObject.SetActive(false);
        }

    }

    void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            var localVel = transform.InverseTransformDirection(rb.velocity);
        
            if(Input.GetAxis("Vertical")>0){
                thrustPower += thrustPowerIncrease;
            }else if(Input.GetAxis("Vertical")<0){
                thrustPower -= thrustPowerIncrease;
            }

            if(localVel.z!=0f && Input.GetAxis("Horizontal")!=0f){
                var desiredQuaternion = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * steerSpeed * localVel.z / 10, 0);
                //transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * SteerSpeed * Time.fixedDeltaTime * localVel.z / 10, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation,desiredQuaternion, Time.fixedDeltaTime * damping);
            }

            if(localVel.magnitude>0f){
                motorFoam.rateOverTime = thrustPower * foamParticleMultiplier + foamParticleBase;
            }else{
                motorFoam.rateOverTime = foamParticleBase;
            }

            if(thrustPower!=0){
                rb.AddRelativeForce(Vector3.forward * thrustPower * enginePower *Time.deltaTime);
            }

            thrustPower = Mathf.Clamp(thrustPower,-maxReverseThrustPower,100);
            
            rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxSpeed);
        
        }
    }


}
