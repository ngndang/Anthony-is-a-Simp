using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntLeg : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform stepNode;
    [SerializeField] private GameObject kneeNodePrefab;
    private GameObject kneeNode;
    //private Vector3[] points = new Vector3[3]{Vector3.zero, Vector3.zero, Vector3.zero};
    [SerializeField] private Vector3[] points = new Vector3[3];
    private float[] lengths = new float[2];



    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        stepNode = transform.GetChild(0);

        lineRenderer.GetPositions(points);
        CreateKneeNode();
        GetLengths();
    }

    // Update is called once per frame
    void Update()
    {

        //points[1] = stepNode.transform.localPosition;

        //Solve(points, stepNode.transform.localPosition);
        Solve2();
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
    const int maxIterations = 10;
    const float minDist = 0.01f;
    public void Solve(Vector3[] points, Vector3 target){
        Vector3 origin = points[0];
        float[] lengths = new float[points.Length - 1];

        for (int i = 0; i < points.Length - 1; i++)             //Get distances between points
        {
            lengths[i] = Vector3.Distance(points[i+1], points[i]);
        }

        for (int it = 0; it < maxIterations; it ++)     //it: Iterations
        {
            bool startsFromTarget = it % 2 == 0;        //Divisible by 2

            System.Array.Reverse(points);
            System.Array.Reverse(lengths);
            points[0] = startsFromTarget ? target : origin;

            for (int i = 1; i < points.Length; i++)            //Constraint lengths
            {
                Vector3 dir = (points[i] - points[i-1]).normalized; //Get direction 
                points[i] = points[i-1] + dir * lengths[i-1];
            } 

            float distToTarget = Vector3.Distance(points[points.Length - 1], target);
            if (!startsFromTarget && distToTarget <= minDist) {
                return;
            }
        }
    }

    private void Solve2(){
        points[0] = Vector3.zero; //Origin
        points[2] = stepNode.transform.localPosition;       //Target

        Vector3 dir = (kneeNode.transform.localPosition - points[2]).normalized;

        points[1] = points[2] + dir * lengths[1];
    }

    private void CreateKneeNode(){
        float length = Vector3.Distance(points[2], points[1]) * -0.8f;
        Vector3 dir = (points[2] - points[1]).normalized;
        Vector3 pos = points[1] + dir * length; 
        kneeNode = Instantiate(kneeNodePrefab, transform, true);
        kneeNode.transform.localPosition = pos;
    }

    private void GetLengths(){
        for (int i = 0; i < points.Length - 1; i++)             //Get distances between points
        {
            lengths[i] = Vector3.Distance(points[i+1], points[i]);
        }
    }
}
