using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PunSetParent : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private string parentName = "";

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;
        if (data != null && data.Length == 1)
        {
            this.parentName = (string)data[0];
        }
    }
    void Update()
    {
        if (transform.parent == null && parentName != "")
        {
            Debug.Log("BBBBBBBBBBB setting parent of " + this.gameObject.name + " to " + parentName);
            if (GameObject.Find(parentName))
            {
                Debug.Log("found parent object, setting it...");
                transform.parent = GameObject.Find(parentName).transform;
            }
        }

    }
}
