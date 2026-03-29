using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiClient : MonoBehaviour
{
    [Header("API")]
    public string apiKey = "YOUR_GEMINI_KEY";

    private const string URL =
        "https://generativelanguage.googleapis.com" +
        "/v1beta/models/gemini-2.5-flash" +
        ":generateContent?key=";

    private float lastCallTime = -60f;
    private const float COOLDOWN = 15f;

    public IEnumerator GetExplanation(
        string partName,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        float since = Time.time - lastCallTime;
        if (since < COOLDOWN)
        {
            onError("Wait " +
                Mathf.CeilToInt(COOLDOWN - since) +
                "s before tapping again.");
            yield break;
        }
        lastCallTime = Time.time;

        string prompt =
            "Explain the " + partName +
            " of the human brain in exactly 3 " +
            "simple sentences for a first-year " +
            "medical student. Include: " +
            "(1) where it is located, " +
            "(2) its main function, " +
            "(3) one interesting clinical fact.";

        yield return StartCoroutine(
            PostRequest(prompt, onSuccess, onError));
    }

    public IEnumerator SendChat(
        string prompt,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        yield return StartCoroutine(
            PostRequest(prompt, onSuccess, onError));
    }

    IEnumerator PostRequest(
        string prompt,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        string clean = prompt
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "");

        string body =
            "{\"contents\":[{\"parts\":" +
            "[{\"text\":\"" + clean + "\"}]}]}";

        using (UnityWebRequest req =
            new UnityWebRequest(
                URL + apiKey, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(
                System.Text.Encoding.UTF8
                .GetBytes(body));
            req.downloadHandler =
                new DownloadHandlerBuffer();
            req.SetRequestHeader(
                "Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result ==
                UnityWebRequest.Result.Success)
            {
                onSuccess(
                    ExtractText(
                        req.downloadHandler.text));
            }
            else
            {
                Debug.LogError("Gemini " +
                    req.responseCode + ": " +
                    req.downloadHandler.text);
                if (req.responseCode == 429)
                    onError("Rate limit. Wait 60s.");
                else
                    onError("Error " +
                        req.responseCode);
            }
        }
    }

    public string ExtractText(string json)
    {
        try
        {
            string k = "\"text\": \"";
            int s = json.IndexOf(k) + k.Length;
            int e = json.IndexOf("\"", s);
            return json.Substring(s, e - s)
                .Replace("\\n", "\n")
                .Replace("\\\"", "\"");
        }
        catch { return "Could not parse response."; }
    }
}