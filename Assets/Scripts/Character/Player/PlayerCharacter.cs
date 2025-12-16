using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CombatCharacter
{
    private void Start()
    {
        _currentHealth = BaseStats.Health;
        _currentTime = 0f;
    }

    private void Update()
    {
        HandleCombat();
        _currentTime += Time.deltaTime;
    }

    // TODO: Possibly have this in a Combat Manager or something... Already thinking on a Multiplayer Context
    private void HandleCombat()
    {
        if(Target != null)
        {
            if(_currentTime > BaseStats.AttackSpeed)
            {
                Debug.Log("Attack");
                _currentTime = 0f;
            }
        }
    }
}
