using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCube : MonoBehaviour
{
    [SerializeField] private float riseAmount = 2;
    [SerializeField] private float riseTime = 60;
    [SerializeField] private float riseSpin = 50;
    private Vector3 startingPos;
    private Quaternion startingRot;

    private Material startMat;
    [SerializeField] private Material riseMat;
    private Renderer renderer;

    [HideInInspector] public bool Active = false;
    [HideInInspector] public bool NeverEnd = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        startMat = renderer.material;

        startingPos = transform.position;
        startingRot = transform.rotation;
    }

    public void Trigger()
    {
        StopAllCoroutines();
        StartCoroutine(AliveRoutine());
    }

    private IEnumerator AliveRoutine()
    {
        renderer.material = riseMat;

        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        Active = true;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * 1.8f;
            transform.position = Vector3.Lerp(pos, startingPos + Vector3.up * riseAmount, progress);
            transform.rotation = Quaternion.Lerp(rot, Quaternion.identity, progress);
            yield return null;
        }

        progress = 0;
        while (progress < riseTime || NeverEnd)
        {
            progress += Time.deltaTime;
            float value = Mathf.Cos(progress / 2);
            transform.Rotate(new Vector3(value, Mathf.Sin(progress) + 1, 1 - value) * riseSpin * Time.deltaTime);
            yield return null;
        }

        renderer.material = startMat;
        pos = transform.position;
        rot = transform.rotation;

        Active = false;

        progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, startingPos, progress);
            transform.rotation = Quaternion.Lerp(rot, startingRot, progress);
            yield return null;
        }
    }
}
