using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField]
    private InventoryMenu inventoryMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        try
        {
            inventoryMenu =
                GameObject
                    .Find("Canvas")
                    .transform
                    .Find("InventoryMenu")
                    .GetComponent<InventoryMenu>();
            inventoryMenu.ShipSelectEvent += UpdateLocalSelectedPlayerShip;
        }
        catch (System.Exception)
        {
            Debug.Log("PlayerStats, InventoryMenu objesini bulamadı...");
        }
    }

    private void OnDestroy() {
        try
        {
            inventoryMenu =
                GameObject
                    .Find("Canvas")
                    .transform
                    .Find("InventoryMenu")
                    .GetComponent<InventoryMenu>();
            inventoryMenu.ShipSelectEvent -= UpdateLocalSelectedPlayerShip;
        }
        catch (System.Exception)
        {
            Debug.Log("PlayerStats, InventoryMenu objesini bulamadı...");
        }
    }

    [SerializeField]
    private string _playerName = "Player1";

    public string PlayerName
    {
        get
        {
            return _playerName;
        }
        private
        set
        {
        }
    }

    [SerializeField]
    ShipType _selectedShipType = ShipType.CORVETTE;

    public ShipType SelectedShipType
    {
        get
        {
            return _selectedShipType;
        }
        private
        set
        {
        }
    }

    [SerializeField]
    private float _localPlayerMoney = 3500f;

    public float PlayerMoney
    {
        get
        {
            return _localPlayerMoney;
        }
        private
        set
        {
        }
    }

    public void UpdateLocalPlayerMoney(float newMoney)
    {
        _localPlayerMoney = newMoney;
    }

    private List<Ship>
        _localPlayerShips =
            new List<Ship> {
                new Ship {
                    id = 1,
                    name = "Corvet",
                    isBought = true,
                    isSelected = false
                },
                new Ship {
                    id = 2,
                    name = "Corvet Green",
                    isBought = false,
                    isSelected = false
                },
                new Ship {
                    id = 3,
                    name = "Frigate",
                    isBought = true,
                    isSelected = false
                },
                new Ship {
                    id = 4,
                    name = "Frigate Green",
                    isBought = false,
                    isSelected = false
                }
            };

    public List<Ship> LocalPlayerShips
    {
        get
        {
            return _localPlayerShips;
        }
        private
        set
        {
        }
    }

    public void UpdateLocalSelectedPlayerShip()
    {
        //Set selected ship type
        int index = 1;
        foreach (var ship in _localPlayerShips)
        {
            if (ship.isSelected)
            {
                switch (index)
                {
                    case 1:
                        _selectedShipType = ShipType.CORVETTE;
                        break;
                    case 2:
                        _selectedShipType = ShipType.CORVETTE_GREEN;
                        break;
                    case 3:
                        _selectedShipType = ShipType.FRIGATE;
                        break;
                    case 4:
                        _selectedShipType = ShipType.FRIGATE_GREEN;
                        break;
                    default:
                        break;
                }
            }
            index++;
        }
    }

    private void GetAPIData()
    {
    }

    public float GetUpdatedPlayerMoney()
    {
        GetAPIData();
        return _localPlayerMoney;
    }

    public List<Ship> GetUpdatedPlayerShips()
    {
        GetAPIData();
        return _localPlayerShips;
    }
}
