using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private int shipItemId;

    private List<Ship> tmpPlayerShips;
    
    [SerializeField]
    GameObject healthUpgrade;
    
    [SerializeField]
    GameObject engineUpgrade;
    
    [SerializeField]
    GameObject weaponUpgrade;

    public void OpenUpgradeMenu(int shipItemId)
    {
        this.shipItemId = shipItemId;
        gameObject.SetActive(true);
    }

    public void OnClick_CloseUpgradeMenu()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        tmpPlayerShips = PlayerStats.Instance.GetUpdatedPlayerShips();
        healthUpgrade.gameObject.SetActive(true);
        engineUpgrade.gameObject.SetActive(true);
        weaponUpgrade.gameObject.SetActive(true);

        Debug.Log($"Upgrade Menu {shipItemId} id'li gemiden açıldı");
    }

    private void OnDisable() {
        shipItemId=0;
    }

    public int GetUpgradeItemLevelById(int upgradeItemId)
    {
        Debug.Log(shipItemId + " idli geminin " + upgradeItemId + " idli itemi geldi..");
        int? result = tmpPlayerShips.Find(item => item.id == shipItemId).upgrades.Find(i=> i.id == upgradeItemId).level; 
        if(result==null)
        {
            Debug.Log("ItemLevel null döndü");
            return 1;
        }else{
            return (int)result;  
        } 
    }

    public void SetUpgradeItemLevelById(int upgradeItemId)
    {
        tmpPlayerShips.Find(item => item.id == shipItemId).upgrades.Find(i=> i.id == upgradeItemId).level++;
    }
}
