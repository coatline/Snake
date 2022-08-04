
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    //private static GameHandler instance;
    private static int score;
    public static bool wallMode = false;
    public static Color snakeColor = Color.green;

    [SerializeField] Snake snake;
    [SerializeField] TMP_Text startText;

    private LevelGrid levelGrid;

    public static int Score => score;
 
    //public static GameHandler I => instance;

    private void Awake()
    {
        //instance = this;
        var current = SceneManager.GetActiveScene();
        startText.enabled = true;
        if (current.buildIndex == 1)
        {
            Time.timeScale = 0;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Time.timeScale = 1;
            startText.enabled = false;
        }
    }

    void Start()
    {

        levelGrid = new LevelGrid(20, 20);
        if (snake != null)
        {
            snake.Setup(levelGrid, snakeColor);
            levelGrid.Setup(snake);
        }
    }

    public static void  Reset() => score = 0;

    public static void AddScore(int amount = 1)
    {
        score += amount;
    }

}
