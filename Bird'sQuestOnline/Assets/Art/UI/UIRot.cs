using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRot : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float mag = 2;
    [SerializeField] private float offset = 2;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Cos(Time.time * speed) * mag - offset);
    }
}
