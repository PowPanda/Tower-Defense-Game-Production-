using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BowString : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float factor;

    Vector3 firstPosition;
    Vector3 lastPosition;

    public bool stretching;
    public bool releasing;

    public float stretchSpeed = 0.1f;
    public float releaseSpeed = 0.1f;

    public GameObject normal_arrowPrefab;
    public GameObject fire_arrowPrefab;
    public Transform arrowSpawnPos;
    GameObject arrow;
    int previosArrow;
    bool arrowSlotted;
    
    //public GameObject iceArrow;
    public PlayerController player;

    Quaternion arrowRotation;

    float shootInterval;
    float timer;

    // Use this for initialization
    void Start () {
        firstPosition = transform.localPosition;
        lastPosition = transform.localPosition + Vector3.up * 0.45f;
        arrowSlotted = false;
        SlotFirstArrow();
        shootInterval = 1.0f;
        timer = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timer == shootInterval)
        {
            SpawnArrow();
            ShootLogic();
            transform.localPosition = Vector3.Lerp(firstPosition, lastPosition, factor);
        }

        timer += Time.deltaTime;

        if (timer > shootInterval)
        {
            timer = shootInterval;
        }
    }

    //You probably want to call this somewhere
    void Stretch()
    {
        stretching = true;
        releasing = false;
    }

    void Release()
    {
        releasing = true;
        stretching = false;
    }

    void SpawnArrow()
    {
        if (!arrowSlotted)
        {
            arrowRotation = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
            if (previosArrow == 1)
                arrow = Instantiate(fire_arrowPrefab, arrowSpawnPos.position, arrowRotation) as GameObject;

            else
                arrow = Instantiate(normal_arrowPrefab, arrowSpawnPos.position, arrowRotation) as GameObject;

            arrow.transform.parent = transform;
            arrowSlotted = true;
        }

        else if (arrowSlotted)
        {
            arrowRotation = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))// || Input.GetButtonDown("A") || Input.GetButtonDown("B") || Input.GetButtonDown("C") || Input.GetButtonDown("D")) 
            {
                player.UpdateSelectedArrow();
                if (player.iconList[0] == true)
                {
                    Destroy(arrow);

                    arrow = Instantiate(normal_arrowPrefab, arrowSpawnPos.position, arrowRotation) as GameObject;
                    arrow.transform.parent = transform;
                    previosArrow = 0;
                }

                else if (player.iconList[1] == true && player.fireArrowCount != 0)
                {
                    if (arrow.tag != "FireArrow")
                        Destroy(arrow);

                    arrow = Instantiate(fire_arrowPrefab, arrowSpawnPos.position, arrowRotation) as GameObject;
                    arrow.transform.parent = transform;
                    previosArrow = 1;
                }
            }
        }
    }

    void ShootLogic()
    {
        Rigidbody arrow_RB = arrow.transform.GetComponent<Rigidbody>();
        Projectile arrowProjectile = arrow.transform.GetComponent<Projectile>();

        if (Input.GetMouseButton(0) && arrowSlotted) //|| Input.GetButtonDown("A") || Input.GetButtonDown("B") || Input.GetButtonDown("C") || Input.GetButtonDown("D"))
        {
            factor += stretchSpeed * Time.deltaTime;

            if (factor > 1f)
                factor = 1f;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            arrowSlotted = false;
            arrow_RB.isKinematic = false;
            arrow.transform.parent = null;
            arrowProjectile.shootForce += 2500 * (factor / 1f) + 0.05f;    // The arrow will be shot depends on the force player give
                                                                         // 0.05f is just to make sure the minimum force is always 0.05f
            arrowProjectile.enabled = true;
            factor = 0.0f;

            // Reduce fire arrow count after player shoot a fire arrow
            if (previosArrow == 1)
                player.fireArrowCount--;

            // Slot normal arrow when player shoot last fire arrow
            if (player.fireArrowCount == 0)
                previosArrow = 0;

            timer = 0;
        }

        //if (Input.GetMouseButtonDown(0) && arrowSlotted == false)
        //    SpawnArrow();
    }

    void SlotFirstArrow()
    {
        arrowRotation = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        arrow = Instantiate(normal_arrowPrefab, arrowSpawnPos.position, arrowRotation) as GameObject;
        arrow.transform.parent = transform;
        arrowSlotted = true;
    }
}
