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
    private bool allowFire = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer){
            return;
        }


        if(Input.GetButtonDown("Fire1") && allowFire){
            StartCoroutine(Fire());
        }   
    }

    IEnumerator Fire(){
        allowFire=false;
        GameObject launchedBullet =  Instantiate(bulletProjectile,firePointTransform.transform.position,firePointTransform.transform.rotation);
        launchedBullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,launchVelocity,0),ForceMode.Impulse);
        yield return new WaitForSeconds(fireCooldownInSeconds);
        allowFire=true;
    }
}
