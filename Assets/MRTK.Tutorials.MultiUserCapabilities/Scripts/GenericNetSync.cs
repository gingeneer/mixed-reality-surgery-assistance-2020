using Photon.Pun;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class GenericNetSync : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private bool isUser = default;

        private Camera mainCamera;

        private Vector3 networkLocalPosition;
        private Quaternion networkLocalRotation;
        private Vector3 networkLocalScale;
        private Transform networkParent;

        private Vector3 startingLocalPosition;
        private Quaternion startingLocalRotation;
        private Vector3 startingLocalScale;
        private Transform startingParent;

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext(transform.localScale);
                stream.SendNext(transform.parent);
            }
            else
            {
                networkLocalPosition = (Vector3) stream.ReceiveNext();
                networkLocalRotation = (Quaternion) stream.ReceiveNext();
                networkLocalScale = (Vector3) stream.ReceiveNext();
                networkParent = (Transform)stream.ReceiveNext();
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;

            if (isUser)
            {
                if (TableAnchor.Instance != null) transform.parent = FindObjectOfType<TableAnchor>().transform;

                if (photonView.IsMine) GenericNetworkManager.Instance.localUser = photonView;
            }

            var trans = transform;
            startingLocalPosition = trans.localPosition;
            startingLocalRotation = trans.localRotation;
            startingLocalScale = trans.localScale;
            startingParent = trans.parent;

            networkLocalPosition = startingLocalPosition;
            networkLocalRotation = startingLocalRotation;
            networkLocalScale = startingLocalScale;
            networkParent = startingParent;
        }

        // private void FixedUpdate()
        private void Update()
        {
            
            if (!photonView.IsMine)
            {
                var trans = transform;
                trans.localPosition = networkLocalPosition;
                trans.localRotation = networkLocalRotation;
                trans.localScale = networkLocalScale;
                if (transform.parent == null && !isUser)
                {
                    trans.parent = networkParent;
                }
            }

            if (photonView.IsMine && isUser)
            {
                var trans = transform;
                var mainCameraTransform = mainCamera.transform;
                trans.position = mainCameraTransform.position;
                trans.rotation = mainCameraTransform.rotation;
                trans.localScale = mainCameraTransform.localScale;
            }
        }
    }
}
