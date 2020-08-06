using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Legs : MonoBehaviour
{
    public UnityEvent Stepped;

    [SerializeField] private Transform target;
    [SerializeField] private Transform airTarget;
    [SerializeField] private Rigidbody rb;
    private LimbIK ik;
    private Transform ikPos;
    private Vector3 targetPos;
    private Quaternion targetRot;

    [SerializeField] private float maxDistance = 1;
    [SerializeField] private float addedStepMult = 1;
    [SerializeField] private float stepSpeed = 2;

    [SerializeField] private Legs opposingLeg;
    public bool stepping = false;
    private bool grounded = true;
    private float lerpRate = 16;


    void Start()
    {
        ikPos = new GameObject().transform;
        ikPos.position = target.position;

        ik = GetComponent<LimbIK>();
        ik.solver.target = ikPos.transform;

        targetPos = ikPos.position;
        targetRot = ikPos.rotation;
    }

    private void Update()
    {
        if (grounded == true)
        {
            if (Vector3.Distance(target.position, targetPos) > maxDistance && stepping == false && opposingLeg.stepping == false)
            {
                Vector3 nextPos = target.position + new Vector3(rb.velocity.x, 0, rb.velocity.z) * addedStepMult;
                nextPos += new Vector3((Random.value - 0.5f) / 4, 0, (Random.value - 0.5f) / 4);
                nextPos.y = NextPosHeight(nextPos);

                StopAllCoroutines();
                StartCoroutine(stepRoutine(targetPos, targetRot, nextPos, target.rotation));
            }
        }
        else
        {
            targetPos = airTarget.position;
            targetRot = airTarget.rotation;
        }

        ikPos.position = Vector3.Lerp(ikPos.position, targetPos, Time.deltaTime * lerpRate);
        ikPos.rotation = Quaternion.Lerp(ikPos.rotation, targetRot, Time.deltaTime * lerpRate);
    }

    private IEnumerator stepRoutine(Vector3 pStart, Quaternion pStartRot, Vector3 pEnd, Quaternion pEndRot)
    {
        stepping = true;
        Vector3 middlePos = (pStart + pEnd) / 2 + Vector3.up * 0.25f;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * stepSpeed;
            targetPos = GetQuadraticCurvePoint(progress, pStart, middlePos, pEnd);
            targetRot = Quaternion.Lerp(pStartRot, pEndRot, progress);
            yield return null;
        }

        Stepped.Invoke();
        stepping = false;
    }

    private float NextPosHeight(Vector3 pPos)
    {
        RaycastHit hit;
        if (Physics.Linecast(pPos + Vector3.up * 0.4f, pPos - Vector3.up * 0.5f, out hit))
        {
            return hit.point.y;
        }
        else
        {
            return 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(target.position + Vector3.up * 0.5f, target.position - Vector3.up * 0.3f);
    }

    public void OnGround()
    {
        Vector3 nextPos = target.position + new Vector3(rb.velocity.x, 0, rb.velocity.z) * addedStepMult;
        nextPos += new Vector3((Random.value - 0.5f) / 4, 0, (Random.value - 0.5f) / 4);
        nextPos.y = NextPosHeight(nextPos);

        StopAllCoroutines();
        StartCoroutine(stepRoutine(targetPos, targetRot, nextPos, target.rotation));

        grounded = true;
        lerpRate = 16;
    }

    public void OffGround()
    {
        StopAllCoroutines();

        grounded = false;
        lerpRate = 35;
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
