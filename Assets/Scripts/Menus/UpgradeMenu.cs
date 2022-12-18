using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private int shipItemId;

    private List<Ship> tmpPlayerShips;
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
        Debug.Log($"Upgrade Menu {shipItemId} id'li gemiden açıldı");
    }

    private void OnDisable() {
        shipItemId=0;
    }

    public int GetUpgradeItemLevelById(int upgradeItemId)
    {
        Debug.Log(shipItemId + " idli geminin " + upgradeItemId + " idli itemi geldi..");
        return tmpPlayerShips.Find(item => item.id == shipItemId).upgrades.Find(i=> i.id == upgradeItemId).level;
    }

    public void SetUpgradeItemLevelById(int upgradeItemId)
    {
        tmpPlayerShips.Find(item => item.id == shipItemId).upgrades.Find(i=> i.id == upgradeItemId).level++;
    }
}
