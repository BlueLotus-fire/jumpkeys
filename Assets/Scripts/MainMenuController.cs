
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text highestScoreText;

    private void Start()
    {
        int highestScore = PlayerPrefs.GetInt("HighestScore", 0);

        highestScoreText.text = "HIGHEST SCORE: " + highestScore;
    }

    public void Play()
    {
        SceneManager.LoadScene("GAMEPLAY");
    }
}
