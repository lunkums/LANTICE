using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private bool paused;

    public static PauseMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            return;

        paused = false;
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        pauseCanvas.SetActive(paused);
    }

    public void Quit()
    {
        Application.Quit();
    }    
}
