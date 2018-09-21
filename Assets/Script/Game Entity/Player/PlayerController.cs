using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // Attributes
    string normalArrowCount;
    public int fireArrowCount;
    int currentHealth;
    public bool isDie;
    int totalHealth;

    // Accessed Components
    public Camera fpsCamera;
    public Text text_normalArrowCount;
    public Text text_fireArrowCount;
    public Text text_healthpoint;
    public Text gameOver;
    public GameObject castle_Flames;

    // Others
    List<SpriteRenderer> listRenderer = new List<SpriteRenderer>();
    List<GameObject> castle_Fires = new List<GameObject>();
    int i;
    public float lerpSpeed;
    int castleFireIndex;
    bool isFireSpawned;

    // Icon List
    public bool[] iconList = new bool[2] { true, false };

	// Use this for initialization
	void Start () {
        normalArrowCount = "\u221E";
        fireArrowCount = 1;
        i = 0;
        currentHealth = 1200;
        totalHealth = currentHealth;
        isDie = false;
        castleFireIndex = 0;
        isFireSpawned = false;

        for (int i = 0; i < castle_Flames.transform.GetChildCount(); i++)
        {
            castle_Fires.Add(castle_Flames.transform.GetChild(i).gameObject);
        }

        SpriteRenderer iconFireArrow = GameObject.Find("Player/Main Camera/Bow/Icon_Fire_Arrow").GetComponent<SpriteRenderer>();
        //SpriteRenderer iconIceArrow = GameObject.Find("Player/Main Camera/Bow/Icon_Ice_Arrow").GetComponent<SpriteRenderer>();
        SpriteRenderer iconNormalArrow = GameObject.Find("Player/Main Camera/Bow/Icon_Normal_Arrow").GetComponent<SpriteRenderer>();

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

        switch(currentHealth * 100 / totalHealth)
        {
            case 10:
                SpawnFireOnCastle(3);
                break;

            case 20:
                SpawnFireOnCastle(3);
                break;

            case 40:
                SpawnFireOnCastle(2);
                break;

            case 50:
                SpawnFireOnCastle(2);
                break;

            case 60:
                SpawnFireOnCastle(1);
                break;

            case 80:
                SpawnFireOnCastle(1);
                break;
        }
    }

    public void UpdateSelectedArrow()
    {
        if (fireArrowCount != 0)
        {
            iconList[i] = false;

            if (i == 1)
            {
                i = 0;
                iconList[i] = true;
            }

            else
            {
                iconList[i + 1] = true;
                i++;
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
        if (currentHealth >= 1000)
            currentHealth = 1000;
    }

    void DefaultArrow()
    {
        if (fireArrowCount == 0)
            iconList[0] = true;
    }

    void SpawnFireOnCastle(int fireAmount)
    {
        foreach (GameObject fire in castle_Fires)
        {
            if (fireAmount != 0)
            {
                if (fire.active == false)
                    fire.SetActive(true);

                fireAmount--;
            }
        }
    }
}
