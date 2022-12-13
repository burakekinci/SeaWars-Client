using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipItemController : MonoBehaviour
{
    //private List<Ship> ships;
    
    [SerializeField]
    private Text itemStatusText, itemShipName;
    private Sprite itemShipSprite;
    public int itemId;
    private float shipPrice;
    private bool itemIsBoughtStatus;
    
    [SerializeField] int selectedItemId;

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
        /* ships = inventoryMenu.tmpPlayerShips;
        foreach(var ship in ships){
            if(ship.id.Equals(itemId)){
                itemShipName.text = ship.name;
                //todo set image sprite
                if(ship.isBought){
                    itemStatus = ItemStatus.BOUGHT;
                    itemStatusText.text = "EQUİPED";
                }else{
                    itemStatus= ItemStatus.NONBOUGHT;
                    itemStatusText.text = "BUY (" + shipPrice+ "$)";
                }
                break;
            } 
        }*/

        itemShipName.text = inventoryMenu.GetShipName(itemId);
        if(inventoryMenu.GetShipIsBoughtStatus(itemId) && !inventoryMenu.GetShipIsSelectedStatus(itemId)){
            itemStatusText.text = "SELECT";
        }else if(inventoryMenu.GetShipIsSelectedStatus(itemId)){
            itemStatusText.text = "EQUIPPED";
        }else{
            itemStatusText.text = "BUY (" + shipPrice+ "$)";
        }        

    }

    public void OnClick_BuyOrSelect(){
        Debug.Log(inventoryMenu.tmpPlayerMoney + " " + shipPrice);
        if(inventoryMenu.tmpPlayerMoney>= shipPrice && !inventoryMenu.GetShipIsBoughtStatus(itemId)){
            inventoryMenu.SetShipIsBoughtStatus(itemId,true);
            selectedItemId = itemId;
            inventoryMenu.SetMoney(shipPrice);
            Debug.Log("===GEMİ SATIN ALINDI===");
        }else if(inventoryMenu.tmpPlayerMoney<shipPrice && !inventoryMenu.GetShipIsBoughtStatus(itemId)){
            Debug.Log("===SATIN ALMAK İÇİN YETERLİ PARA YOK===");
        }else if(inventoryMenu.GetShipIsBoughtStatus(itemId)){
            inventoryMenu.SetShipIsSelectedStatus(itemId,true);   
        }
        SetShipProperties();
    }

    

}
