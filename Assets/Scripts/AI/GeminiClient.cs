using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiClient : MonoBehaviour
{
    [Header("API Config")]
    [SerializeField]
    private string apiKey = "YOUR_GEMINI_KEY_HERE";

    private const string API_URL =
        "https://generativelanguage.googleapis.com" +
        "/v1beta/models/gemini-2.0-flash:generateContent?key=";

    public IEnumerator GetExplanation(
        string partName,
        System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        string prompt = BuildPrompt(partName);
        string jsonBody = BuildRequestBody(prompt);

        using (UnityWebRequest request = new UnityWebRequest(
            API_URL + apiKey, "POST"))
        {
            byte[] bodyRaw =
                System.Text.Encoding.UTF8.GetBytes(jsonBody);

            request.uploadHandler =
                new UploadHandlerRaw(bodyRaw);
            request.downloadHandler =
                new DownloadHandlerBuffer();
            request.SetRequestHeader(
                "Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result ==
                UnityWebRequest.Result.Success)
            {
                string response =
                    request.downloadHandler.text;
                string extracted =
                    ExtractTextFromResponse(response);
                onSuccess(extracted);
            }
            else
            {
                onError("Could not load explanation. " +
                       "Check your connection.");
            }
        }
    }

    private string BuildPrompt(string partName)
    {
        return $"Explain the {partName} of the human brain " +
               "in exactly 3 simple sentences. " +
               "Write for a medical student. " +
               "Include its location, main function, " +
               "and one interesting fact. " +
               "Be concise and clear.";
    }

    private string BuildRequestBody(string prompt)
    {
        // Escape special characters
        prompt = prompt.Replace("\"", "\\\"")
                       .Replace("\n", "\\n");

        return "{\"contents\":[{\"parts\":" +
               "[{\"text\":\"" + prompt + "\"}]}]}";
    }

    private string ExtractTextFromResponse(string json)
    {
        try
        {
            // Find the text field in the JSON response
            string searchKey = "\"text\": \"";
            int startIndex = json.IndexOf(searchKey);

            if (startIndex == -1)
                return "Could not parse response.";

            startIndex += searchKey.Length;
            int endIndex = json.IndexOf("\"", startIndex);

            if (endIndex == -1)
                return "Could not parse response.";

            string extracted = json.Substring(
                startIndex, endIndex - startIndex);

            // Unescape newlines
            return extracted.Replace("\\n", "\n");
        }
        catch
        {
            return "Error reading response.";
        }
    }
}