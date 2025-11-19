using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    [SerializeField] TextMeshProUGUI gameOverText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Scenes/Main Menu");
    }

    public void updateScore()
    {
        float finalScore = ScoreScript.Instance.GetScore;
        gameOverText.text = finalScore.ToString();
    }
}
