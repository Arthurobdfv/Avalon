using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObserserInfoPanels : MonoBehaviour
{
    private EntitySpawner _entitySpawner;

    [SerializeField] TMPro.TMP_Text _playerListPanel;
    [SerializeField] TMPro.TMP_Text _enemiesListPanel;

    private void OnEnable()
    {
        if(_entitySpawner == null)
        {
            _entitySpawner = FindAnyObjectByType<EntitySpawner>();
        }
    }
    

    private void Update()
    {
        UpdatePlayerListPanel();
        UpdateEnemiesListPanel();
    }

    private void UpdateEnemiesListPanel()
    {
        _enemiesListPanel.text = "Enemies:\n";  
        _enemiesListPanel.text += string.Join(Environment.NewLine, _entitySpawner.SpawnedEntities
            .Where(x => x.Value.EntityType == EntityTypeEnum.Enemy)
            .Select(x => $"Id: {x.Key}, {x.Value.AssetId}"));
    }

    private void UpdatePlayerListPanel()
    {
        _playerListPanel.text = "Players:\n";
        _playerListPanel.text += string.Join(Environment.NewLine, _entitySpawner.Characters.Select(x => $"Id: {x.Key}, {x.Value}"));
    }
}
