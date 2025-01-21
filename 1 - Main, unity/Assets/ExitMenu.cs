using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        // Exit the application
        Debug.Log("Exiting the game..."); // Useful for testing in the editor
        Application.Quit();
    }
}
