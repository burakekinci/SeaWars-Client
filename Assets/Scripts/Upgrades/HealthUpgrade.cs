using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUpgrade : MonoBehaviour,IUpgrade
{
    //
    //  UI'daki elementleri set et, bu sınıf UI'da kullanılacak... 
    //

    [Header("Item Properties")]
    [SerializeField]
    private int upgradeItemId = 1;

    [SerializeField]
    private int upgradeItemLevel;

    [SerializeField]
    private int upgradePrice = 500;


    [Header("UI Elements")]
    [SerializeField]
    private Text upgradeLabel;

    [SerializeField]
    private Button upgradeButton;


    [Header("Hirerarchy Elements")]    
    [SerializeField]
    public GameObject upgradeMenuObject;

    [SerializeField]
    public GameObject inventoryMenuObject;
    public GameObject[] levels;

    
    private UpgradeMenu upgradeMenu;
    private InventoryMenu inventoryMenu;
  

    private void OnEnable() {
        InitProperties();
    }

    private void OnDisable() {
        foreach(var i in levels){
            i.SetActive(false);
            upgradeLabel.text = "UPGRADE 500$";
            upgradeButton.interactable = true;
        }
    }

    private void Awake() {
    }
    
    public void InitProperties(){
        upgradeMenu = upgradeMenuObject.GetComponent<UpgradeMenu>();
        upgradeItemLevel = upgradeMenu.GetUpgradeItemLevelById(upgradeItemId);
        
        //upgradeMenu.GetUpgradeItemLevelById(upgradeItemId);
        inventoryMenu = inventoryMenuObject.GetComponent<InventoryMenu>();
        
        for(int i=0;i<upgradeItemLevel;i++)
        {
            levels[i].SetActive(true);
        }

        if(upgradeItemLevel==3)
        {
            //artık yapılacak upgrade kalmamış demektir.
            upgradeLabel.text = "FULL";
            upgradeButton.interactable = false;
        }
        
    }

    
    //PlayerInstance değişkenlerini ayarla, oyun sahnesinde uygulamayı initializationer yapıyor...
    public void OnClick_Upgrade()
    {
        if(upgradeItemLevel<3 && inventoryMenu.tmpPlayerMoney>= upgradePrice)
        {
            upgradeMenu.SetUpgradeItemLevelById(upgradeItemId);
            inventoryMenu.SetMoney(upgradePrice);
            InitProperties();
        }
        
    }
}
