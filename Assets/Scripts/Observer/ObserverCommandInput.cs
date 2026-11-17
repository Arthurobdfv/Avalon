using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObserverCommandInput : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField _commandInput;
    private IAvalonClientPacketSender _packetSender;

    private void OnEnable()
    {
        _packetSender = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None)
            .OfType<IAvalonClientPacketSender>()
            .FirstOrDefault();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProcessInput();
        }
    }

    public void ProcessInput()
    {
        var commandInput = _commandInput?.text;
        if (string.IsNullOrEmpty(commandInput))
        {
            return;
        }
        var parsedCommand = commandInput.Split(' ');

        if (parsedCommand[0] == "move")
        {
            MoveCommand(parsedCommand);
        }

        if (parsedCommand[0] == "list")
        {
            ListCommand(parsedCommand);
        }
        _commandInput.text = string.Empty;
    }

    private void ListCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Debug.Log("List command expects mapname. Format:\"move {mapId}\" ");
            return;
        }
        var command = args[1];
        switch (command)
        {
            case "maps":
                Debug.Log($"List of available maps: {Environment.NewLine}{string.Join(Environment.NewLine,MapManager.AvailableMaps.Select((mapId, idx) => $"{idx}: {mapId}"))}");
                break;
            default:
                Debug.Log($"Command {command} not yet implemented under list command");
                break;
        }
    }

    public void MoveCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Debug.Log("Move command expects mapname. Format:\"move {mapId}\" ");
            return;
        }
        var mapToMove = TryParseMapInput(args[1]);
        var moveMapPacket = new ObserveMapPacket()
        {
            MapId = mapToMove
        };
        moveMapPacket.SetClientId(LocalServerPacketSender.CurrentObserverId);
        _packetSender.Send(moveMapPacket);
    }

    private string TryParseMapInput(string input)
    {
        if (int.TryParse(input, out var index))
        {
            if (index < MapManager.AvailableMaps.Count)
            {
                return MapManager.AvailableMaps.ElementAt(index);
            }
            else Debug.Log($"No map found with index {index}");
        }
        else
        {
            var map = MapManager.AvailableMaps.FirstOrDefault(x => x.ToLower() == input);
            if (map != null)
            {
                return map;
            }
            else Debug.Log($"No map found with name {input}");
        }
        return null;
    }
}
