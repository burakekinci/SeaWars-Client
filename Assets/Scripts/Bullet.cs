using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public int damagePower = 10;
    public float launchVelocity = 200f;
    public float lifeTime = 5f;
    
    [SerializeField] ParticleSystem explodeParticle;
    private Rigidbody rb;

    private ShootingController shootingController;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Invoke(nameof(SelfDestruct),lifeTime);
        rb.AddRelativeForce(new Vector3(0,launchVelocity,0),ForceMode.Impulse);
    }

    void Update(){
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
    }

    [Server]
    void SelfDestruct(){
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    void OnCollisionEnter(Collision other) {
        //if(other.gameObject.CompareTag("Enemy")){
            if(other.gameObject == shootingController.gameObject)
                return;
            Debug.Log("temas");
            shootingController.HitActive();
            var otherShip = other.gameObject?.GetComponent<ShootingController>();
            if(otherShip != null)
            {
                otherShip.health -= damagePower;
                if(otherShip.health<=0)
                {
                    otherShip.RpcOnDeath();
                }
            }
            DestroySelf();
        //}
    }

    public void SetController(ShootingController shootingController){
        this.shootingController = shootingController;
    }

    void DestroySelf()
    {
        CMDExplode();
        NetworkServer.Destroy(this.gameObject);
    }

    [Command]
    void CMDExplode()
    {
        RpcOnExplode();
    }

    [ClientRpc]
    void RpcOnExplode()
    {
        explodeParticle.Play();
    }
}
