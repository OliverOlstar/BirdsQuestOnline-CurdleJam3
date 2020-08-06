using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float offSetUp = 0.6f;
    [SerializeField] private float dampening = 10;

    private void Start()
    {
        transform.parent = null;
    }

    void Update()
    {
        //Position the camera pivot on the player
        Vector3 targetPos = target.transform.position + (Vector3.up * offSetUp);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * dampening);
    }
}
