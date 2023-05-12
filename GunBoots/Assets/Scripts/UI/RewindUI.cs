using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindUI : MonoBehaviour
{
    [SerializeField] GameObject rewindUi;

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            rewindUi.SetActive(false);
            return;
        }
        if (TimeBody.isRewinding)
        {
            rewindUi.SetActive(true);
        }
        else
        {
            rewindUi.SetActive(false);
        }
    }
}
