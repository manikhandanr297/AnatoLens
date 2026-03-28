using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiClient : MonoBehaviour
{
    [Header("API Config")]
    [SerializeField]
    private string apiKey = "AIzaSyB1DP6HBtd8bIgwR_7b3upgAOFQAoybOp0";

    private const string API_URL =
        "https://generativelanguage.googleapis.com" +
        "/v1beta/models/gemini-2.5-flash:generateContent?key=";

    [Header("Rate Limiting")]
    private float lastCallTime = -60f;
    private float cooldownSeconds = 15f;

    public IEnumerator GetExplanation(
        string partName,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        // Cooldown check
        float timeSinceLast = Time.time - lastCallTime;
        if (timeSinceLast < cooldownSeconds)
        {
            int waitTime = Mathf.CeilToInt(
                cooldownSeconds - timeSinceLast);
            onError("Please wait " + waitTime +
                    "s before tapping again.");
            yield break;
        }

        lastCallTime = Time.time;

        string prompt = BuildPrompt(partName);
        string jsonBody = BuildRequestBody(prompt);

        Debug.Log("Calling Gemini for: " + partName);
        Debug.Log("URL: " + API_URL +
                  apiKey.Substring(0, 5) + "...");

        using (UnityWebRequest req =
            new UnityWebRequest(API_URL + apiKey, "POST"))
        {
            byte[] bodyRaw =
                System.Text.Encoding.UTF8.GetBytes(jsonBody);

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
                string json = req.downloadHandler.text;
                Debug.Log("Gemini SUCCESS: " +
                    json.Substring(0,
                        Mathf.Min(200, json.Length)));
                string result = ExtractText(json);
                onSuccess(result);
            }
            else
            {
                string errorBody =
                    req.downloadHandler.text;
                long code = req.responseCode;
                Debug.LogError("=== GEMINI ERROR ===");
                Debug.LogError("Response Code: " + code);
                Debug.LogError("Error: " + req.error);
                Debug.LogError("Body: " + errorBody);

                if (code == 429)
                {
                    onError("Rate limit hit. " +
                            "Wait 60s and try again.");
                }
                else if (code == 403)
                {
                    onError("API key invalid " +
                            "or not authorized.");
                }
                else
                {
                    onError("Error " + code +
                            ": check Unity console.");
                }
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

    private string BuildRequestBody(string prompt)
    {
        prompt = prompt
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r");

        return "{\"contents\":[{\"parts\":" +
               "[{\"text\":\"" + prompt + "\"}]}]}";
    }

    private string ExtractText(string json)
    {
        try
        {
            string key = "\"text\": \"";
            int start = json.IndexOf(key);
            if (start == -1)
            {
                Debug.LogError(
                    "text key not found in: " + json);
                return "Could not parse response.";
            }

            start += key.Length;
            int end = json.IndexOf("\"", start);
            if (end == -1)
                return "Could not parse response.";

            string result = json.Substring(
                start, end - start);

            return result
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