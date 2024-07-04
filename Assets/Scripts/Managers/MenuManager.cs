using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void OpenLeaderboard()
    {
        Debug.Log("Leaderboards");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}