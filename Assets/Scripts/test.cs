using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + 15*Time.fixedDeltaTime,0);
    }
}
