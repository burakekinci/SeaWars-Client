using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryMenu : MonoBehaviour
{
    [SerializeField]
    public float tmpPlayerMoney { get; set; }

    [SerializeField]
    private Text playerMoneyText;

    public event Action ShipSelectEvent;

    public List<Ship> tmpPlayerShips { get; set; }

    private void Awake()
    {
        playerMoneyText = transform.Find("MoneyInt_Label").GetComponent<Text>();
    }

    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MAIN_MENU, gameObject);
    }

    private void OnEnable()
    {
        //TODO: refresh the playerstats via API
        tmpPlayerMoney = PlayerStats.Instance.GetUpdatedPlayerMoney();
        tmpPlayerShips = PlayerStats.Instance.GetUpdatedPlayerShips();
        SetMoney();
    }

    private void OnDisable()
    {
        if (tmpPlayerMoney != PlayerStats.Instance.PlayerMoney)
        {
            //TODO: send update request to the cloud money
            PlayerStats.Instance.UpdateLocalPlayerMoney (tmpPlayerMoney);
            Debug.Log("Para degisti " + tmpPlayerMoney);
        }
    }

    public void SetMoney()
    {
        playerMoneyText.text = tmpPlayerMoney.ToString() + '$';
    }

    public void SetMoney(float price)
    {
        tmpPlayerMoney -= price;
        playerMoneyText.text = tmpPlayerMoney.ToString() + '$';
    }

    public string GetShipName(int buttonId)
    {
        return tmpPlayerShips.Find(item => item.id == buttonId).name;
    }

    public bool GetShipIsBoughtStatus(int buttonId)
    {
        return tmpPlayerShips.Find(item => item.id == buttonId).isBought;
    }

    public void SetShipIsBoughtStatus(int buttonId, bool newStatus)
    {
        tmpPlayerShips.Find(item => item.id == buttonId).isBought = newStatus;
    }

    public bool GetShipIsSelectedStatus(int buttonId)
    {
        return tmpPlayerShips.Find(item => item.id == buttonId).isSelected;
    }

    public void SetShipIsSelectedStatus(int buttonId)
    {
        foreach (var ship in tmpPlayerShips)
        {
            ship.isSelected = ship.id == buttonId;
        }

        if (ShipSelectEvent != null)
        {
            ShipSelectEvent.Invoke();
        }
    }

    public void OnClick_Upgrade(int itemId)
    {
        Debug.Log("upgrade menu " + itemId);
    }
}
