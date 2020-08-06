using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    public UnityEvent Collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            Collected.Invoke();
            other.GetComponentInParent<PlayerMovement>().collectables++;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 5 * Time.deltaTime, 0, Space.World);
    }
}
