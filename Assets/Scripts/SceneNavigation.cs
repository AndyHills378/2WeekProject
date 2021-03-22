using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("FloodedGrounds");
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }   
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void GameDescription()
    {
        SceneManager.LoadScene("GameDescription");
    }
    public void ExitGame()
    {
        Application.Quit(1);
    }
}
