using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void PlayGame()
    {
        Invoke("Plays", 1f);
    }

    public void ExitGame()
    {
        Invoke("Exits", 1f);
    }

    private void Plays()
    {
        SceneManager.LoadScene("Test");
    }

    private void Exits()
    {
        Application.Quit();
    }
}
