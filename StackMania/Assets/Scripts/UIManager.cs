using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text levelText1, goldText, levelText2;

    public GameObject tapToPlayScreen, inGameScreen, endGameScreen, retryScreen;

    public GameObject upgradeTextBackground, upgradeStackButton;

    public Text upgradeText, upgradeButtonText, collectedGoldAmountText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        levelText1.text = "Level: " + PlayerPrefs.GetInt("Level").ToString();
        levelText2.text = "Level: " + PlayerPrefs.GetInt("Level").ToString();
        goldText.text = PlayerPrefs.GetInt("Golds").ToString();
        upgradeText.text = "Upgrade Starting Stack: " + (PlayerPrefs.GetInt("UpgradedStackAmount") + 5).ToString();
        upgradeButtonText.text = (PlayerPrefs.GetInt("UpgradedStackAmount") + 5).ToString();

        if (PlayerPrefs.GetInt("UpgradedStackAmount") >= 20)
        {
            upgradeStackButton.SetActive(false);
            upgradeTextBackground.SetActive(false);
        }

        if (GameManager.instance.endGame && PlayerController.instance.alive)
        {
            inGameScreen.SetActive(false);
            endGameScreen.SetActive(true);
            collectedGoldAmountText.text = "Collected Gold: " + GameManager.instance.collectedGolds.Count.ToString();
        }
        else if (GameManager.instance.endGame && !PlayerController.instance.alive)
        {
            inGameScreen.SetActive(false);
            retryScreen.SetActive(true);
        }
    }

    public void PlayButtonClick()
    {
        tapToPlayScreen.SetActive(false);
        inGameScreen.SetActive(true);
        PlayerController.instance.alive = true;
        GameManager.instance.inGame = true;
        PlayerController.instance.rg.useGravity = true;
    }

    public void StackUpgradeButtonClick()
    {
        if (PlayerPrefs.GetInt("UpgradedStackAmount") + 5 <= PlayerPrefs.GetInt("Golds"))
        {
            PlayerPrefs.SetInt("Golds", PlayerPrefs.GetInt("Golds") - (PlayerPrefs.GetInt("UpgradedStackAmount") + 5));
            PlayerPrefs.SetInt("UpgradedStackAmount", PlayerPrefs.GetInt("UpgradedStackAmount") + 5);
            GameManager.instance.currentStackAmount = PlayerPrefs.GetInt("UpgradedStackAmount");
        }
        else
        {
            GameManager.instance.InstantiateError();
        }
    }

    public void NextLevelButtonClick()
    {
        GameManager.instance.endGame = false;
        GameManager.instance.LoadLevel();
        endGameScreen.SetActive(false);
    }

    public void RetryButtonClick()
    {
        GameManager.instance.endGame = false;
        GameManager.instance.LoadLevel();
        retryScreen.SetActive(false);
    }
}
