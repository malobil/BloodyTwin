using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField] private uint _maxRoomSize = 4;

    private string _roomName;

    private bool _isCreatingRoom;

    private NetworkManager _networkManager;

    public static HostGame Instance;

    private bool _isHost = false;

    private void OnEnable()
    {
        _networkManager = NetworkManager.singleton;
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string roomName)
    {
        _roomName = roomName;
    }

    public string GetRoomName()
    {
        return _roomName;
    }

    public bool IsHost()
    {
        return _isHost;
    }

    public void CreateRoom()
    {
        if (_isCreatingRoom)
            return;

        if (string.IsNullOrEmpty(_roomName))
            return;

        _isCreatingRoom = true;
        DontDestroyOnLoad(gameObject);
        Instance = this;
        _isHost = true;
        Debug.Log("Creating Room: " + _roomName + " Max size: " + _maxRoomSize);
        _networkManager.matchMaker.CreateMatch(_roomName, _maxRoomSize, true, "", "", "", 0, 0, _networkManager.OnMatchCreate);
    }
}
