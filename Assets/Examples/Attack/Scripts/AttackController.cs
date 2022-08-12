using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class AttackController : MonoBehaviour
{
    //Can be any name
    delegate void AttackDelegate();

    private const int MaxCombo = 3;
    private const float ComboResetTime = 0.5f;

    [SerializeField] private float attackSpeed = 0.1f;
    
    [SerializeField, ReadOnly] private int currentCombo;
    [SerializeField, ReadOnly] private float attackCd;
    
    private Coroutine holdAttackCoroutine;
    private Coroutine resetComboCoroutine;
    
    private AttackDelegate attackDelegate;

    /// <summary>
    /// Call from <see cref="CharacterController"/>. 
    /// </summary>
    //Doing this instead of using Start because we can control which class need to init first.
    //If using Start we can't tell which start gonna run first and can cause unexpected bug.
    public void Init()
    {
        ResetCombo();
        attackCd = 0;
    }

    private void Update()
    {
        UpdateAttackTime();
    }

    /// <summary>
    /// Decrease attack cooldown.
    /// </summary>
    private void UpdateAttackTime()
    {
        if (attackCd >= 0)
        {
            attackCd -= Time.deltaTime;
        }
    }
    
    public void AttackAction(InputAction.CallbackContext _context)
    {
        //Read action phase
        //see https://docs.unity3d.com/Packages/com.unity.inputsystem@1.3/api/UnityEngine.InputSystem.InputActionPhase.html
        switch (_context.phase)
        {
            case InputActionPhase.Started:
                //Debug.Log("Start");
                break;
            
            case InputActionPhase.Performed:
                ResolveAttackInteraction(_context.interaction);
                break;
            
            case InputActionPhase.Canceled:
                //Release attack button, stop running hold attack coroutine 
                if (holdAttackCoroutine != null)
                    StopCoroutine(holdAttackCoroutine);
                
                break;
        }
    }

    /// <summary>
    /// Check player input interaction type
    /// </summary>
    /// <param name="_interaction"></param>
    private void ResolveAttackInteraction(IInputInteraction _interaction)
    {
        //see https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Interactions.html
        switch (_interaction)
        {
            //Player tapping
            case TapInteraction:
            {
                if (attackCd <= 0)
                {
                    //Call attack delegate. Put ? before calling function to check if delegate is null or not
                    //Same thing as
                    // if (attackDelegate != null)
                    // {
                    //     attackDelegate.Invoke();
                    // }
                    attackDelegate?.Invoke();
                    
                    //Start attack cooldown
                    attackCd = attackSpeed;
                }
                break;
            }
            //Player holding button
            case HoldInteraction:
                Debug.Log("HoldAttack");
                holdAttackCoroutine = StartCoroutine(IEHoldAttack());
                break;
        }
    }

    /// <summary>
    /// Keep attacking while holding attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEHoldAttack()
    {
        //while (true) mean Keep looping until coroutine is stopped.
        //Keep calling attack delegate every x second.
        while (true)
        {
            attackDelegate?.Invoke();
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    /// <summary>
    /// Change attack delegate to next stage
    /// </summary>
    private void CycleCombo()
    {
        //Same thing as
        // currentCombo++;
        // if (currentCombo >= MaxCombo)
        // {
        //     currentCombo = 1;
        // }
        // Basically if current combo is 3 reset to 1 (3 % 3 = 0 + 1 = 1)
        currentCombo = (currentCombo % MaxCombo) + 1;
        
        switch (currentCombo)
        {
            case 1:
                attackDelegate = Attack1;
                break;
            case 2:
                attackDelegate = Attack2;
                break;
            case 3:
                attackDelegate = Attack3;
                break;
        }
        //Stop reset combo coroutine before starting new one to reset coroutine
        if (resetComboCoroutine != null)
            StopCoroutine(resetComboCoroutine);
        
        resetComboCoroutine = StartCoroutine(IETryResetCombo());
    }

    /// <summary>
    /// When stop attacking long enough reset combo to 1st stage
    /// </summary>
    /// <returns></returns>
    private IEnumerator IETryResetCombo()
    {
        yield return new WaitForSeconds(ComboResetTime);
        ResetCombo();
    }
    
    /// <summary>
    /// Reset combo to 1st stage
    /// </summary>
    private void ResetCombo()
    {
        Debug.Log("ResetCombo");
        
        //Combo already is 1, don't do anything
        if (currentCombo == 1) return;

        currentCombo = 1;
        attackDelegate = Attack1;
    }
    
    private void Attack1()
    {
        Debug.Log("Attack1");
        CycleCombo();
    }

    private void Attack2()
    {
        Debug.Log("Attack2");
        CycleCombo();
    }

    private void Attack3()
    {
        Debug.Log("Attack3");
        CycleCombo();
    }

}
