using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManagement : MonoBehaviour
{
    PlayerMovement player;

    float timeRemaining;

    [SerializeField] float timeLimit = 300f;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider hungerBar;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] Canvas gameplayUI;
    [SerializeField] Canvas startUI;
    [SerializeField] Canvas pauseUI;
    [SerializeField] Canvas winUI;
    [SerializeField] Canvas loseUI;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        timeRemaining = timeLimit;

        Time.timeScale = 0;
        gameplayUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
        winUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);
        startUI.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        UpdateStats();
        CheckGameOver();

        if (Input.GetKey(KeyCode.Escape))
        {
            OpenMenu();
        }

    }
    void UpdateStats()
    {
        healthBar.value = player.GetHealth();
        hungerBar.maxValue = player.GetMaxHunger();
        hungerBar.value = player.GetHunger();
        timer.text = Mathf.RoundToInt(timeRemaining).ToString();
    }
    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
    }
    void CheckGameOver()
    {
        if(player.GetHealth() <= 0)
        {
            Time.timeScale = 0;
            Lose();
            return;
        }
        if (player.GetHunger() <= 0)
        {
            Time.timeScale = 0;
            Lose();
            return;
        }
        if(timeRemaining <= 0)
        {
            Time.timeScale = 0;
            Win();
            return;
        }
    }
    void Lose()
    {
        Time.timeScale = 0;
        loseUI.gameObject.SetActive(true);
    }

    void Win()
    {
        Time.timeScale = 0;
        winUI.gameObject.SetActive(true);
    }
    public void OpenMenu()
    {
        Time.timeScale = 0;
        pauseUI.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Continue()
    {
        Time.timeScale = 1;
        startUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
