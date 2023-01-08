using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    [SerializeField] ParticleSystem motorFoam;
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
       
       
        Debug.Log("init test");
        motorFoamEmission = motorFoam.emission;
        motorFoamEmission.rateOverTime = 0;
        maxSpeed = Mathf.Sqrt(enginePower / rb.mass)  * crouchConstant;
       

        if(isLocalPlayer){
            if(SceneManager.GetActiveScene().name == "Multiplayer")
            {
                playerCamera = Instantiate(cameraPrefab,transform.position,Quaternion.identity);
            }
            Debug.Log("Local playera girdi.. kamera instantiate etmeli...");
            
        }else{
            if(playerCamera!=null)
                playerCamera.gameObject.SetActive(false);
            else{
                Debug.Log("Clientların camerası NULL -> olması gereken de bu zaten");
            }
        }

    }

    public override void OnStartClient()
    {
        //ShipType tmp = PlayerStats.Instance.shipType;
        InitializeShipType(PlayerStats.Instance.SelectedShipType);
        CmdStartParticles();
        Debug.Log("clientStarted and particle pos updated");  
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

    void InitializeShipType(ShipType shipType){
        string s_shipType;
        Debug.Log("init ici");
        GameObject shipObject;
        Transform turretTransform;
        NetworkTransform[] networkTransforms = gameObject.GetComponents<NetworkTransform>();

        
        switch (shipType)
        {
            case ShipType.CORVETTE:
                s_shipType = "Corvette";
                break;
            case ShipType.CORVETTE_GREEN:
                s_shipType = "CorvetteGreen";
                break;
            case ShipType.FRIGATE:
                s_shipType = "Frigate";
                break;
            case ShipType.FRIGATE_GREEN: 
                s_shipType = "Frigate_Green";
                break;                      
            default:
                s_shipType = "Corvette";
                break;
        }
        shipObject = gameObject.transform.Find(s_shipType).gameObject;
        turretTransform = shipObject.transform.Find("Weapons").Find("SmallTurret");

        if(shipObject){
            shipObject.SetActive(true);
            Debug.Log(shipObject);
        }else{
            Debug.Log("shipObje bulunamadı");
        }

        //Network transform of ship
        networkTransforms[0].target = gameObject.transform;

        //Network transform of turret
        networkTransforms[1].target = shipObject.transform.Find("Weapons").Find("SmallTurret");

        gameObject.GetComponent<CameraController>().turret = turretTransform;
        gameObject.GetComponent<ShootingController>().firePointTransform = turretTransform.Find("FirePointTransform");
        gameObject.GetComponent<ShootingController>().FireParticle = turretTransform.Find("SmallExplosionEffect").GetComponent<ParticleSystem>();
        motorFoam = shipObject.transform.Find("FoamParticle").GetComponent<ParticleSystem>();
    }
}
