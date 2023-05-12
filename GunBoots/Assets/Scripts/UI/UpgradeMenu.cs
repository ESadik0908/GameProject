using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject normalUi;
    public GameObject upgradeUi;

    [SerializeField] private Button upgradeButton1;
    [SerializeField] public Button upgradeButton2;
    [SerializeField] public Button upgradeButton3;

    private Color shotgunCoulour = new Color(1.00f, 0.30f, 0.30f);
    private Color dashColour = new Color(0.24f, 0.87f, 0.87f);
    private Color laserColour = new Color(0.67f, 1.00f, 0.60f);

    private System.Action[] weapons = new System.Action[3];
    private System.Action[] upgrades = new System.Action[5];

    private GameObject player;
    private PlayerStateController playerState;
    private PlayerUpgrades playerUpgrades;
    private PlayerMovement playerMovement;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerState = player.GetComponent<PlayerStateController>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        playerMovement = player.GetComponent<PlayerMovement>();

        // Assign the functions to functionGroup1
        weapons[0] = playerState.ShotgunUpgrade;
        weapons[1] = playerState.DashUpgrade;
        weapons[2] = playerState.LaserUpgrade;

        upgrades[0] = playerUpgrades.UpgradeDamage;
        upgrades[1] = playerUpgrades.UpgradeHealth;
        upgrades[2] = playerUpgrades.UpgradeSpeed;
        upgrades[3] = playerUpgrades.UpgradeAmmo;
        upgrades[4] = playerUpgrades.UpgradeLives;
    }


    public void ShowUpgrades()
    {
        ChooseButtonUpgrade(upgradeButton1);
        ChooseButtonUpgrade(upgradeButton2);
        ChooseButtonUpgrade(upgradeButton3);

        normalUi.SetActive(false);
        pauseMenuUi.SetActive(false);
        upgradeUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void Resume()
    {
        upgradeUi.SetActive(false);
        pauseMenuUi.SetActive(false);
        normalUi.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void ChooseButtonUpgrade(Button button)
    {
        int randomWepon = Random.Range(0, weapons.Length);
        int randomUpgrade = Random.Range(0, upgrades.Length);
        string upgradeText = "";
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();


        if(randomWepon == 0)
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = shotgunCoulour;
            colorBlock.highlightedColor = shotgunCoulour;
            button.colors = colorBlock;
            upgradeText = "Shotgun";
        }
        else if(randomWepon == 1)
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = dashColour;
            colorBlock.highlightedColor = dashColour;
            button.colors = colorBlock;
            upgradeText = "Dash";
        }
        else
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = laserColour;
            colorBlock.highlightedColor = laserColour;
            button.colors = colorBlock;
            upgradeText = "Laser";
        }
        
        if(randomUpgrade == 0)
        {
            upgradeText += ("\n+\nDamage");
        }
        else if(randomUpgrade == 1)
        {
            upgradeText += ("\n+\nMax Health");
        }
        else if (randomUpgrade == 2)
        {
            upgradeText += ("\n+\nMove Speed");
        }
        else if (randomUpgrade == 3)
        {
            upgradeText += ("\n+\nMax Ammo");
        }
        else
        {
            upgradeText += ("\n+\nExtra Life");
        }



        buttonText.text = upgradeText;
       
        // Assign a random function from functionGroup1 to button1
        System.Action resume = Resume;
        System.Action resetGravity = playerMovement.ResetGravity;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => resume.Invoke());
        button.onClick.AddListener(() => weapons[randomWepon].Invoke());
        button.onClick.AddListener(() => upgrades[randomUpgrade].Invoke());
        button.onClick.AddListener(() => resetGravity.Invoke());
    }

}
