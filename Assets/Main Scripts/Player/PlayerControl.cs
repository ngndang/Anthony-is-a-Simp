using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody controlTarget;
    [SerializeField] private Rigidbody head;
    private Vector3 movement;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxDistance = 1.7f;
    private float currentSpeed;
    private float currentRotationSpeed;

    private Vector3 direction;
    private bool followEnabled = true;

    [Header("Weapon")]
    [SerializeField] private Transform gunContainer;
    [SerializeField] private List<Transform> carryingGuns = new List<Transform>();
    private Transform activeGun;


    void Awake()
    {

        carryingGuns = FindChildrenWithTag(gunContainer, "Gun");
        ConfigActiveGun();
    }
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        //Shooting
        if (Input.GetMouseButton(0) && activeGun){
            AttemptShoot();
        }
    }

    void FixedUpdate()
    {
        controlTarget.velocity = movement.normalized * currentSpeed;
        controlTarget.position = new Vector3(controlTarget.position.x, head.position.y ,controlTarget.position.z);

        Follow();

        float distance = Vector3.Distance(controlTarget.transform.position, head.transform.position);
        if (distance >= maxDistance) {
            currentSpeed = 0f;
        } else {
            currentSpeed = speed;
        }

        if (distance <= 0.01f){ //Disable movement when its too close
            currentRotationSpeed = 0f;
            controlTarget.transform.position = head.transform.position;
            followEnabled = false;
        } else {
            currentRotationSpeed = rotationSpeed;
            followEnabled = true;
        }
        
        //print(Vector3.Distance(controlTarget.transform.position, head.transform.position));
    }

#region  ------------------Movement Functions--------------
    private void Follow(){
        if (followEnabled){
            Vector3 _direction = (controlTarget.position - head.position).normalized;
            if (_direction.x != 0) //Stop debug messages
                direction = _direction;

            Quaternion _lookRotation = Quaternion.LookRotation(direction);
            Vector3 _velocity = Vector3.zero;
            head.position = Vector3.SmoothDamp(head.position, controlTarget.transform.position, ref _velocity, smoothSpeed * Time.deltaTime);
            head.rotation = Quaternion.Slerp(head.rotation, _lookRotation, currentRotationSpeed * Time.deltaTime);
        }
    }
#endregion

#region ---------------------Gun Functions------------------------
    private void ConfigActiveGun(){
        activeGun = carryingGuns[0];
    }

    private void AttemptShoot() {
        activeGun.GetComponent<NormalGun>()?.Shoot();
    }
#endregion

#region  ------------------Helper Functions--------------
    private List<Transform> FindChildrenWithTag(Transform parent, string tag){
        List<Transform> objs = new List<Transform>();
        foreach(Transform child in parent.GetComponentsInChildren<Transform>())
            if (child.tag == tag)
                objs.Add(child);
        return objs;
    }

#endregion

}
