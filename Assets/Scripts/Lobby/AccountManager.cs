using UnityEngine;
using UnityEngine.Networking;

public class AccountManager : MonoBehaviour
{

    public static AccountManager Instance;

    public GameObject LoginObject;
    public GameObject BrowserObject;
    public GameObject HostGameObject;
    public GameObject JoinGameObject;

    private string _username;

    private bool _loggedIn = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);

        if (NetworkManager.singleton.matchMaker == null) return;

        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopMatchMaker();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public string GetUsername()
    {
        return _username;
    }

    public bool IsLoggedIn()
    {
        return _loggedIn;
    }

    public void Login(string username)
    {
        _username = username;
        _loggedIn = true;
        LoginObject.SetActive(false);
        BrowserObject.SetActive(true);
        HostGameObject.SetActive(true);
        JoinGameObject.SetActive(true);
    }

    public void Logout()
    {
        _username = "";
        _loggedIn = false;
        LoginObject.SetActive(true);
        BrowserObject.SetActive(false);
        HostGameObject.SetActive(false);
        JoinGameObject.SetActive(false);
    }
}