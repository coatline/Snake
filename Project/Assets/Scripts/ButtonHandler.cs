using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour
{
    public ColorChanger cc;

    public void ChangeTheScene(int scene)
    {
        var current = SceneManager.GetActiveScene();
        if (scene == 1 && current.buildIndex == 0)
        {
            EventSystem.current.enabled = false;
            StartCoroutine(SceneChanger(scene, 0.3f));
        }
        else
        {
            StartCoroutine(SceneChanger(scene, 0));
        }
    }

    IEnumerator SceneChanger(int scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
        var amount = GameHandler.Score;
        GameHandler.Reset();
        Time.timeScale = 1;
    }

    public void Pause(Button pauseLayout)
    {
        Time.timeScale = 0;
        pauseLayout.gameObject.SetActive(true);
    }

    public void UnPause(Button self)
    {
        Time.timeScale = 1;
        self.gameObject.SetActive(false);
    }

    public void SetMode(Toggle toggle)
    {
        if (toggle.isOn)
        {
            GameHandler.wallMode = true;
        }
        else
        {
            GameHandler.wallMode = false;
        }
    }

    public void SetColor(int direction)
    {
        cc.ChangeColor(direction);

        print(direction);
    }
}
