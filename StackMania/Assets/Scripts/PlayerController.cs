using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public DynamicJoystick joystick;

    public bool alive = true;

    float movementSpeed = 10f;
    public float rotationSpeed = 2f;
    private float touchStartingPosition;

    public GameObject stackAmountBar, currentStackBar;

    public GameObject cameraPosition;

    public Vector3 startingPosition = new Vector3(0f, -2.91f, 0.3f);

    float stackRate = 0f;
    float stackTime = 0.25f;

    public Rigidbody rg;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        rg = transform.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        StackBar();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void StackBar()
    {
        if (GameManager.instance.inGame)
        {
            if (GameManager.instance.currentStackAmount > 0)
            {
                stackAmountBar.SetActive(true);
                currentStackBar.transform.localScale = new Vector3((GameManager.instance.currentStackAmount) / 20f, 1f, 1f);
            }
            else
            {
                stackAmountBar.SetActive(false);
            }
        }
        else
        {
            stackAmountBar.SetActive(false);
        }
    }

    void Movement()
    {
        if (alive && GameManager.instance.inGame)
        {
            var horizontal = joystick.Horizontal;

            if (horizontal> 0 || horizontal < 0)
            {
                transform.Rotate(0f, horizontal * rotationSpeed, 0f);
            }

            // OTHER WAY
            //if (Input.touchCount > 0)
            //{
            //    Touch touch = Input.GetTouch(0);
            //    switch (touch.phase)
            //    {
            //        case TouchPhase.Began:
            //            touchStartingPosition = touch.position.x;
            //            break;
            //        case TouchPhase.Moved:
            //            if (touchStartingPosition > touch.position.x)
            //            {
            //                transform.Rotate(-Vector3.up, rotationSpeed * Time.deltaTime);
            //            }
            //            else if (touchStartingPosition < touch.position.x)
            //            {
            //                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            //            }
            //            break;
            //        case TouchPhase.Ended:
            //            break;
            //    }
            //}

            if (GameManager.instance.currentStackAmount > 0)
            {
                transform.GetComponent<Animation>().Play("Run2");
            }
            else
            {
                transform.GetComponent<Animation>().Play("Run");
            }
            transform.Translate(Vector3.forward / movementSpeed);
        }
        else if (GameManager.instance.endGame && alive)
        {
            transform.GetComponent<Animation>().Play("Dance");
        }
        else
        {
            transform.GetComponent<Animation>().Play("Idle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "stack" && GameManager.instance.currentStackAmount<20)
        {
            GameManager.instance.InstantiateStackSound(transform.position);
            GameManager.instance.collectedStacks.Add(other.gameObject);
            other.gameObject.SetActive(false);
            GameManager.instance.currentStackAmount += 1;
        }

        if (other.gameObject.tag == "gold")
        {
            GameManager.instance.InstantiateGoldSound(transform.position);
            GameManager.instance.collectedGolds.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "obstacle" || other.gameObject.tag == "standObstacle")
        {
            if (GameManager.instance.currentStackAmount>2)
            {
                GameManager.instance.currentStackAmount -= 2;
            }
            else
            {
                GameManager.instance.currentStackAmount = 0;
            }
            Debug.Log("Hit");
        }

        if (other.gameObject.tag == "finishLine")
        {
            var collectedGoldsAmount = GameManager.instance.collectedGolds.Count;
            PlayerPrefs.SetInt("Golds", PlayerPrefs.GetInt("Golds") + collectedGoldsAmount);
            GameManager.instance.InstantiateApplauseSound(transform.position);
            GameManager.instance.inGame = false;
            GameManager.instance.endGame = true;
            var level = PlayerPrefs.GetInt("Level");
            if (level <3)
            {
                level += 1;
            }
            else
            {
                level = 1;
            }

            PlayerPrefs.SetInt("Level", level);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "outGround")
        {
            if (GameManager.instance.currentStackAmount > 0)
            {
                stackRate += Time.deltaTime;
                if (stackRate>stackTime)
                {
                    stackRate = 0f;
                    GameManager.instance.InstantiateStack(transform.position, transform.rotation);
                    GameManager.instance.currentStackAmount -= 1;
                }
            }
            else
            {
                stackRate += Time.deltaTime;
                if (stackRate>stackTime)
                {
                    stackRate = 0f;
                    collision.gameObject.transform.GetComponent<BoxCollider>().enabled = false;
                    GameManager.instance.inGame = false;
                    GameManager.instance.endGame = true;
                    alive = false;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "outGround")
        {
            stackRate = 0f;
        }
    }
}
