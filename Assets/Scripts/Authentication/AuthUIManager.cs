using UnityEngine;
using TMPro; // Make sure TMP is installed
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField usernameInput;  // TMP Input for username
    public TMP_InputField passwordInput;  // TMP Input for password
    public Button registerButton;         // Button for registration
    public Button loginButton;            // Button for login
    public TMP_Text feedbackText;         // Text field to show feedback messages

    private void Start()
    {
        // Check if the user is already logged in via token
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("auth_token", "")))
        {
            LoginWithToken();
        }

        // Add listeners for buttons
        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(Login);
    }

    // Register the user
    private void Register()
    {
        string email = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please enter both username and password.";
            return;
        }

        APIManager.Instance.Register(email, password, (response) =>
        {
            feedbackText.text = response;
        });
    }

    // Login the user
    private void Login()
    {
        string email = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please enter both username and password.";
            return;
        }

        APIManager.Instance.Login(email, password, (response) =>
        {
            feedbackText.text = response;
        });
    }

    // Log in directly using the saved token
    private void LoginWithToken()
    {
        APIManager.Instance.CallProtectedAPI((response) =>
        {
            if (response.StartsWith("Error"))
            {
                feedbackText.text = "Session expired. Please log in again.";
            }
            else
            {
                feedbackText.text = "Welcome back!";
            }
        });
    }
}
