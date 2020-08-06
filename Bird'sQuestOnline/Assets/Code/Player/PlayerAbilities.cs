using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviourPunCallbacks, IPunObservable
{
    public UnityEvent started;
    public UnityEvent ended;
    public UnityEvent Chirped;

    [SerializeField] private Transform ChirpPos;
    [SerializeField] private Transform PeckPos;

    private Coroutine routine;
    private bool pecking = false;

    private IPeckable curTarget;
    private Transform holding;
    private IPeckable holdingPeck;

    private bool SendChirp = false;

    public void OnChirp(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            curTarget = null;

            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(AbilityRoutine(ChirpPos, 5, 0.1f, 3.5f, 0.05f, 1, true));
        }
    }

    public void OnPeck(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (pecking == false)
            {
                FindPeckTarget();

                if (routine != null)
                    StopCoroutine(routine);
                routine = StartCoroutine(AbilityRoutine(PeckPos, 9, 0.0f, 5f, 0.3f, 2));
            }
        }
        else if (ctx.canceled)
        {
            DropObject();
        }
    }

    public void DropObject()
    {
        Debug.Log("Host: Drop");
        if (holding != null)
        {
            holdingPeck.Released();
            holding.SetParent(null);
            holding = null;
            holdingPeck = null;
        }

        curTarget = null;
    }

    private void FindPeckTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward / 2 - Vector3.up / 4, 1);
        foreach (Collider other in colliders)
        {
            curTarget = other.GetComponent<IPeckable>();
            if (curTarget != null)
            {
                // Other Pos
                PeckPos.position = other.transform.position;
                PeckPos.rotation = Quaternion.LookRotation(other.transform.position - transform.position);
                return;
            }
        }

        curTarget = null;

        // Default Pos
        PeckPos.position = transform.parent.position + transform.parent.forward * 0.8f;
        PeckPos.rotation = transform.parent.rotation;
    }

    private IEnumerator AbilityRoutine(Transform pTarget, float pFadeIn, float pReturnDelay, float pFadeOut, float pTargetDistance = 0.05f, float pRotSpeed = 1, bool pChirp = false)
    {
        pecking = true;
        started.Invoke();

        while (Vector3.Distance(transform.position, pTarget.position) > pTargetDistance)
        {
            transform.position = Vector3.Lerp(transform.position, pTarget.position, Time.deltaTime * pFadeIn);
            transform.rotation = Quaternion.Lerp(transform.rotation, pTarget.rotation, Time.deltaTime * pFadeIn * pRotSpeed);
            yield return null;
        }

        if (pChirp)
        {
            SendChirp = true;
            Chirp();
        }

        if (pReturnDelay > 0)
            yield return new WaitForSeconds(pReturnDelay);

        ended.Invoke();
        pecking = false;

        Pickup();

        while (Vector3.Distance(transform.localPosition, Vector3.zero) > 0.05f || Quaternion.Dot(transform.localRotation, Quaternion.identity) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * pFadeOut);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * pFadeOut);
            yield return null;
        }
    }

    private void Chirp()
    {
        foreach (Collider other in Physics.OverlapSphere(transform.position, 3))
        {
            Chirpable chirpable = other.GetComponent<Chirpable>();
            if (chirpable != null)
                chirpable.Trigger();
        }

        Chirped.Invoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (holdingPeck != null && PecksList._instance.peckables.ContainsKey(holdingPeck.ID()))
            {
                stream.SendNext(holdingPeck.ID());
            }
            else
            {
                stream.SendNext(99999);
            }

            stream.SendNext(SendChirp);
            SendChirp = false;
        }
        else
        {
            int index = (int)stream.ReceiveNext();
            bool alreadyHolding = false;

            // If holding something
            if (index != 99999)
            {
                IPeckable pecked = PecksList._instance.peckables[index];
                if (holdingPeck == pecked)
                    alreadyHolding = true; // If already holding it
                else
                    holdingPeck = pecked; // else
            }
            // If not holding anything
            else
            {
                // If you are holding then drop it
                if (holding != null)
                {
                    holdingPeck.Released();
                    holding.SetParent(null);
                    holdingPeck = null;
                    holding = null;
                }
            }

            // If you started holding something, run pickup code
            if (holdingPeck != null && alreadyHolding == false)
            {
                holding = holdingPeck.Pecked();
                holding.SetParent(transform);
                holding.localPosition = new Vector3(0, 0, 0.85f);
            }

            if ((bool)stream.ReceiveNext())
                Chirp();
        }
    }

    private void Pickup()
    {
        if (curTarget != null)
        {
            holding = curTarget.Pecked();
            holdingPeck = curTarget.GetThis();
            if (holding != null)
            {
                holding.SetParent(transform);
                holding.localPosition = new Vector3(0, 0, 0.85f);
            }

            curTarget = null;
        }
    }
}
