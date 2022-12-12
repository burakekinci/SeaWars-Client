using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootingController : NetworkBehaviour
{
    public GameObject bulletProjectile;
    public Transform firePointTransform;

    public float fireCooldownInSeconds = 1f;
    public float launchVelocity = 750f;
    private float time;
    public ParticleSystem FireParticle;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float nextTimeToFire = 1 / fireCooldownInSeconds;
        if(isLocalPlayer)
        {
            if(Input.GetButtonDown("Fire1") && time>= nextTimeToFire){
                CMDFire();
            }   
                    
        }

    }

    [ClientRpc]
    void RpcOnFire(){
        FireParticle.Play();
    }

    [Command]
    void CMDFire(){
        time=0;
        RpcOnFire();
        //FireParticle.Play();
        GameObject launchedBullet =  Instantiate(bulletProjectile,firePointTransform.transform.position,firePointTransform.transform.rotation);
        NetworkServer.Spawn(launchedBullet);
    }


}
