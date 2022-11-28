using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Play(){
        MenuManager.OpenMenu(Menu.PLAY,gameObject);
    }

    public void OnClick_Settings(){
        MenuManager.OpenMenu(Menu.SETTINGS,gameObject);
    }

    public void OnClick_Inventory(){
        MenuManager.OpenMenu(Menu.INVENTORY,gameObject);
    }


    public void OnClick_Quit(){
        MenuManager.QuitGame();
    }

}
