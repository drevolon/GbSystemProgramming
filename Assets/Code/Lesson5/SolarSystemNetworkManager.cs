using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        [SerializeField] private string _playerName;
        [SerializeField] TMP_InputField _playerNameShip;
        
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            SpawnPlayer(conn, playerControllerId);
        }

        public void SpawnPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            player.GetComponent<ShipController>().PlayerName = _playerNameShip.ToString();
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}