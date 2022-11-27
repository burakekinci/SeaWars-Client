using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] public float tmpPlayerMoney{get;  set;}
    [SerializeField] private TMP_Text playerMoneyText;

    public List<Ship> tmpPlayerShips{get; set;} 

    private void Awake() {
        playerMoneyText = transform.Find("MoneyInt_Label").GetComponent<TMP_Text>();
    }

    public void OnClick_Back(){
        MenuManager.OpenMenu(Menu.MAIN_MENU,gameObject);
    }

    private void OnEnable() {
        //TODO: refresh the playerstats via API
        tmpPlayerMoney = PlayerStats.Instance.GetUpdatedPlayerMoney();
        tmpPlayerShips = PlayerStats.Instance.GetUpdatedPlayerShips();
        Debug.Log(tmpPlayerMoney);
        SetMoney();
        Debug.Log("pencere acildi");
    }

    private void OnDisable() {
        Debug.Log("pencere kapandi");
        if(tmpPlayerMoney != PlayerStats.Instance.PlayerMoney){
            //TODO: send update request to the cloud money
            PlayerStats.Instance.UpdateLocalPlayerMoney(tmpPlayerMoney);
            Debug.Log("Para degisti " + tmpPlayerMoney);
        }

        if(tmpPlayerShips != PlayerStats.Instance.LocalPlayerShips){
            //TODO: send update request to the cloud owned ships
            PlayerStats.Instance.UpdateLocalPlayerShips(tmpPlayerShips);
            Debug.Log("Gemi sahipliği değişti");
        }
    }

    public void SetMoney(){
        playerMoneyText.SetText(tmpPlayerMoney.ToString()+'$');
    }

    public void SetMoney(float price){
        tmpPlayerMoney -= price;
        playerMoneyText.SetText(tmpPlayerMoney.ToString()+'$');
    }

    public string GetShipName(int buttonId){
        return tmpPlayerShips.Find(item=> item.id == buttonId).name;
    }

    public bool GetShipStatus(int buttonId){
        return tmpPlayerShips.Find(item=> item.id == buttonId).isBought;
    }

    public void SetShipStatus(int buttonId, bool newStatus){
        tmpPlayerShips.Find(item=> item.id==buttonId).isBought = newStatus;
    }


}
