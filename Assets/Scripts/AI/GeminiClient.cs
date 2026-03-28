using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiClient : MonoBehaviour
{
    [Header("API Config")]
    [SerializeField]
    public string apiKey = "AIzaSyB1DP6HBtd8bIgwR_7b3upgAOFQAoybOp0";

    private const string API_URL =
        "https://generativelanguage.googleapis.com" +
        "/v1beta/models/gemini-2.5-flash" +
        ":generateContent?key=";

    private float lastCallTime = -60f;
    private float cooldownSeconds = 15f;

    public IEnumerator GetExplanation(
        string partName,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        float timeSinceLast =
            Time.time - lastCallTime;
        if (timeSinceLast < cooldownSeconds)
        {
            int wait = Mathf.CeilToInt(
                cooldownSeconds - timeSinceLast);
            onError("Wait " + wait + "s before tapping again.");
            yield break;
        }

        lastCallTime = Time.time;

        string prompt = BuildPrompt(partName);
        string jsonBody = BuildRequestBody(prompt);

        Debug.Log("Calling Gemini for: " + partName);

        using (UnityWebRequest req =
            new UnityWebRequest(
                API_URL + apiKey, "POST"))
        {
            byte[] bodyRaw =
                System.Text.Encoding.UTF8
                .GetBytes(jsonBody);
            req.uploadHandler =
                new UploadHandlerRaw(bodyRaw);
            req.downloadHandler =
                new DownloadHandlerBuffer();
            req.SetRequestHeader(
                "Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result ==
                UnityWebRequest.Result.Success)
            {
                string json =
                    req.downloadHandler.text;
                Debug.Log("Gemini SUCCESS: " +
                    json.Substring(0,
                    Mathf.Min(200, json.Length)));
                string result = ExtractText(json);
                onSuccess(result);
            }
            else
            {
                long code = req.responseCode;
                string body =
                    req.downloadHandler.text;
                Debug.LogError(
                    "GEMINI ERROR " + code +
                    ": " + body);
                if (code == 429)
                    onError(
                        "Rate limit. Wait 60s.");
                else if (code == 403)
                    onError("API key invalid.");
                else
                    onError(
                        "Error " + code +
                        ". Check console.");
            }
        }
    }

    public IEnumerator SendChat(
        string fullPrompt,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        string jsonBody =
            BuildRequestBody(fullPrompt);

        using (UnityWebRequest req =
            new UnityWebRequest(
                API_URL + apiKey, "POST"))
        {
            byte[] bodyRaw =
                System.Text.Encoding.UTF8
                .GetBytes(jsonBody);
            req.uploadHandler =
                new UploadHandlerRaw(bodyRaw);
            req.downloadHandler =
                new DownloadHandlerBuffer();
            req.SetRequestHeader(
                "Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result ==
                UnityWebRequest.Result.Success)
            {
                string result = ExtractText(
                    req.downloadHandler.text);
                onSuccess(result);
            }
            else
            {
                onError("Could not connect. " +
                    "Error: " + req.responseCode);
            }
        }
    }

    private string BuildPrompt(string partName)
    {
        return "Explain the " + partName +
               " of the human brain in exactly " +
               "3 simple sentences for a first-year " +
               "medical student. Include: " +
               "(1) where it is located, " +
               "(2) its main function, " +
               "(3) one interesting clinical fact. " +
               "Be concise and clear.";
    }

    public string BuildRequestBody(string prompt)
    {
        prompt = prompt
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r");
        return "{\"contents\":[{\"parts\":" +
               "[{\"text\":\"" +
               prompt + "\"}]}]}";
    }

    public string ExtractText(string json)
    {
        try
        {
            string key = "\"text\": \"";
            int start = json.IndexOf(key);
            if (start == -1)
            {
                Debug.LogError(
                    "text key not found: " + json);
                return "Could not parse response.";
            }
            start += key.Length;
            int end = json.IndexOf("\"", start);
            if (end == -1)
                return "Could not parse response.";
            return json.Substring(start, end - start)
                .Replace("\\n", "\n")
                .Replace("\\\"", "\"")
                .Replace("\\'", "'");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Parse error: " + e.Message);
            return "Error reading response.";
        }
    }
}