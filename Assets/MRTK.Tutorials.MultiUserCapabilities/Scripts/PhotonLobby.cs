using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class PhotonLobby : MonoBehaviourPunCallbacks
    {
        public static PhotonLobby Lobby;

        public string roomName = "default";
        public MixedRealityKeyboard mrkeyboard;

        private int roomNumber = 1;
        private int userIdCount;

        private void Awake()
        {
            if (Lobby == null)
            {
                Lobby = this;
            }
            else
            {
                if (Lobby != this)
                {
                    Destroy(Lobby.gameObject);
                    Lobby = this;
                }
            }

            DontDestroyOnLoad(gameObject);

            mrkeyboard = GetComponent<MixedRealityKeyboard>();

            GenericNetworkManager.OnReadyToStartNetwork += StartNetwork;
        }
        public void GoOnline()
        {
            var randomUserId = Random.Range(0, 999);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = randomUserId.ToString();
            userIdCount++;
            PhotonNetwork.NickName = PhotonNetwork.AuthValues.UserId;
            Debug.Log("Going Online");

            //string defaultName = "room_" + Random.Range(1000, 9999) + "_" + Random.Range(1000, 9999);
            string defaultName = roomName;

            if (Application.isEditor)
            {
                Debug.Log("we are inside editor, connecting to " + defaultName);
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsVisible = false;
                PhotonNetwork.JoinOrCreateRoom(defaultName, roomOptions, TypedLobby.Default);

            }
            else
            {
                mrkeyboard.ShowKeyboard(defaultName);
                Debug.Log(mrkeyboard);
            }
            

        }

        public void Connect()
        {
            Debug.Log("Typing done! Room Name=" + mrkeyboard.Text);
            roomName = mrkeyboard.Text;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);

        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            Debug.Log("\nPhotonLobby.OnJoinedRoom()");
            Debug.Log("Current room name: " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Other players in room: " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("Total players in room: " + (PhotonNetwork.CountOfPlayersInRooms + 1));
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("\nPhotonLobby.OnCreateRoomFailed()");
            Debug.LogError("Creating Room Failed");
            CreateRoom();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            roomNumber++;
        }

        public void OnCancelButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void StartNetwork()
        {
            PhotonNetwork.ConnectUsingSettings();
            Lobby = this;
        }

        private void CreateRoom()
        {
            var roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
            PhotonNetwork.CreateRoom("Room" + Random.Range(1, 3000), roomOptions);
        }
    }
}
