using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void MenuButton()
    {
        Invoke("toMenu", 0f);
    }

    private void toMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
