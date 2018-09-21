using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    private float typeSpeed = 0.125f;
    private float startDelay = 0.0f;
    private float volumeVariation;
    private bool startOnAwake = true;

    private int counter;
    private string textToType;
    private bool typing;
    private AudioSource audioComp;

    public Text textComp;
    public PlayerController player;
    public BowString bowString;
    public GameObject GhostPrefab;
    public GameObject BalloonPrefab;
    public EnemySpawnManager enemySpawnManager;
    public Transform MonsterSpawnPoint;
    public Canvas tutorialCanvas;

    string sentence1 = "Welcome to the game, now I will\nguide you through the tutorial.";
    string sentence2 = "Press lower trigger to stretch\nthe bow string and release it\nto shoot an arrow.";
    string sentence3 = "One monster will appear near\nto the bridge, try to shoot it down.";
    string sentence4 = "Great! You can see the\nhealth point is increased.";
    string sentence5 = "The amount to increase is\nbased on enemy damage.";
    string sentence6 = "Press upper trigger to switch arrow.";
    string sentence7 = "Two monsters will appear soon,\nshoot them down with the fire arrow.";
    string sentence8 = "You can get one fire arrow\nonce you shoot a balloon.";
    string sentence9 = "Lastly, the top right corner shows\nthe amount of remaining monster\nfor current wave.";
    string sentence10 = "Enjoy the game!";

    int sentenceIndex;

    public bool isTutorialEnd;
    bool currentSectionEnd;
    bool processing;

    GameObject ghost;
    GameObject ghost1;
    GameObject balloon;

    List<string> sentenceList = new List<string>();

    void OnEnable()
    {
        player.enabled = false;
        bowString.enabled = false;
        enemySpawnManager.enabled = false;
        isTutorialEnd = false;
        currentSectionEnd = false;
        processing = true;

        sentenceIndex = 0;
        sentenceList.Add(sentence1);
        sentenceList.Add(sentence2);
        sentenceList.Add(sentence3);
        sentenceList.Add(sentence4);
        sentenceList.Add(sentence5);
        sentenceList.Add(sentence6);
        sentenceList.Add(sentence7);
        sentenceList.Add(sentence8);
        sentenceList.Add(sentence9);
        sentenceList.Add(sentence10);

        textComp.text = sentenceList[sentenceIndex];

        ////audioComp = GetComponent<AudioSource>();

        counter = 0;
        textToType = textComp.text;
        textComp.text = "";

        StartCoroutine(Step1());
    }

    // Intro (10f)
    IEnumerator Step1()
    {
        while (!currentSectionEnd)
        {
            if (processing)
            {
                StartTyping();
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(10f);
            yield return StartCoroutine(Step2());
        }
    }

    // Teach user how to shoot arrow (11f)
    IEnumerator Step2()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(11f);
            bowString.enabled = true;
            yield return StartCoroutine(WaitForKeyUp(KeyCode.Mouse0));
            yield return StartCoroutine(Step3());
        }
    }

    // Ask player to shoot monster (8f)
    IEnumerator Step3()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(8f);
            player.enabled = true;
            SpawnSingleMonster();
            yield return StartCoroutine(MonsterKilled());
            yield return StartCoroutine(Step4());
        }
    }

    // Inform the player health point is increased (9f)
    IEnumerator Step4()
    {
        currentSectionEnd = false;
        processing = true;
        player.enabled = false;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(9f);
            yield return StartCoroutine(Step5());
        }
    }

    // Explain about the amount to increase (9f)
    IEnumerator Step5()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(9f);
            yield return StartCoroutine(Step6());
        }
    }

    // Teach player to switch arrow (4f)
    IEnumerator Step6()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(4f);
            player.enabled = true;
            player.fireArrowCount += 1;
            yield return StartCoroutine(WaitForKeyDown(KeyCode.Escape));
            yield return StartCoroutine(Step7());
        }
    }

    // Ask player to shoot monster with fire arrow (9f)
    IEnumerator Step7()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(9f);
            SpawnMultipleMonster();
            yield return StartCoroutine(MultipleMonsterKilled());
            yield return StartCoroutine(Step8());
        }
    }

    // Spawn balloon (8f)
    IEnumerator Step8()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(8f);
            SpawnBalloon();
            yield return StartCoroutine(BalloonDisappear());
            yield return StartCoroutine(Step9());
        }
    }

    // Explain remaining monster (13f)
    IEnumerator Step9()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(13f);
            yield return StartCoroutine(Step10());
        }
    }

    // Display "Enjoy the game"
    IEnumerator Step10()
    {
        currentSectionEnd = false;
        processing = true;
        while (!currentSectionEnd)
        {
            if (processing)
            {
                UpdateText(sentenceList[sentenceIndex]);
                sentenceIndex++;
                processing = false;
            }
            yield return new WaitForSeconds(3f);
            StopTyping();
            StopAllCoroutines();
            enemySpawnManager.enabled = true;
            tutorialCanvas.enabled = false;
            yield break;
        }
    }

    public void StartTyping()
    {
        if (!typing)
        {
            InvokeRepeating("Type", startDelay, typeSpeed);
        }
    }

    public void StopTyping()
    {
        counter = 0;
        typing = false;
        CancelInvoke("Type");
    }

    void UpdateText(string newText)
    {
        StopTyping();
        textComp.text = "";
        textToType = newText;
        StartTyping();
    }

    private void Type()
    {
        typing = true;
        textComp.text += textToType[counter];
        //audioComp.Play();
        counter++;

        //RandomiseVolume();

        if (counter == textToType.Length)
        {
            typing = false;
            CancelInvoke("Type");
            currentSectionEnd = true;
        }
    }

    //private void RandomiseVolume()
    //{
    //    audioComp.volume = Random.Range(1 - volumeVariation, volumeVariation + 1);
    //}

    public bool IsTyping() { return typing; }

    IEnumerator WaitForKeyUp(KeyCode keycode)
    {
        bool done = false;

        while (!done)
        {
            if (Input.GetKeyUp(keycode))
                done = true;

            yield return null;
        }
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        bool done = false;

        while (!done)
        {
            if (Input.GetKeyDown(keyCode))
                done = true;

            yield return null;
        }
    }

    IEnumerator MonsterKilled()
    {
        bool done = false;

        while(!done)
        {
            if (ghost == null)
                done = true;

            yield return null;
        }
    }

    IEnumerator MultipleMonsterKilled()
    {
        bool done = false;
        
        while(!done)
        {
            if (ghost == null && ghost1 == null)
                done = true;

            yield return null;
        }
    }

    IEnumerator BalloonDisappear()
    {
        bool done = false;

        while (!done)
        {
            if (balloon == null)
                done = true;

            yield return null;
        }
    }

    void SpawnSingleMonster()
    {
        ghost = Instantiate(GhostPrefab, MonsterSpawnPoint.position, Quaternion.identity);
    }

    void SpawnMultipleMonster()
    {
        SpawnSingleMonster();
        ghost1 = Instantiate(GhostPrefab, MonsterSpawnPoint.position + new Vector3(0,0,1), Quaternion.identity);
    }

    void SpawnBalloon()
    {
        balloon = Instantiate(BalloonPrefab, MonsterSpawnPoint.position, BalloonPrefab.transform.rotation);
    }
}