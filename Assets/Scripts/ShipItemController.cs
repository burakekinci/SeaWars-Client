using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipItemController : MonoBehaviour
{
    private List<Ship> ships;
    
    [SerializeField]
    private Text itemStatusText, itemShipName;
    private Sprite itemShipSprite;
    public int itemId;
    private float shipPrice;
    private bool itemStatus;

    private InventoryMenu inventoryMenu;

    private void Awake() {
        itemShipName = transform.Find("ShipName_Label").GetComponent<Text>();
        itemStatusText = transform.Find("ShipItemStatus/ShipStatus_Label").GetComponent<Text>();
        inventoryMenu = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        shipPrice = GameStats.Instance.ShipPrices[itemId];        
    }

    private void OnEnable() {
        //read data from playerstats singleton
        
        
    }

    private void Start() {
        SetShipProperties();
    }
    void SetShipProperties(){
        /* if( ){
            itemShipName.SetText(inventoryMenu.GetShipName(itemId));
            //TODO: set image sprite

            if(inventoryMenu.GetShipStatus(itemId)){
                itemStatus=true;
                itemStatusText.SetText("EQUİPED");
            }else{
                itemStatus=false;
                itemStatusText.SetText("BUY (" + shipPrice+ "$)");
            }
        } */
        ships = inventoryMenu.tmpPlayerShips;
        foreach(var ship in ships){
            if(ship.id.Equals(itemId)){
                itemShipName.text = ship.name;
                //todo set image sprite
                if(ship.isBought){
                    itemStatus = true;
                    itemStatusText.text = "EQUİPED";
                }else{
                    itemStatus=false;
                    itemStatusText.text = "BUY (" + shipPrice+ "$)";
                }
                break;
            }
        }
    }

    public void OnClick_Buy(){
        Debug.Log(inventoryMenu.tmpPlayerMoney + " " + shipPrice);
        if(inventoryMenu.tmpPlayerMoney>= shipPrice && !itemStatus){
            itemStatusText.text = "EQUİPED";
            inventoryMenu.SetMoney(shipPrice);
            inventoryMenu.SetShipStatus(itemId,true);
            Debug.Log("alindi");
        }else if(inventoryMenu.tmpPlayerMoney<shipPrice && !itemStatus){
            Debug.Log("Yeterli Para yok");
        }
    }

}
