
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MonitorController : MonoBehaviour
{
    [SerializeField] private int passwordLength = 5;
    [SerializeField] private float Deathtimer = 15f;
    [SerializeField] private int rightAnswer = 100;
    [SerializeField] private CapsLock capsLock;
    public static int FinalScore = 0;
    [SerializeField] private TMP_Text targetPasswordText;
    [SerializeField] private TMP_Text typedPasswordText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    private string targetPassword;
    private string typedPassword = "";
    private float timeRemaining;
    private int score;
    private bool capital;
    private bool gameOver;

    private void Start()
    {
        FinalScore = 0;
        score = 0;
        StartRound();
    }

    private void Update()
    {
        if (gameOver)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateScreen();
            EndGame();
            return;
        }

        UpdateScreen();
    }

    public void KeyPressed(string key)
    {
        if (gameOver)
            return;

        switch (key)
        {
            case "CAPSLOCK":
                capital = !capital;
                capsLock.UpdateVisual(capital);
                break;

            case "BACKSPACE":
                if (typedPassword.Length > 0)
                    typedPassword = typedPassword.Substring(
                        0, typedPassword.Length - 1);
                break;

            case "ENTER":
                SubmitPassword();
                break;

            default:
                if (typedPassword.Length < targetPassword.Length)
                    typedPassword += capital ? key.ToUpper() : key.ToLower();
                break;
        }

        UpdateScreen();
    }

    private void StartRound()
    {
        targetPassword = GeneratePassword(passwordLength);
        typedPassword = "";
        capital = false;
        capsLock.UpdateVisual(capital);
        timeRemaining = Deathtimer;

        UpdateScreen();
    }

    private string GeneratePassword(int length)
    {
        const string characters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string password = "";

        for (int i = 0; i < length; i++)
            password += characters[Random.Range(0, characters.Length)];

        return password;
    }

    private void SubmitPassword()
    {
        if (typedPassword != targetPassword)
        {
            EndGame();
            return;
        }

        score += rightAnswer;
        StartRound();
    }

    private void EndGame()
    {
        if (gameOver)
            return;

        gameOver = true;
        FinalScore = score;
        typedPasswordText.color = Color.red;
        int highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        if (score > highestScore)
        {
            PlayerPrefs.SetInt("HighestScore", score);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene("ENDSCREEN");
    }

    private void UpdateScreen()
    {
        targetPasswordText.text = targetPassword;
        typedPasswordText.text = typedPassword + "_";
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        scoreText.text = "SCORE: " + score;

        timerText.color = timeRemaining <= 5f
            ? Color.red
            : Color.white;
    }
}
