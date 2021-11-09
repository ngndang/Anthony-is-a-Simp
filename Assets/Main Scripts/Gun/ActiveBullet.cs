using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBullet : MonoBehaviour
{
    public GameObject spawner;
    void Awake()
    {
        if (spawner){
            Physics.IgnoreCollision(spawner.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
