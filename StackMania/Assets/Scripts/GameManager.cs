using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool inGame = false;
    public bool endGame = false;

    public List<GameObject> collectedStacks = new List<GameObject>();
    public List<GameObject> collectedGolds = new List<GameObject>();

    public int currentStackAmount;

    public List<Material> skyBoxes = new List<Material>();

    public GameObject[] levels;

    public GameObject stackPrefab;
    public List<GameObject> instantiatedStacks = new List<GameObject>();

    public GameObject errorPrefab, canvas;

    public GameObject goldSoundPrefab, stackSoundPrefab, applauseSoundPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //LoadLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        PlayerController.instance.alive = true;
        var level = PlayerPrefs.GetInt("Level");
        foreach (GameObject levelItem in levels)
        {
            levelItem.SetActive(false);
        }

        levels[level - 1].SetActive(true);

        foreach (GameObject stack in instantiatedStacks)
        {
            Destroy(stack, 0f);
        }

        instantiatedStacks.Clear();

        foreach (GameObject collectedStack in collectedStacks)
        {
            collectedStack.SetActive(true);
        }

        collectedStacks.Clear();

        foreach (GameObject collectedGold in collectedGolds)
        {
            collectedGold.SetActive(true);
        }

        collectedGolds.Clear();
        
        ChangeSkyBox();

        currentStackAmount = PlayerPrefs.GetInt("UpgradedStackAmount");

        UIManager.instance.tapToPlayScreen.SetActive(true);

        PlayerController.instance.rg.useGravity = false;
        PlayerController.instance.transform.position = PlayerController.instance.startingPosition;
        PlayerController.instance.transform.rotation = Quaternion.identity;
    }

    void ChangeSkyBox()
    {
        var level = PlayerPrefs.GetInt("Level");

        if (level == 1)
        {
            RenderSettings.skybox = skyBoxes[0];
        }
        else if (level == 2)
        {
            RenderSettings.skybox = skyBoxes[1];
        }
        else
        {
            RenderSettings.skybox = skyBoxes[2];
        }
    }

    public void InstantiateStack(Vector3 position, Quaternion rotation)
    {
        GameObject stack;

        stack = Instantiate(stackPrefab, position, rotation);

        instantiatedStacks.Add(stack);
    }

    public void InstantiateError()
    {
        GameObject error;

        error = Instantiate(errorPrefab);
        error.transform.SetParent(canvas.transform, false);
        Destroy(error, 3f);
    }

    public void InstantiateGoldSound(Vector3 position)
    {
        GameObject goldSound;

        goldSound = Instantiate(goldSoundPrefab, position, Quaternion.identity);

        Destroy(goldSound, 2f);
    }

    public void InstantiateStackSound(Vector3 position)
    {
        GameObject stackSound;

        stackSound = Instantiate(stackSoundPrefab, position, Quaternion.identity);

        Destroy(stackSound, 2f);
    }

    public void InstantiateApplauseSound(Vector3 position)
    {
        GameObject applauseSound;

        applauseSound = Instantiate(applauseSoundPrefab, position, Quaternion.identity);

        Destroy(applauseSound, 5f);
    }
}
