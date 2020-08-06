using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoccerGoal : MonoBehaviourPunCallbacks, IPunObservable
{
    public UnityEvent Scored;
    [SerializeField] private Text text;

    [SerializeField] private bool mustBeFalling = false;
    private int value = 0;

    private void Start()
    {
        text.text = "";
        value = -1;
    }

    public void OnStarted()
    {
        text.text = "0";
        value = 0;
    }

    public void OnEnded()
    {
        text.text = "";
        value = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (mustBeFalling == true && other.GetComponent<Rigidbody>().velocity.y > 0)
                return;

            Scored.Invoke();
        }
    }

    public void UpdateText(int pValue)
    {
        if (pValue > -1)
            text.text = pValue.ToString();
        else
            text.text = "";

        value = pValue;
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(value);
        }
        else
        {
            UpdateText((int)stream.ReceiveNext());
        }
    }
}
