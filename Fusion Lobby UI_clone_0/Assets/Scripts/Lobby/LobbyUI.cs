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
    
    private string generatedCode;
    private bool clicked;

    private void Awake()
    {
        CreateBtn.onClick.AddListener(() => HostGame());
        JoinBtn.onClick.AddListener(() => EnterCode());
    }

    // Used to generate and host
    public void HostGame()
    {

        // Ensures code and room will only be created once after click
        if(!clicked)
        {
            GenerateCode();
            roomCode.text = generatedCode; // Display code for Host
            StartGame(GameMode.Host);
        }
        else
        {
            Debug.Log("Already Clicked");
        }
    }

    public void EnterCode()
    {
  
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        InputPanel.SetActive(true);

        RoomBtn.onClick.AddListener(() => CheckCode());
    }

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

    public void EnterGame()
    {
        RoomPanel.SetActive(false);
    }

    public void CheckCode()
    {
        if(string.IsNullOrEmpty(roomName.text))
        {
            Debug.Log("No Input, Enter Code!");
        }

        else
        {
            StartGame(GameMode.Client);
        }
    }

    void GenerateCode(){
        string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        //Creates a random code based on characters provided
        for(int i=0; i<8; i++)
        {
            generatedCode += characters[UnityEngine.Random.Range(0, characters.Length)];
        }

        clicked = true;
    }
}
