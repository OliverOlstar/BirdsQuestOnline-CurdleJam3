using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpRotate : MonoBehaviour
{
    [SerializeField] private Rigidbody target;
    [SerializeField] private float dampening = 5;

    void Update()
    {
        Quaternion targetQ;

        if (new Vector2(target.velocity.x, target.velocity.z).magnitude >= 0.5f)
            targetQ = Quaternion.LookRotation(new Vector3(target.velocity.x, 0, target.velocity.z));
        else
            targetQ = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z));

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQ, Time.deltaTime * dampening);
    }

    public void SetDampening(float pDamp)
    {
        dampening = pDamp;
    }
}
