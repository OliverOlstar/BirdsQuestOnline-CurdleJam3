using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crouching : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float dampening = 5;
    private float targetHeight = 0;
    private float targetBob = 0;
    private float targetRand = 0;

    private Coroutine bobCoroutine;

    private void Start()
    {
        StartCoroutine(RandBobRoutine());
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, Mathf.Max(targetHeight + targetBob + targetRand, -0.36f), 0), Time.deltaTime * dampening);
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            targetHeight = -0.25f;
        }
        else
        {
            targetHeight = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targetHeight);
        }
        else
        {
            targetHeight = (float)stream.ReceiveNext();
        }
    }

    public void OnGround()
    {
        if (bobCoroutine != null)
            StopCoroutine(bobCoroutine);
        bobCoroutine = StartCoroutine(bobRoutine(0.05f, 0.1f, 0.15f, -0.35f, 0));
    }

    public void OffGround()
    {
        if (bobCoroutine != null)
            StopCoroutine(bobCoroutine);
        bobCoroutine = StartCoroutine(bobRoutine(0.1f, 0.1f, 0.1f, -0.2f, 0));
    }

    private IEnumerator bobRoutine(float pFadeIn, float pPause, float pFadeOut, float pValue, float pEndValue)
    {
        float startingValue = targetBob;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime / pFadeIn;
            targetBob = Mathf.Lerp(startingValue, pValue, progress);
            yield return null;
        }

        if (pPause > 0)
            yield return new WaitForSeconds(pPause);

        progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime / pFadeOut;
            targetBob = Mathf.Lerp(pValue, pEndValue, progress);
            yield return null;
        }
    }

    private IEnumerator RandBobRoutine()
    {
        float progress = 0;
        while (true)
        {
            progress += Time.deltaTime;
            targetRand = Mathf.Cos(progress + Mathf.PI) / 21.0f;
            yield return null;
        }
    }
}
