using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnGround : MonoBehaviour
{
    [SerializeField] private float CheckDistanceDown = 1;
    [SerializeField] private float CheckDistanceUp = 1;
    [SerializeField] private LayerMask groundLayer;
    public bool OnIce = false;

    private bool grounded = true;

    public UnityEvent Landed;
    public UnityEvent Jumped;

    [SerializeField] private PlayerRespawn respawn;

    [SerializeField] private PhotonView view;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * CheckDistanceUp, transform.position - Vector3.up * CheckDistanceDown, out hit, groundLayer))
        {
            if (grounded == false)
            {
                OnIce = hit.collider.tag == "Ice";
                grounded = true;
                Landed.Invoke();
            }
        }
        else
        {
            if (grounded == true)
            {
                grounded = false;
                Jumped.Invoke();
            }

            if (view.IsMine && transform.position.y < -20)
                respawn.Respawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up * CheckDistanceUp, transform.position - Vector3.up * CheckDistanceDown);
    }
}
