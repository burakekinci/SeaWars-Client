using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUpgrade : MonoBehaviour,IUpgrade
{
    //
    //  UI'daki elementleri set et, bu sınıf UI'da kullanılacak... 
    //

    [SerializeField]
    private int upgradeItemId = 1;

    [SerializeField]
    private int upgradeItemLevel;

    [SerializeField]
    private int upgradePrice = 500;

    [SerializeField]
    private Text upgradeLabel;

    public GameObject upgradeMenuObject;
    
    
    public GameObject[] levels;

    private UpgradeMenu upgradeMenu;
    private InventoryMenu inventoryMenu;
  
    private void OnEnable() {
        InitProperties();
        Debug.Log("init girdi");
    }

    private void OnDisable() {
        foreach(var i in levels){
            i.SetActive(false);
            upgradeLabel.text = "UPGRADE 500$";
            transform.Find("UpgradeButton").GetComponent<Button>().interactable = true;
        }
    }

    private void Start() {
        upgradeLabel = transform.Find("UpgradeButton").Find("Upgrade_Label").GetComponent<Text>();
        upgradeMenu = upgradeMenuObject.GetComponent<UpgradeMenu>();
    }
    
    public void InitProperties(){
        upgradeItemLevel = upgradeMenu.GetUpgradeItemLevelById(upgradeItemId);
        //upgradeMenu.GetUpgradeItemLevelById(upgradeItemId);
        inventoryMenu =
            GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        
        for(int i=0;i<upgradeItemLevel;i++)
        {
            levels[i].SetActive(true);
        }

        if(upgradeItemLevel==3)
        {
            //artık yapılacak upgrade kalmamış demektir.
            upgradeLabel.text = "FULL";
            transform.Find("UpgradeButton").GetComponent<Button>().interactable = false;
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
