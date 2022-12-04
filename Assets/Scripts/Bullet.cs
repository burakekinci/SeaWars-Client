using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float damagePower = 10f;
    public float launchVelocity = 200f;
    public float lifeTime = 5f;
    private Rigidbody rb;

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
            //TODO:damagePower miktarında hasar ver
            Debug.Log("temas");
            NetworkServer.Destroy(other.gameObject);
            NetworkServer.Destroy(this.gameObject);
        //}
    }
}
