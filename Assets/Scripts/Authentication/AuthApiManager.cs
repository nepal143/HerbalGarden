using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class APIManager : MonoBehaviour
{
    private const string BASE_URL = "http://localhost:3000"; // Replace with your actual server URL
    private const string TOKEN_KEY = "auth_token";

    public static APIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class RequestData
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string token;
    }

    public void Register(string email, string password, System.Action<string> callback)
    {
        StartCoroutine(RegisterCoroutine(email, password, callback));
    }

    private IEnumerator RegisterCoroutine(string email, string password, System.Action<string> callback)
    {
        string url = BASE_URL + "/register";
        var requestData = new RequestData { email = email, password = password };
        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke("User registered successfully");
            }
            else
            {
                callback?.Invoke($"Error: {request.error}");
            }
        }
    }

    public void Login(string email, string password, System.Action<string> callback)
    {
        StartCoroutine(LoginCoroutine(email, password, callback));
    }

    private IEnumerator LoginCoroutine(string email, string password, System.Action<string> callback)
    {
        string url = BASE_URL + "/login";
        var requestData = new RequestData { email = email, password = password };
        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(response.token))
                {
                    PlayerPrefs.SetString(TOKEN_KEY, response.token);
                    PlayerPrefs.Save();
                    callback?.Invoke("Login successful");
                }
                else
                {
                    callback?.Invoke("Login failed: No token received");
                }
            }
            else
            {
                callback?.Invoke($"Error: {request.error}");
            }
        }
    }

    public void CallProtectedAPI(System.Action<string> callback)
    {
        StartCoroutine(ProtectedAPICoroutine(callback));
    }

    private IEnumerator ProtectedAPICoroutine(System.Action<string> callback)
    {
        string url = BASE_URL + "/protected";
        string token = PlayerPrefs.GetString(TOKEN_KEY, "");

        if (string.IsNullOrEmpty(token))
        {
            callback?.Invoke("No token found. Please log in.");
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(request.downloadHandler.text);
            }
            else
            {
                callback?.Invoke($"Error: {request.error}");
            }
        }
    }
}
