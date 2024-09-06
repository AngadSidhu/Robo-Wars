using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void PlayGame()
    {
        Invoke("Plays", 1f);
    }

    private void Plays()
    {
        SceneManager.LoadScene("Test");
    }
}
