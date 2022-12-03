using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float damagePower = 10f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(SelfDestruct());
    }

    void Update(){
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
    }

    [Server]
    IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(10f);
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy")){
            //TODO:damagePower miktarında hasar ver
            Debug.Log("temas");
            NetworkServer.Destroy(other.gameObject);
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
