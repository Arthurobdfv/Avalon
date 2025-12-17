using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider BarSlider;
    [SerializeField] CombatCharacter CharacterData;

    private void Start()
    {
        if (BarSlider == null)
        {
            BarSlider = GetComponent<Slider>() ?? throw new MissingComponentException(nameof(Slider));
        }
        TrySetCharacterData();
    }

    private void OnEnable()
    {
        TrySetCharacterData();
        CharacterData.OnHealthChangeHandler += OnHealthChange;
    }

    private void OnDisable()
    {
        CharacterData.OnHealthChangeHandler -= OnHealthChange;
    }

    private void OnHealthChange(CombatCharacter.HealthChangeEventArgs healthChangeEvent)
    {
        BarSlider.value = healthChangeEvent.NewHealth / healthChangeEvent.MaxHealth;
    }

    private void TrySetCharacterData()
    {
        if(CharacterData == null)
        {
            CharacterData = GetComponentInParent<CombatCharacter>(true);
        }
    }
}
