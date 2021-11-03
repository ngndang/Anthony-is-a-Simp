using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody controlTarget;
    [SerializeField] private Rigidbody head;
    private Vector3 movement;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector3 direction;
    void Start()
    {
        
    }
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");


    }

    void FixedUpdate()
    {
        controlTarget.velocity = movement.normalized * speed;
        
        Vector3 _direction = (controlTarget.position - head.position).normalized;
        if (_direction.x != 0){
        direction = _direction;
        }
        Quaternion _lookRotation = Quaternion.LookRotation(direction);
        Vector3 _velocity = Vector3.zero;
        head.position = Vector3.SmoothDamp(head.position, controlTarget.transform.position, ref _velocity, smoothSpeed * Time.deltaTime);
        head.rotation = Quaternion.Slerp(head.rotation, _lookRotation, rotationSpeed * Time.deltaTime);
    }
}
