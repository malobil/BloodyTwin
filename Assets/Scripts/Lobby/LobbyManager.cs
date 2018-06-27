using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    public enum PlayerType
    {
        Undefined = -1,
        Exectioner = 0,
        Ghost = 1,
        Intruder = 2
    }

    public static LobbyManager Instance;

    private string _clientNetId;
    private bool _ready;
    private PlayerType _type = PlayerType.Undefined;
    private bool _roomReady;

    [Header("Game options")]
    [SerializeField] private int _maxExecutioners = 1;
    [SerializeField] private int _maxGhosts = 1;
    [SerializeField] private int _maxIntruders = 2;

    [Header("UI elements")]
    [SerializeField] private GameObject _waitingLobby;
    [SerializeField] private GameObject _managers;
    [SerializeField] private Text _readyButton;
    [SerializeField] private Text _readyErrorText;
    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private Text _roomName;
    [SerializeField] private GameObject _playerListObject;
    [SerializeField] private Toggle _bourreauToggle;
    [SerializeField] private Toggle _spectreToggle;
    [SerializeField] private Toggle _intrusToggle;

    [Header("Character prefabs")]
    [SerializeField] private GameObject _bourreauPrefab;
    [SerializeField] private GameObject _spectrePrefab;
    [SerializeField] private GameObject _intruderPrefab;

    [Header("Tuto")]
    public GameObject bourreauTuto, spectreTuto, intruderTuto;
    private GameObject currentTuto;

    private readonly List<GameObject> _playerList = new List<GameObject>();

    private List<Transform> spawnPoint = new List<Transform>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        foreach (Transform child in _playerListObject.transform)
        {
            _playerList.Add(child.gameObject);
        }
    }

    private void Start()
    {
        spawnPoint = NetworkManager.singleton.startPositions;
    }

    public GameObject GetPlayerListObject()
    {
        return _playerListObject;
    }

    public void SetRoomNameText(string roomName)
    {
        _roomName.text = roomName;
    }

    public void SetNetId(string clientNetId)
    {
        _clientNetId = clientNetId;
    }

    public string GetNetId()
    {
        return _clientNetId;
    }

    private int GetMaxNumberOfType(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Undefined:
                return -1;
            case PlayerType.Exectioner:
                return _maxExecutioners;
            case PlayerType.Ghost:
                return _maxGhosts;
            case PlayerType.Intruder:
                return _maxIntruders;
            default:
                return -1;
        }
    }

    private int GetCurrentNumberOfType(PlayerType type)
    {
        var amount = 0;

        foreach (var pair in Players)
        {
            var player = pair.Value;
            if (player.Type == type)
                amount++;
        }

        return amount;
    }

    private void ToggleType(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Exectioner:
                _bourreauToggle.isOn = false;
                break;
            case PlayerType.Ghost:
                _spectreToggle.isOn = false;
                break;
            case PlayerType.Intruder:
                _intrusToggle.isOn = false;
                break;
        }
    }

    private bool IsToggleOff(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Exectioner:
                return _bourreauToggle.isOn;
            case PlayerType.Ghost:
                return _spectreToggle.isOn;
            case PlayerType.Intruder:
                return _intrusToggle.isOn;
            default:
                return false;
        }
    }

    public void SetPlayerType(int iType)
    {

        var type = (PlayerType)iType;
        var currClient = GetPlayer("Player " + _clientNetId);

        if (!IsToggleOff(type))
        {
            if (_bourreauToggle.isOn || _spectreToggle.isOn || _intrusToggle.isOn) return;

            if (_ready)
                SetReady();

            _readyErrorText.text = "Merci de choisir un personnage";

            _type = PlayerType.Undefined;
            currClient.SetType(_type);

            return;
        }

        if (GetCurrentNumberOfType(type) >= GetMaxNumberOfType(type))
        {
            ToggleType(type);
            switch (type)
            {
                case PlayerType.Exectioner:
                    _readyErrorText.text = "Il y a déjà un Bourreau";
                    break;
                case PlayerType.Ghost:
                    _readyErrorText.text = "Il y a déjà un Spectre";
                    break;
                case PlayerType.Intruder:
                    _readyErrorText.text = "Il y a déjà suffisament d'intrus";
                    break;
                default:
                    _readyErrorText.text = "";
                    break;
            }
            return;
        }

        switch (type)
        {
            case PlayerType.Exectioner:
                ToggleType(PlayerType.Ghost);
                ToggleType(PlayerType.Intruder);
                break;
            case PlayerType.Ghost:
                ToggleType(PlayerType.Exectioner);
                ToggleType(PlayerType.Intruder);
                break;
            case PlayerType.Intruder:
                ToggleType(PlayerType.Exectioner);
                ToggleType(PlayerType.Ghost);
                break;
        }

        _type = type;
        currClient.SetType(_type);
    }

    public void SetReady()
    {
        _readyErrorText.text = "";
        if (_type == PlayerType.Undefined)
        {
            _readyErrorText.text = "Merci de choisir un personnage";
            return;
        }

        _ready = !_ready;
        var currClient = GetPlayer("Player " + _clientNetId);
        currClient.SetReady(_ready);

        _readyButton.text = _ready ? "Not ready" : "Ready";
    }

    public void ReadyCheck()
    {
        if (_startGameButton == null)
            return;

        if (Players.Count != 4)
        {
            _roomReady = false;
            _startGameButton.SetActive(false);
            return;
        }

        foreach (var pair in Players)
        {
            var player = pair.Value;

            if (player.Ready) continue;

            _roomReady = false;
            _startGameButton.SetActive(false);
            return;
        }

        _roomReady = true;
        _startGameButton.SetActive(true);
    }

    public void StartGameButton()
    {
        if (!_roomReady)
        {
            Debug.Log("This should not happen");
            _readyErrorText.text = "Something went wrong, don't ask me I don't know what happened";
            return;
        }

        var hostClient = GetPlayer("Player " + _hostNetId);
        hostClient.StartGame();
    }

    public GameObject GetGameObjectFromType(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Undefined:
                return null;
            case PlayerType.Exectioner:
                return _bourreauPrefab;
            case PlayerType.Ghost:
                return _spectrePrefab;
            case PlayerType.Intruder:
                return _intruderPrefab;
            default:
                return null;
        }
    }

    public void StartGame()
    {
        var currClient = GetPlayer("Player " + _clientNetId);

        if (currClient.isServer)
        {
            var networkManager = NetworkManager.singleton;
            var prefab = GetGameObjectFromType(currClient.Type);

            if (prefab != null)
            {
                var spawnedCharacter = Instantiate(prefab, spawnPoint[0].position, Quaternion.identity);
                SuprSpawnPoint();
                NetworkServer.ReplacePlayerForConnection(currClient.connectionToClient, spawnedCharacter, currClient.playerControllerId);
                NetworkServer.Spawn(spawnedCharacter);
            }

            _managers.SetActive(true);
            NetworkServer.Spawn(_managers.GetComponentInChildren<Script_UI_InGame_Manager>().gameObject);
        }
        else
        {
            currClient.PlayerSpawn(currClient.gameObject, (int) currClient.Type);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _waitingLobby.SetActive(false);
        Script_UI_InGame_Manager.Instance.StartGame();
    }

    public void LeaveRoom()
    {
        var currClient = GetPlayer("Player " + _clientNetId);
        currClient.Disconnect();
    }

    private const string PlayerIdPrefix = "Player ";

    private static readonly Dictionary<string, LobbyPlayer> Players = new Dictionary<string, LobbyPlayer>();

    private static string _hostNetId = "";

    public static void RegisterPlayer(string netId, LobbyPlayer lobbyPlayer, bool isHost)
    {
        var lobbyPlayerId = PlayerIdPrefix + netId;

        if (Players.ContainsKey(lobbyPlayerId))
            Players.Remove(lobbyPlayerId);

        Players.Add(lobbyPlayerId, lobbyPlayer);
        lobbyPlayer.transform.name = lobbyPlayerId;

        if (isHost)
            _hostNetId = netId;

        var lobbyHost = GetPlayer("Player " + _hostNetId);
        if (lobbyHost == null) return;

        var players = lobbyHost.LobbyPlayers;
        if (!players.ContainsKey(lobbyPlayer.Username))
            lobbyHost.LobbyPlayers.Add(lobbyPlayer.Username, lobbyPlayer);
        else
            lobbyHost.KickPlayer(lobbyPlayer);

        Instance.UpdateLobbyList();
    }

    public static void UnRegisterPlayer(string lobbyPlayerId)
    {
        var disconnectedPlayer = GetPlayer(lobbyPlayerId);
        if (disconnectedPlayer == null) return;

        var host = GetPlayer("Player " + _hostNetId);
        if (host == null) return;

        var players = host.LobbyPlayers;
        players.Remove(disconnectedPlayer.Username);
        Players.Remove(lobbyPlayerId);

        Instance.UpdateLobbyList();

        if (host.IsServerSide)
           Instance.ReadyCheck();
    }

    public static LobbyPlayer GetPlayer(string lobbyPlayerId)
    {
        return Players.ContainsKey(lobbyPlayerId) ? Players[lobbyPlayerId] : null;
    }

    public void UpdateLobbyList()
    {
        var i = 0;

        var host = GetPlayer("Player " + _hostNetId);
        if (host == null) return;

        var players = host.LobbyPlayers;

        foreach (var playerBox in _playerList)
        {
            if (playerBox == null) continue;

            playerBox.GetComponentInChildren<Image>().color = Color.gray;
            playerBox.GetComponentInChildren<Text>().text = "Waiting for players...";
        }

        foreach (var pair in players)
        {
            var username = pair.Key;
            var player = pair.Value;

            if (_playerList[i] == null) continue;

            _playerList[i].GetComponentInChildren<Image>().color = player.Ready ? Color.green : Color.red;
            _playerList[i].GetComponentInChildren<Text>().text = username;
            i++;
        }
    }

    public void ShowTuto(GameObject toShow)
    {
        toShow.SetActive(true);

        if(currentTuto != null)
        {
            currentTuto.SetActive(false);
        }

        currentTuto = toShow;
    }

    public List<Transform> GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void SuprSpawnPoint()
    {
        spawnPoint.RemoveAt(0);
    }
}
