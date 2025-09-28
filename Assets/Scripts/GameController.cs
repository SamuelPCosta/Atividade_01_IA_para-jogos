using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private TMP_Text life;
    [SerializeField] private TMP_Text bullet;
    [SerializeField] private int lifeRecover = 20;
    public int DamageCaused = 20;
    public int DamageSuffered = 20;

    public Slider LifeSlider => lifeSlider;
    public int LifeRecover => lifeRecover;

    private void Start()
    {
        life.text = ""+0;
        bullet.text = "" + 0;
    }

    public void UpdateUI(int lifes, int bullets, float currentLife)
    {
        life.text = $"{lifes}";
        bullet.text = $"{bullets}";
        lifeSlider.value = currentLife;
    }
}