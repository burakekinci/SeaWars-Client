using UnityEngine;

public static class MenuManager 
{
    public static GameObject mainMenu, settingsMenu, inventoryMenu, playMenu;
    public static bool IsInıtialized { get; private set;}

    public static void Init(){
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        settingsMenu = canvas.transform.Find("SettingsMenu").gameObject;
        inventoryMenu = canvas.transform.Find("InventoryMenu").gameObject;
        playMenu = canvas.transform.Find("PlayMenu").gameObject;

        IsInıtialized = true;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu){
        
        if(!IsInıtialized)
            Init();

        switch(menu)
        {
            case Menu.MAIN_MENU:
                mainMenu.SetActive(true);
                break;
            case Menu.PLAY:
                playMenu.SetActive(true);
                break;
            case Menu.SETTINGS:
                settingsMenu.SetActive(true);
                break;
            case Menu.INVENTORY:
                inventoryMenu.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);

    }

    public static void QuitGame(){
        Application.Quit();
        Debug.Log("Game Closed via Quit...");
    }

}
