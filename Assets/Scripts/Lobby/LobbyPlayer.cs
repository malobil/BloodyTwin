using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkBehaviour
{
    public Dictionary<string, LobbyPlayer> LobbyPlayers = new Dictionary<string, LobbyPlayer>();

    [SyncVar] public string Username;
    [SyncVar] public bool Ready;
    [SyncVar] public LobbyManager.PlayerType Type = LobbyManager.PlayerType.Undefined;

    [SyncVar] private string _roomName;
    [SyncVar] private bool _isHost;
    [SyncVar] private bool _hasBeenKicked;

    public bool IsServerSide { get; private set; }

    public override void OnStartClient()
    {
        StartCoroutine(SetupClient(GetComponent<LobbyPlayer>()));
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (HostGame.Instance != null && HostGame.Instance.IsHost())
        {
            _isHost = true;
            IsServerSide = true;
            _roomName = HostGame.Instance.GetRoomName();
            Username = AccountManager.Instance.GetUsername();
            Destroy(HostGame.Instance.gameObject);
        }

        LobbyManager.Instance.SetNetId(GetComponent<NetworkIdentity>().netId.ToString());
        CmdSetUsername(AccountManager.Instance.GetUsername());
    }

    public void Disconnect()
    {
        if (isServer)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
    }

    private void OnDestroy()
    {
        if (!_hasBeenKicked)
            LobbyManager.UnRegisterPlayer(transform.name);
    }

    public void KickPlayer(LobbyPlayer lobbyPlayer)
    {
        if (!isServer)
            return;

        var kickedPlayerIdentity = lobbyPlayer.gameObject.GetComponent<NetworkIdentity>();
        RpcKickPlayer(kickedPlayerIdentity.netId.ToString());
        kickedPlayerIdentity.connectionToClient.Disconnect();
    }

    [ClientRpc]
    public void RpcKickPlayer(string clientNetId)
    {
        var kickedPlayer = LobbyManager.GetPlayer("Player " + clientNetId);
        if (kickedPlayer == null)
            return;

        kickedPlayer._hasBeenKicked = true;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!isServer)
            return;

        if (Input.GetKeyDown(KeyCode.O))
            StartGame();
    }

    private IEnumerator SetupRoomName()
    {
        while (string.IsNullOrEmpty(_roomName))
            yield return null;
        LobbyManager.Instance.SetRoomNameText(_roomName);
        yield return null;
    }

    private IEnumerator SetupClient(LobbyPlayer lobbyPlayer)
    {
        while (string.IsNullOrEmpty(lobbyPlayer.Username))
            yield return null;
        PlayerConnected(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<LobbyPlayer>(), _isHost);
        if (_isHost)
            StartCoroutine(SetupRoomName());
        yield return null;
    }

    [Command]
    private void CmdSetUsername(string username)
    {
        RpcSetUsername(username);
    }

    [ClientRpc]
    private void RpcSetUsername(string username)
    {
        Username = username;
    }

    private void PlayerConnected(string clientNetId, LobbyPlayer lobbyPlayer, bool isHost)
    {
        LobbyManager.RegisterPlayer(clientNetId, lobbyPlayer, isHost);
    }

    public void SetReady(bool ready)
    {
        CmdSetReady(ready);
    }

    [Command]
    private void CmdSetReady(bool ready)
    {
        RpcSetReady(ready);
    }

    [ClientRpc]
    private void RpcSetReady(bool ready)
    {
        Ready = ready;
        LobbyManager.Instance.UpdateLobbyList();
        if (isServer)
            LobbyManager.Instance.ReadyCheck();
    }

    public void SetType(LobbyManager.PlayerType type)
    {
        CmdSetType(type);
    }

    [Command]
    private void CmdSetType(LobbyManager.PlayerType type)
    {
        RpcSetType(type);
    }

    [ClientRpc]
    private void RpcSetType(LobbyManager.PlayerType type)
    {
        Type = type;
    }

    public void StartGame()
    {
        RpcStartGame();
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        LobbyManager.Instance.StartGame();
    }

    public void PlayerSpawn(GameObject client, int type)
    {
        CmdPlayerSpawn(client, type);
    }

    [Command]
    private void CmdPlayerSpawn(GameObject client, int type)
    {
        var lobbyClient = client.GetComponent<LobbyPlayer>();
        var prefab = LobbyManager.Instance.GetGameObjectFromType((LobbyManager.PlayerType)type);

        if (prefab == null)
            return;

        var networkManager = NetworkManager.singleton;
        var spawnedCharacter = Instantiate(prefab, networkManager.startPositions[Random.Range(0, networkManager.startPositions.Count - 1)].position, Quaternion.identity);

        NetworkServer.ReplacePlayerForConnection(lobbyClient.connectionToClient, spawnedCharacter, lobbyClient.playerControllerId);
        NetworkServer.Spawn(spawnedCharacter);
    }
}
