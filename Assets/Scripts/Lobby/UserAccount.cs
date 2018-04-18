using UnityEngine;
using UnityEngine.UI;

public class UserAccount : MonoBehaviour
{

    public Text UsernameText;

    private void OnEnable()
    {
        if (AccountManager.Instance.IsLoggedIn())
            UsernameText.text = AccountManager.Instance.GetUsername();
    }

    public void LogOut()
    {
        if (AccountManager.Instance.IsLoggedIn())
            AccountManager.Instance.Logout();
    }
}
