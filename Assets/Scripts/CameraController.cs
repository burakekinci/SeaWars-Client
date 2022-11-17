using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity=3f;

    private float _rotationX;
    private float _rotationY;
    private float _mouseX;
    private float _mouseY;

    [SerializeField]
    private float lookUpLimit = 20f;
    
    [SerializeField]
    private float lookDownLimit = -20f;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget=10f;
    
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity=Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        _mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += _mouseX;
        _rotationX += _mouseY;

        _rotationX = Mathf.Clamp(_rotationX, lookDownLimit,lookUpLimit);
        Vector3 nextRotation = new Vector3(-_rotationX,_rotationY);
        _currentRotation = Vector3.SmoothDamp(_currentRotation,nextRotation,ref _smoothVelocity,_smoothTime);

        transform.localEulerAngles = _currentRotation;
        transform.position = _target.position - transform.forward * _distanceFromTarget;

    }

}