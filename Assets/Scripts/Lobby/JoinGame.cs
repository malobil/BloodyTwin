using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

using System.Collections.Generic;

public class JoinGame : MonoBehaviour
{
    private readonly List<GameObject> _roomList = new List<GameObject>();

    [SerializeField]
    private Text _status;

    [SerializeField]
    private GameObject _roomListItemPrefab;

    [SerializeField]
    private Transform _roomListParent;

    private NetworkManager _networkManager;

    private void OnEnable()
    {
        _networkManager = NetworkManager.singleton;
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        _networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        _status.text = "Loading...";
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (_status == null)
            return;

        _status.text = "";

        if (!success)
        {
            _status.text = "Failed to retrieve rooms.";
            return;
        }

        if (matchList.Count == 0)
        {
            _status.text = "No rooms found.";
            return;
        }

        foreach (var match in matchList)
        {
            var roomListItemGo = Instantiate(_roomListItemPrefab);
            roomListItemGo.transform.SetParent(_roomListParent);
            roomListItemGo.transform.localScale = new Vector3(1, 1, 1);

            var roomListItem = roomListItemGo.GetComponent<RoomListItem>();
            if (roomListItem != null)
                roomListItem.Setup(match, JoinRoom);

            _roomList.Add(roomListItemGo);
        }
    }

    private void ClearRoomList()
    {
        foreach (var room in _roomList)
        {
            Destroy(room);
        }

        _roomList.Clear();
    }

    private void JoinRoom(MatchInfoSnapshot match)
    {
        _networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, _networkManager.OnMatchJoined);
        ClearRoomList();
        _status.text = "Joining " + match.name + "...";
    }
}
