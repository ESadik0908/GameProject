using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class ExtraJumps : MonoBehaviour
{
    Controller2D controller;

    [SerializeField] int jumpCountReset = 2;
    int jumpCount;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        jumpCount = jumpCountReset;

    }

    private void Update()
    {
        if (!controller.collisions.below && Input.GetButtonDown("Jump") && jumpCount != 0)
        {
            SendMessage("Jump");
            jumpCount -= 1;
        }

        if (controller.collisions.below)
        {
            jumpCount = jumpCountReset;
        }
    }
}
