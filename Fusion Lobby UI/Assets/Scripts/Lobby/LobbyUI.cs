using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;

public class LobbyUI : LobbyManager
{
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TextMeshProUGUI numberOfPlayers;
    [SerializeField] private Button CreateBtn;
    [SerializeField] private Button JoinBtn;
    [SerializeField] private Button RoomBtn;

    public Button StartGameButton;
    public Button EnterGameButton;
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI roomCode;
    public GameObject LobbyPanel;
    public GameObject InputPanel;
    public GameObject RoomPanel; 

    public string NumberOfPlayers
    {
        get { return numberOfPlayers.text; }
        set { numberOfPlayers.text = value; }
    }
    
    public string generatedCode;

    [Networked] private bool _hostReady {get; set;}

    private void Awake()
    {
        CreateBtn.onClick.AddListener(() => HostGame());
        JoinBtn.onClick.AddListener(() => EnterCode());
        // QuitBtn.onClick.AddListener(() => Debug.Log("Exit"));
    }

    // Used to generate and host
    public void HostGame()
    {
        GenerateCode();
        roomCode.text = generatedCode; //generatedCode;
        StartGame(GameMode.Host);

        Debug.Log(generatedCode);
    }

    public void EnterCode()
    {
  
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        InputPanel.SetActive(true);

        RoomBtn.onClick.AddListener(() => StartGame(GameMode.Client));
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void CreatePlayers()
    {
        foreach(var player in _runner.ActivePlayers)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % _runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = _runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }

        RoomPanel.SetActive(false);
    }

    void GenerateCode(){
        string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        for(int i=0; i<8; i++)
        {
            generatedCode += characters[UnityEngine.Random.Range(0, characters.Length)];
        }
    }
}
