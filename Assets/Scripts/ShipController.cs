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
    
    Rigidbody rigidbody;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        var localVel = transform.InverseTransformDirection(rigidbody.velocity);
        
        if(Input.GetAxis("Vertical")>0){
            thrustPower += thrustPowerIncrease;
        }else if(Input.GetAxis("Vertical")<0){
            thrustPower -= thrustPowerIncrease;
        }

        if(localVel.x!=0f){
            transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * SteerSpeed * Time.fixedDeltaTime * localVel.x / 10, 0);
        }

        if(thrustPower!=0){
            rigidbody.AddRelativeForce(Vector3.right * thrustPower * EnginePower *Time.deltaTime);
        }

        thrustPower = Mathf.Clamp(thrustPower,-15,100);
    }


}
