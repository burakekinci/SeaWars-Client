using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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

    IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy")){
            //TODO:damagePower miktarında hasar ver
            Debug.Log("temas");
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
