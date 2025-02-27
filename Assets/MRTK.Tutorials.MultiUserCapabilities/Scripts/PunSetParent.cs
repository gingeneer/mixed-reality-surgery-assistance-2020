﻿using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using Photon.Pun;

public class PunSetParent : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private string parentName = "";

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // we need to set this data only if we receive the screws from PUN, 
        // if we are master, this is set in the ScrewSceneController.cs
        if (!PhotonNetwork.IsMasterClient)
        {
            object[] data = info.photonView.InstantiationData;
            Debug.Log($"screw tag is {this.gameObject.tag}");
            Debug.Log($"screw name is {this.gameObject.name}");
            if (data != null && data.Length == 3)
            {
                this.parentName = (string)data[0];
                this.gameObject.tag = (string)data[1];
                this.gameObject.name = (string)data[2];
                Debug.Log($"set screw tag to {this.gameObject.tag}");
                Debug.Log($"set screw name to {this.gameObject.name}");
            }
            this.gameObject.GetComponent<BoundsControl>().enabled = false;
            this.gameObject.GetComponent<ObjectManipulator>().enabled = false;
            this.gameObject.GetComponent<ScaleConstraint>().enabled = false;
            this.gameObject.GetComponent<WholeScaleConstraint>().enabled = false;
            this.gameObject.GetComponent<PositionConstraint>().enabled = false;
            this.gameObject.GetComponent<NearInteractionGrabbable>().enabled = false;
            if (transform.parent == null && parentName != "")
            {
                //Debug.Log("setting parent of " + this.gameObject.name + " to " + parentName);
                if (GameObject.Find(parentName))
                {
                    //Debug.Log("found parent object, setting it...");
                    transform.parent = GameObject.Find(parentName).transform;
                }
            }
            GameObject screwSceneController = GameObject.Find("Controllers/ScrewSceneController");
            if (screwSceneController != null)
            {
                screwSceneController.GetComponent<ScrewSceneController>().RemoteScrewLoadedCallback(this.gameObject);
            }
        }
       
        
    }
    void Update()
    {
        

    }
}
