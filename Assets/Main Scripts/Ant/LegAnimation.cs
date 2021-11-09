using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimation : MonoBehaviour
{
    [SerializeField] private GameObject restNodePrefab;
    private GameObject restNode;
    [SerializeField] private Transform body;
    private float groundY = 0;
    [SerializeField] private float legSpeed = 2f; 
    [SerializeField] private float stepLength = 1.5f;
    private Vector3 legRefVelocity; 
    private bool move;

    [SerializeField] private bool shiftLeg = false;
    [SerializeField] private float shiftAmount = 0.7f;
    
    void Start()
    {
        body = transform.parent.parent;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity)){
            if (hit.transform.CompareTag("Ground")){
                groundY = hit.transform.position.y;
            }
        }

        restNode = Instantiate(restNodePrefab, transform.position, Quaternion.identity, body.transform);
        Vector3 localPos = restNode.transform.localPosition;
        restNode.transform.localPosition = new Vector3(localPos.x, localPos.y, localPos.z);

        if (shiftLeg)
        transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + shiftAmount);

    }

    // Update is called once per frame
    void Update()
    {

        float distToRest = Vector3.Distance(transform.position, restNode.transform.position);
        if (distToRest >= stepLength){
            move = true;
        }

        if (move) {
            //transform.position = new Vector3(restNode.transform.position.x, groundY + groundOffset, restNode.transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, restNode.transform.position, ref legRefVelocity, legSpeed * Time.deltaTime); 
            
            if (distToRest <= 0.5f) {
                move = false;
                transform.position = restNode.transform.position;
                return;
            }
        }


    }
}
