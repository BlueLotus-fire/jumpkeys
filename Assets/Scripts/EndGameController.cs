
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System.Collections;

public class EndGameController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject yourScore;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private float scoreDelay = 1.5f;
    [SerializeField] private float blackScreenDelay = 4f;

    [SerializeField] private float menuDelay = 2f;


    private void Start()
    {
        yourScore.SetActive(false);
        finalScore.gameObject.SetActive(false);
        blackScreen.SetActive(false);

        finalScore.text = MonitorController.FinalScore.ToString();

        StartCoroutine(PlayEndSequence());
    }

    private IEnumerator PlayEndSequence()
    {
        videoPlayer.Play();

        while (!videoPlayer.isPlaying)
            yield return null;

        while (videoPlayer.isPlaying)
            yield return null;

        yield return new WaitForSeconds(scoreDelay);

        yourScore.SetActive(true);
        finalScore.gameObject.SetActive(true);

        yield return new WaitForSeconds(blackScreenDelay);

        blackScreen.SetActive(true);

        yield return new WaitForSeconds(menuDelay);

        UnityEngine.SceneManagement.SceneManager.LoadScene("MAINMENU");
    }
}
