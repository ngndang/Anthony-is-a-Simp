using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnt : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 12f;
    private NavMeshAgent headAgent;
    void Awake()
    {
        head = transform.Find("Head").gameObject;
        headAgent = head.GetComponent<NavMeshAgent>();

        headAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        headAgent.destination = target.position;
    }
}
