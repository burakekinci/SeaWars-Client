using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : NetworkBehaviour
{
    public float thrustPowerIncrease = 0.5f;
    public float steerSpeed = 20;
    [SyncVar] public float thrustPower;
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
    
    ParticleSystem motorFoam;
    ParticleSystem.EmissionModule motorFoamEmission;
    Rigidbody rb;
    public Camera playerCamera;
    public float maxSpeed;
    
    private void Awake() {
        Debug.Log("awaked");
    }

    void Start()
    {
        Debug.Log("ship started");
        rb = gameObject.GetComponent<Rigidbody>();
        motorFoam = transform.Find("FoamParticle").GetComponent<ParticleSystem>();
        
        motorFoamEmission = motorFoam.emission;
        motorFoamEmission.rateOverTime = 0;
        maxSpeed = Mathf.Sqrt(enginePower / rb.mass)  * crouchConstant;
       

        if(isLocalPlayer){
            playerCamera = Instantiate(cameraPrefab,transform.position,Quaternion.identity);
            CmdStartParticles();
            
        }else{
            if(playerCamera!=null)
                playerCamera.gameObject.SetActive(false);
            else{
                Debug.Log("Clientların camerası NULL -> olması gereken de bu zaten");
            }
        }

    }

    void FixedUpdate()
    {
        var localVel = transform.InverseTransformDirection(rb.velocity);
        if(isLocalPlayer)
        {
            if(Input.GetAxis("Vertical")>0){
                CmdChangeThrustPower(thrustPowerIncrease);
            }else if(Input.GetAxis("Vertical")<0){
                CmdChangeThrustPower(-thrustPowerIncrease);
            }

            if(localVel.z!=0f && Input.GetAxis("Horizontal")!=0f){
                var desiredQuaternion = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * steerSpeed * localVel.z / 10, 0);
                //transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * SteerSpeed * Time.fixedDeltaTime * localVel.z / 10, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation,desiredQuaternion, Time.fixedDeltaTime * damping);
            }

            if(thrustPower!=0){
                rb.AddRelativeForce(Vector3.forward * thrustPower * enginePower *Time.deltaTime);
            }

            thrustPower = Mathf.Clamp(thrustPower,-maxReverseThrustPower,100);
            
            rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxSpeed);
        
        }

        if(localVel.magnitude>0f){
            motorFoamEmission.rateOverTime = thrustPower * foamParticleMultiplier + foamParticleBase;
        }else{
            motorFoamEmission.rateOverTime = foamParticleBase;
        }
    }

    //Networkings
    [Command]
    void CmdStartParticles(){
        RPCStartParticles();
        Debug.Log("command sended");
    }

    [Command]
    void CmdChangeThrustPower(float increaseValue){
        thrustPower += increaseValue;
    }

    [ClientRpc]
    void RPCStartParticles(){
        DoStartParticles();
        Debug.Log("rpc recevied");
    }

    void DoStartParticles(){
        motorFoam.Play();
    }
}
