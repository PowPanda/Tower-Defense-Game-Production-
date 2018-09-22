using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public PlayerController player;
    public GameObject castleFlames;
    List<GameObject> castleFires = new List<GameObject>();

    void Start ()
    {
        PopUpTextController.Initialize();
        for (int i = 0; i < castleFlames.transform.childCount; i++)
            castleFires.Add(castleFlames.transform.GetChild(i).gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Break();
        switch (player.currentHealth * 100 / player.totalHealth)
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

    void SpawnFireOnCastle(int fireAmount)
    {
        foreach (GameObject fire in castleFires)
        {
            if (fireAmount != 0)
            {
                if (fire.activeSelf == false)
                    fire.SetActive(true);

                fireAmount--;
            }
        }
    }
}
