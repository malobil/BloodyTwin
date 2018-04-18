using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{

    public GameObject LoginObject;

    public InputField InputLoginUsername;

    public Text LoginError;

    private void Start()
    {
        BlankErrors();
    }

    private void BlankErrors()
    {
        LoginError.text = "";
    }

    public void LoginLoginButton()
    {
        if (InputLoginUsername.text != "")
        {
            if (InputLoginUsername.text.Contains("-"))
                LoginError.text = "Unsupported Symbol '-'";
            else
            {
                AccountManager.Instance.Login(InputLoginUsername.text);
            }
        }
        else
        {
            LoginError.text = "Field Blank!";
        }
    }

    public void LoginQuitButton()
    {
        Application.Quit();
    }
}