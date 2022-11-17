using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    public float SteerSpeed;
    public float thrustPower;
    public float thrustPowerIncrease;
    public float EnginePower;

    [SerializeField]
    private float damping = 10;

    [SerializeField]
    float foamParticleMultiplier;

    [SerializeField]
    float foamParticleBase;
    
    ParticleSystem.EmissionModule motorFoam;
    Rigidbody rb;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        motorFoam = transform.Find("FoamParticle").GetComponent<ParticleSystem>().emission;  
    }

    void FixedUpdate()
    {
        var localVel = transform.InverseTransformDirection(rb.velocity);
        motorFoam.rateOverTime = thrustPower * foamParticleMultiplier + foamParticleBase;

        if(Input.GetAxis("Vertical")>0){
            thrustPower += thrustPowerIncrease;
        }else if(Input.GetAxis("Vertical")<0){
            thrustPower -= thrustPowerIncrease;
        }

        if(localVel.z!=0f && Input.GetAxis("Horizontal")!=0f){
            var desiredQuaternion = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * SteerSpeed * localVel.z / 10, 0);
            //transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * SteerSpeed * Time.fixedDeltaTime * localVel.z / 10, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation,desiredQuaternion, Time.fixedDeltaTime * damping);
        }

        if(thrustPower!=0){
            rb.AddRelativeForce(Vector3.forward * thrustPower * EnginePower *Time.deltaTime);
        }

        thrustPower = Mathf.Clamp(thrustPower,-15,100);
    }


}
