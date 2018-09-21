using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public Image fadeOut;
    public Canvas fadeOutCanvas;
    public Button btnStart;

    void Start()
    {
        fadeOutCanvas.enabled = false;
        btnStart.onClick.AddListener(SwitchScene);
    }

    public void SwitchScene()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        fadeOutCanvas.enabled = true;
        fadeOut.GetComponent<Animator>().SetBool("GamePlay", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scene_1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
