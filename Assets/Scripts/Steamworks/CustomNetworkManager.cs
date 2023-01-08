using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{

    public static CustomNetworkManager Instance {get; private set;}

    public override void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private PlayerObjectController GamePlayerPrefab;
    public List<PlayerObjectController> GamePlayers {get;} = new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            Transform startPos = GetStartPosition();

            PlayerObjectController GamePlayerInstance = startPos != null
                ? Instantiate(GamePlayerPrefab,startPos.position, startPos.rotation)
                : Instantiate(GamePlayerPrefab);
            
            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);

            GamePlayerInstance.gameObject.GetComponent<ShipController>().enabled = false;
            GamePlayerInstance.gameObject.GetComponent<ShootingController>().enabled = false;
            GamePlayerInstance.gameObject.GetComponent<CameraController>().enabled = false;

        }

        
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == "Multiplayer")
        {
            Debug.Log("sahne değişmiş...");

            Transform startPos = GetStartPosition();

            foreach (var item in GamePlayers)
            {
                item.gameObject.GetComponent<ShipController>().enabled = true;
                item.gameObject.GetComponent<ShootingController>().enabled = true;
                item.gameObject.GetComponent<CameraController>().enabled = true;
            }
            Debug.Log("kontrol izinleri verildi...");
        }

    }

    

    public override void OnClientDisconnect()
    {
        SceneManager.LoadScene(offlineScene, LoadSceneMode.Single);
    }

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }


}
