using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiClient : MonoBehaviour
{
    [SerializeField]
    private string apiKey = "AIzaSyDaAliL5I50g6iCxsqvPGSF_e7awRrvDoA";

    private const string API_URL =
         "https://generativelanguage.googleapis.com" +
    "/v1beta/models/gemini-1.5-flash:generateContent?key=";

    public IEnumerator GetExplanation(
        string partName,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        string prompt =
            $"Explain the {partName} of the human brain " +
            "in 3 simple sentences for a medical student. " +
            "Include location, function, and one fact.";

        prompt = prompt.Replace("\"", "\\\"")
                       .Replace("\n", "\\n");

        string body =
            "{\"contents\":[{\"parts\":" +
            "[{\"text\":\"" + prompt + "\"}]}]}";

        using (UnityWebRequest req =
            new UnityWebRequest(API_URL + apiKey, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(
                System.Text.Encoding.UTF8.GetBytes(body));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader(
                "Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result ==
                UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;
                string key = "\"text\": \"";
                int start = json.IndexOf(key) + key.Length;
                int end = json.IndexOf("\"", start);
                string result = json.Substring(
                    start, end - start)
                    .Replace("\\n", "\n");
                onSuccess(result);
            }
            else
            {
                Debug.LogError("Gemini Error: " +
                    req.responseCode + " - " +
                    req.downloadHandler.text);
                onError("AI unavailable. Error: " +
                    req.responseCode);
            }
        }
    }
}