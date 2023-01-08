using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class BuoyancyObject : MonoBehaviour
{
    public Transform[] floaters;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;



    [SerializeField]
    //private OceanReader oceanReader;
    Rigidbody m_rigidbody;
    bool underWater;

    int floatersUnderWater;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        //oceanReader = FindObjectOfType<OceanReader>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        floatersUnderWater = 0;
        for(int i=0; i < floaters.Length; i++)
        {                                             //oceanReader.HeightOfVertex(floaters[i].position)
            float difference = floaters[i].position.y - OceanReader.Instance.HeightOfVertex(floaters[i].position);

            if (difference < 0)
            {
                m_rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderWater += 1;
                if (!underWater)
                {
                    underWater = true;
                    SwitchState(true);
                }
            }
        }
        
        if (underWater && floatersUnderWater == 0)
        {
            underWater = false;
            SwitchState(false);
        }
    }


    void SwitchState(bool isUnderWater)
    {
        if (isUnderWater)
        {
            m_rigidbody.drag = underWaterDrag;
            m_rigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            m_rigidbody.drag = airDrag;
            m_rigidbody.angularDrag = airAngularDrag;
        }
    }
}
