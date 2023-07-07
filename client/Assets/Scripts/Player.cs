namespace DevelopersHub.RaidingThrones
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Threading;
    public class Player : MonoBehaviour
    {
        public enum RequestsID
        {
            AUTH = 1
        }
        private void Start()
        {
            RealtimeNetworking.OnLongReceived += ReceivedLong;
            ConnectToServer();
        }

        private void ReceivedLong(int id, long value)
        {
            switch (id)
            {
                case 1:
                    Debug.Log("Received long: " + value);
                    break;
            }

        }

        private void ConnectionResponse(bool successful)
        {
            if (successful)
            {
                RealtimeNetworking.OnDisconnectedFromServer += DisconnectedFromServer;
                string device = SystemInfo.deviceUniqueIdentifier;
                Sender.TCP_Send((int)RequestsID.AUTH, device);
            }
            else
            {
                // TODO: connection failded message box retry button
            }
                RealtimeNetworking.OnConnectingToServerResult -= ConnectionResponse;
            
        }

        private void ConnectToServer()
        {
            RealtimeNetworking.OnConnectingToServerResult += ConnectionResponse;
            RealtimeNetworking.Connect();       
        }

        private void DisconnectedFromServer()
        {
            RealtimeNetworking.OnDisconnectedFromServer -= DisconnectedFromServer;
            // TODO: Connection failed message box with retry button
        }
    }
}