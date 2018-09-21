using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInCanvas : MonoBehaviour {

    public GameObject tutorialCanvas;
    public Text gameOver;

	void Start () {
        StartCoroutine(ActiveTutorialCanvas());
	}

    IEnumerator ActiveTutorialCanvas()
    {
        gameOver.enabled = false;
        yield return new WaitForSeconds(1.5f);
        tutorialCanvas.GetComponent<Canvas>().enabled = true;
        tutorialCanvas.GetComponent<Tutorial>().enabled = true;
        StopAllCoroutines();
    }
}
