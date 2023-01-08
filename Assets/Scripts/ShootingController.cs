using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class ShootingController : NetworkBehaviour
{
    public Bullet bulletProjectile;
    public Transform firePointTransform;
    
    public float fireCooldownInSeconds = 1f;
    public float launchVelocity = 750f;
    private float time;

    [SyncVar] public  int health=30;
    public ParticleSystem FireParticle;

    public GameObject hitMarker;
 
    // Start is called before the first frame update
    void Start()
    {
       //hitMarker = GameObject.Find("Canvas").transform.Find("HitMarker").gameObject;
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
        if(hitMarker == null && SceneManager.GetActiveScene().name == "Multiplayer")
        {
            hitMarker = GameObject.Find("Canvas").transform.Find("HitMarker").gameObject;
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
        Bullet launchedBullet =  Instantiate(bulletProjectile,firePointTransform.transform.position,firePointTransform.transform.rotation);
        launchedBullet.SetController(this);
        NetworkServer.Spawn(launchedBullet.gameObject);
    }

    [Client]
    public void HitActive()
    {
        hitMarker.SetActive(true);
        Invoke("HitDisable",0.25f);
    }

    void HitDisable()
    {
        hitMarker.SetActive(false);
    }


    [ClientRpc]
    public void RpcOnDeath()
    {
        NetworkServer.Destroy(this.gameObject);
        //ExplosionParticle.Play();
    }

}
