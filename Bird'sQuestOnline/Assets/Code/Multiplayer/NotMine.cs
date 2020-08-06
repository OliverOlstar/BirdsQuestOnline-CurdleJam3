using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotMine : MonoBehaviour
{
    void Start()
    {
        PhotonView view = GetComponent<PhotonView>();
        if (view.IsMine == false)
        {
            foreach(INotMine nm in GetComponents<INotMine>())
                nm.Remove();

            foreach (INotMine nm in GetComponentsInChildren<INotMine>())
                nm.Remove();
        }

        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
