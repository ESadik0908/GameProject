using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider ammoCountSlider;
    private PlayerStatsTracker playerStats;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStatsTracker>();

    }

    // Update is called once per frame
    void Update()
    {
        ammoCountSlider.maxValue = playerStats.maxAmmo;
        ammoCountSlider.value = playerStats.ammo;
    }
}
