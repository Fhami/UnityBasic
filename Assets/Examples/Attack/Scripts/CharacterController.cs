using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private AttackController attackController;

    private void Start()
    {
        attackController.Init();
    }
}
