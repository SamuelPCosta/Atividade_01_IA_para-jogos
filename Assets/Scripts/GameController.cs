using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private TextMeshProUGUI life;
    [SerializeField] private TextMeshProUGUI bullet;
    [SerializeField] private int lifeRecover = 20;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject endGame;
    [SerializeField] private TextMeshProUGUI timerEndGame;
    [SerializeField] private Transform enemies;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject youWinText;

    public int DamageCaused = 20;
    public int DamageSuffered = 20;

    private float elapsedTime;
    private PlayerController player;

    public Slider LifeSlider => lifeSlider;
    public int LifeRecover => lifeRecover;

    private void Start()
    {
        life.text = "0";
        bullet.text = "0";
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        Time.timeScale = 1f;

        endGame.SetActive(false);
        gameOverText.SetActive(false);
        youWinText.SetActive(false);

        elapsedTime = 0f;
    }

    private void Update()
    {
        if (player != null && player.currentLife <= 0)
        {
            if (!endGame.activeSelf)
            {
                endGame.SetActive(true);
                gameOverText.SetActive(true);
                youWinText.SetActive(false);
                timerEndGame.text = timerText.text;
                Time.timeScale = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Level");
            }
            return;
        }

        if (enemies != null && enemies.childCount == 0)
        {
            if (!endGame.activeSelf)
            {
                endGame.SetActive(true);
                youWinText.SetActive(true);
                gameOverText.SetActive(false);
                timerEndGame.text = timerText.text;
                Time.timeScale = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Level");
            }
            return;
        }

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    public void UpdateUI(int lifes, int bullets, float currentLife)
    {
        life.text = $"{lifes}";
        bullet.text = $"{bullets}";
        lifeSlider.value = currentLife;
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
