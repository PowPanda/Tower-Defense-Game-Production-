using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // Attributes
    public int fireArrowCount;
    public int currentHealth;
    public int totalHealth;
    public bool isDie;

    // Accessed Components
    public Camera fpsCamera;
    public Text text_normalArrowCount;
    public Text text_fireArrowCount;
    public Text text_healthpoint;
    public Text gameOver;
    public SpriteRenderer iconFireArrow;
    public SpriteRenderer iconNormalArrow;

    // Others
    List<SpriteRenderer> listRenderer = new List<SpriteRenderer>();
    
    int iconListIndex;

    // Icon List
    public bool[] iconList = new bool[2] { true, false };

	// Use this for initialization
	void Start () {
        fireArrowCount = 1;
        iconListIndex = 0;
        currentHealth = 1200;
        totalHealth = currentHealth;
        isDie = false;

        listRenderer.Add(iconNormalArrow);
        //listRenderer.Add(iconIceArrow);
        listRenderer.Add(iconFireArrow);
    }

    void Update()
    {
        //text_iceArrowCount.text = ": " + iceArrowCount.ToString();
        text_healthpoint.text = currentHealth.ToString();
        text_fireArrowCount.text = fireArrowCount.ToString();
        text_normalArrowCount.text = "\u221E";

        if (!isDie)
        {
            DefaultArrow();
            //UpdateSelectedArrow();

            if (iconList[0] == true)
            {
                listRenderer[0].color = new Color32(255, 255, 255, 255);
                listRenderer[1].color = new Color32(66, 66, 66, 255);
            }

            else if (iconList[1] == true)
            {
                listRenderer[0].color = new Color32(66, 66, 66, 255);
                listRenderer[1].color = new Color32(255, 255, 255, 255);
            }
        }
        //else if (iconList[2] == true)
        //{
        //    listRenderer[0].color = new Color32(66, 66, 66, 255);
        //    listRenderer[1].color = new Color32(66, 66, 66, 255);
        //    listRenderer[2].color = new Color32(255, 255, 255, 255);
        //}

        //Debug.Log(currentHealth);
        //healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / 100, lerpSpeed * Time.deltaTime);
    }

    public void UpdateSelectedArrow()
    {
        if (fireArrowCount != 0)
        {
            iconList[iconListIndex] = false;

            if (iconListIndex == 1)
            {
                iconListIndex = 0;
                iconList[iconListIndex] = true;
            }

            else
            {
                iconListIndex++;
                iconList[iconListIndex] = true;
            }
        }
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDie = true;
        gameOver.enabled = true;
        Invoke("RestartLevel", 3.0f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene("Scene_0");
    }

    public void RestoreHP(int damageTaken)
    {
        currentHealth += damageTaken;
        if (currentHealth >= 1200)
            currentHealth = 1200;
    }

    void DefaultArrow()
    {
        if (fireArrowCount == 0)
            iconList[0] = true;
    }
}
