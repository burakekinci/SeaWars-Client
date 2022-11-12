using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OceanReader : MonoBehaviour
{
    public Transform ocean;
    Material oceanMat;

    [SerializeField]
    private Vector4 timeScales;

    [SerializeField]
    private Vector3 direction1;
    [SerializeField]
    private Vector3 direction2;
    [SerializeField]
    private Vector3 direction3;
    [SerializeField]
    private Vector3 direction4;

    [SerializeField]
    private float amplitude1;
    [SerializeField]
    private float amplitude2;
    [SerializeField]
    private float amplitude3;
    [SerializeField]
    private float amplitude4;

    [SerializeField]
    private float gravity;
    [SerializeField]
    private float depth;
    [SerializeField]
    private float phase;

    // Start is called before the first frame update
    void Start()
    {
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;
        InitializeVariables();
    }

    void InitializeVariables()
    {
        timeScales = oceanMat.GetVector("_TimeScales");

        direction1 = oceanMat.GetVector("_Direction1");
        direction2 = oceanMat.GetVector("_Direction2");
        direction3 = oceanMat.GetVector("_Direction3");
        direction4 = oceanMat.GetVector("_Direction4");

        amplitude1 = oceanMat.GetFloat("_Amplitude1");
        amplitude2 = oceanMat.GetFloat("_Amplitude2");
        amplitude3 = oceanMat.GetFloat("_Amplitude3");
        amplitude4 = oceanMat.GetFloat("_Amplitude4");

        gravity = oceanMat.GetFloat("_Gravity");
        depth = oceanMat.GetFloat("_Depth");
        phase = oceanMat.GetFloat("_Phase");

    }

    public float HeightOfVertex(Vector3 position)
    {
        float result = DisplacementY(position).y;
        Debug.Log($"Result is {result}");
        return result;
    }

    float Frequency(Vector3 direction)
    {
        float length = direction.magnitude;
        return Mathf.Sqrt((gravity*length)*(math.tanh(length*depth)));
    }

    float Theta(Vector3 direction, Vector3 position, float timeMultiplier)
    {
        return ((direction.x*position.x) + (direction.z*position.z)-timeMultiplier*Frequency(direction))-phase;
    }
    Vector3 GerstnerWaveY(Vector3 direction, Vector3 position, float amplitude, float timeMultiplier)
    {
        float length = direction.magnitude;
        float xInner1 = direction.x / length;
        float zInner1 = direction.z / length;
        float xzCommonInner = amplitude / math.tanh(length * depth);
        float thetaValue = Theta(direction, position, timeMultiplier);
        float x = -math.sin(thetaValue) * (xInner1 * xzCommonInner); 
        float y = math.cos(thetaValue) * amplitude;
        float z = -math.sin(thetaValue) * (zInner1 * xzCommonInner);
        return new Vector3(x,y,z);
    }

    Vector3 DisplacementY(Vector3 position)
    {
        return transform.TransformPoint(GerstnerWaveY(direction1, position, amplitude1, timeScales.x * Time.time) + 
               GerstnerWaveY(direction2, position, amplitude2, timeScales.y * Time.time) +
               GerstnerWaveY(direction3, position, amplitude3, timeScales.z * Time.time) +
               GerstnerWaveY(direction4, position, amplitude4, timeScales.w * Time.time) + 
               position);
    }
}
