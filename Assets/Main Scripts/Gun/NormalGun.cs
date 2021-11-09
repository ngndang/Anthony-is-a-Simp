using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : MonoBehaviour
{
    public List<Transform> firePoints = new List<Transform>();
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public float cooldownDuration = 1f;
    public float spread = 1f;
    [Range(0f, 1.2f)] public float recoil = 1f;

    [Header("Aiming")]
    public Transform aimingTarget;
    [SerializeField] private Camera mouseTrackCamera;

    public float maxAimAngle = 0f;
    public float aimSpeed = 10f;

    private float cooldown = 0f;

    private Transform head;

    void Awake()
    {
        //firePoints = FindFirePoints("FirePoint");
        head = transform.parent.parent.parent.Find("Head");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0f){
            cooldown -= Time.deltaTime;
            //print("Cooldown: " + cooldown);
        }

        if (!aimingTarget)
            Aiming(GetMousePos());
        else
            Aiming(aimingTarget.position);

    }

    public void Shoot(){
        if (cooldown <= 0f){
            foreach (Transform firePoint in firePoints){
                var bullet = Instantiate(bulletPrefab,
                    firePoint.position,
                    firePoint.rotation * Quaternion.Euler(0, Random.Range(-spread, spread), 0));

                var bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.velocity = (bullet.transform.forward * bulletSpeed);


                bullet.GetComponent<ActiveBullet>().spawner = head.gameObject;
            }
            head.transform.position = head.transform.position - transform.forward * recoil;
            cooldown = cooldownDuration;
        }
    }
    private void Aiming(Vector3 targetPos){
        if (maxAimAngle <= 0f) 
            return;
                    
        Vector3 headDir = head.forward * 5f;
        Vector3 leveledHeadDir = new Vector3(headDir.x, 0, headDir.z);

        Vector3 leveledTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 dir = (leveledTarget - transform.position).normalized * 5f;
        Vector3 leveledDir = new Vector3(dir.x, 0, dir.z);

        Debug.DrawRay(transform.position, leveledDir, Color.red);
        Debug.DrawRay(transform.position, leveledHeadDir, Color.blue);
        
        Vector3 angleDifference = Quaternion.FromToRotation(leveledHeadDir, leveledDir).eulerAngles;
        Quaternion rotation = Quaternion.LookRotation(leveledDir);

        //Clamp Aim 
        if (((angleDifference.y <= 180 && angleDifference.y <= maxAimAngle) || (angleDifference.y >= 180 && (angleDifference.y - 360f) >= -maxAimAngle) )) { 
            Debug.DrawRay(transform.position, leveledDir, Color.white);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, aimSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetMousePos(){
        Plane plane = new Plane(Vector3.up, 0);
        Vector3 hitPoint;
        float distance;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = mouseTrackCamera.ScreenPointToRay(Input.mousePosition);
        
        if (plane.Raycast(ray, out distance))
        {   
            hitPoint = ray.GetPoint(distance);
        } else {
            hitPoint = Vector3.zero;
        }        
        return hitPoint;
    }

}

