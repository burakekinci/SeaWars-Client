using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipItemController : MonoBehaviour
{
    //private List<Ship> ships;
    [SerializeField] 
    private int itemId;
    
    [SerializeField] 
    private Text itemStatusText, itemShipName;

    private float shipPrice;
    private bool itemIsBoughtStatus;

   
    public GameObject upgradeMenuObject;
    
    [SerializeField]
    private GameObject upgradeButton;

    [SerializeField]
    private bool isBought;

    [SerializeField] 
    private bool isSelected;

    private InventoryMenu inventoryMenu;

    private void Awake()
    {
        itemShipName = transform.Find("ShipName_Label").GetComponent<Text>();
        itemStatusText =
            transform
                .Find("ShipItemStatus/ShipStatus_Label")
                .GetComponent<Text>();
        inventoryMenu =
            GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        shipPrice = GameStats.Instance.ShipPrices[itemId];
        upgradeButton = transform.Find("ShipItemUpgrade").gameObject;
    }

    private void OnEnable()
    {
        UpdateShipProperties();
        inventoryMenu.ShipSelectEvent += UpdateShipProperties;
    }

    private void OnDisable()
    {
        inventoryMenu.ShipSelectEvent -= UpdateShipProperties;
    }

    private void Start()
    {
        itemShipName.text = inventoryMenu.GetShipName(itemId);
    }

    void UpdateShipProperties()
    {
        isBought = inventoryMenu.GetShipIsBoughtStatus(itemId);
        isSelected = inventoryMenu.GetShipIsSelectedStatus(itemId);

        if (isBought)
        {
            itemStatusText.text = isSelected ? "EQUIPPED" : "SELECT";
            upgradeButton.SetActive(true);
        }
        else
        {
            itemStatusText.text = "BUY (" + shipPrice + "$)";
            upgradeButton.SetActive(false);
        }
    }

    public void OnClick_BuyOrSelect()
    {
        Debug.Log(inventoryMenu.tmpPlayerMoney + " " + shipPrice);
        if (!isBought)
        {
            if (inventoryMenu.tmpPlayerMoney >= shipPrice)
            {
                inventoryMenu.SetShipIsBoughtStatus(itemId, true);
                inventoryMenu.SetMoney (shipPrice);
                Debug.Log($"==={itemId} id'li GEMİ SATIN ALINDI===");
            }
            else
            {
                Debug.Log("===SATIN ALMAK İÇİN YETERLİ PARA YOK===");
            }
        }
        else
        {
            if (!isSelected)
            {
                inventoryMenu.SetShipIsSelectedStatus (itemId);
                Debug.Log($"==={itemId} id'li GEMİ SEÇİLDİ===");
            }
            else
                return;
        }
        UpdateShipProperties();
    }

    public void OnClick_Upgrade()
    {
        UpgradeMenu upgradeMenu = upgradeMenuObject.GetComponent<UpgradeMenu>();
        upgradeMenu.OpenUpgradeMenu(itemId);
    }

}
