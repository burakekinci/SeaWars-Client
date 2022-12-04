using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    [SerializeField]
    private float _mouseSensitivity=3f;

    private float _rotationX;
    private float _rotationY;
    public float turretRotateOffset;
    private float _mouseX;
    private float _mouseY;

    [SerializeField]
    private float lookUpLimit = 20f;
    
    [SerializeField]
    private float lookDownLimit = -20f;

    
    public Transform _target;
    public Transform turret;
    public Camera mainCamera;

    [SerializeField]
    private float _distanceFromTarget=10f;
    
    private Vector3 _currentRotation;
    private Vector3 _currentTurretRotation;
    private Vector3 _smoothVelocity=Vector3.zero;
    private Vector3 _smoothTurretVelocity=Vector3.zero;

    private Vector3 _cameraLookVector;
    private Vector3 _turretLookVector;

    [SerializeField]
    private float _smoothTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _target = gameObject.transform;
        turret = gameObject.transform.Find("Turret").transform.Find("GunRoot");
        

        if(_target == null || turret == null || mainCamera == null){
            Debug.Log("not assigned properly");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer){
            return;
        }
        mainCamera = gameObject.GetComponent<ShipController>().playerCamera;
        if(mainCamera == null){
            return;
        }
        _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        _mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += _mouseX;
        _rotationX += _mouseY;

        _rotationX = Mathf.Clamp(_rotationX, lookDownLimit,lookUpLimit);
        Vector3 nextRotation = new Vector3(-_rotationX,_rotationY);
        _currentRotation = Vector3.SmoothDamp(_currentRotation,nextRotation,ref _smoothVelocity,_smoothTime);

        mainCamera.transform.localEulerAngles = _currentRotation;
        mainCamera.transform.position = _target.position - mainCamera.transform.forward * _distanceFromTarget;

        _cameraLookVector = new Vector3(mainCamera.transform.position.x,transform.position.z);
        _turretLookVector = new Vector3(turret.position.x,turret.position.z);
        
        
        Vector3 turretNextRotation = new Vector3(-_rotationX-turretRotateOffset,_rotationY) + mainCamera.transform.forward - turret.forward;
        //_currentTurretRotation = turret.transform.localEulerAngles;
        _currentTurretRotation = Vector3.SmoothDamp(_currentTurretRotation,turretNextRotation,ref _smoothTurretVelocity,_smoothTime);
        turret.eulerAngles = _currentTurretRotation;
    }

}
