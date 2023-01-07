using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;

    //UI elements
    public Text lobbyNameText;

    //Player Data
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    //Other Data
    public ulong CurrentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    //Ready
    public Button startGameButton;
    public Text readyButtonText;


    //Manager
    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager =
                CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ReadyPlayer()
    {
        localPlayerController.ChangeReady();
    }

    public void UpdateButton()
    {
        if(localPlayerController.Ready)
        {
            readyButtonText.text = "Not Ready";
        }
        else
        {
            readyButtonText.text = "Go Ready!";
        }
    }

    public void CheckIfAllReady()
    {
        bool allReady = false;

        foreach(PlayerObjectController player in Manager.GamePlayers)
        {
            if(player.Ready)
            {
                allReady = true;
            }
            else
            {
                allReady = false;
                break;
            }
        }

        if(allReady)
        {
            if(localPlayerController.PlayerIdNumber == 1)
            {
                startGameButton.interactable = true;
            }
            else
            {
                startGameButton.interactable = false;
            }
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    public void UpdateLobbyName()
    {
        CurrentLobbyID = manager.GetComponent<SteamLobby>().CurrentLobbyID;
        lobbyNameText.text =
            SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if (!playerItemCreated)
        {
            CreateHostPlayerItem();
        }

        if (playerListItems.Count < Manager.GamePlayers.Count)
        {
            CreateClientPlayerItem();
        }

        if (playerListItems.Count > Manager.GamePlayers.Count)
        {
            RemovePlayerItem();
        }

        if (playerListItems.Count == Manager.GamePlayers.Count)
        {
            UpdatePlayerItem();
        }
    }

    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController =
            localPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            GameObject newPlayerItem =
                Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem newPlayerItemScript =
                newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.PlayerName = player.PlayerName;
            newPlayerItemScript.ConnectionID = player.ConnectionID;
            newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            newPlayerItemScript.ready = player.Ready;
            newPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            playerListItems.Add (newPlayerItemScript);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (!playerListItems.Any(p => p.ConnectionID == player.ConnectionID))
            {
                GameObject newPlayerItem =
                    Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript =
                    newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.PlayerName = player.PlayerName;
                newPlayerItemScript.ConnectionID = player.ConnectionID;
                newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                newPlayerItemScript.ready = player.Ready;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem
                    .transform
                    .SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;

                playerListItems.Add (newPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            foreach(PlayerListItem playerListItemScript in playerListItems)
            {
                if(playerListItemScript.ConnectionID == player.ConnectionID)
                {
                    playerListItemScript.PlayerName = player.PlayerName;
                    playerListItemScript.ready = player.Ready;
                    playerListItemScript.SetPlayerValues();

                    if(player == localPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

        foreach(PlayerListItem playerListItem in playerListItems)
        {
            if(!Manager.GamePlayers.Any(p=>p.ConnectionID == playerListItem.ConnectionID))
            {
                playerListItemsToRemove.Add(playerListItem);
            }
        }

        if(playerListItemsToRemove.Count>0)
        {
            foreach(PlayerListItem playerListItemToRemove in playerListItemsToRemove)
            {
                GameObject objectToRemove = playerListItemToRemove.gameObject;
                playerListItems.Remove(playerListItemToRemove);
                Destroy(objectToRemove);
                objectToRemove = null;
            }
        }
    }

    public void StartGame(string SceneName)
    {
        localPlayerController.CanStartGame(SceneName);
    }
}
