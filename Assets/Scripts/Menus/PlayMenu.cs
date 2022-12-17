using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MAIN_MENU, gameObject);
    }

    public void OnClick_CreateGame()
    {
        SceneManager.LoadScene("Multiplayer");
        MenuManager.CloseMenu (gameObject);
        GameObject.Find("Canvas").SetActive(false);
    }
}
