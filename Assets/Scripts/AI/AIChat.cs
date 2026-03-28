using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIChat : MonoBehaviour
{
    [Header("References")]
    public GeminiClient geminiClient;
    public GameObject chatPanel;
    public TMP_InputField inputField;
    public Transform messageContainer;
    public GameObject userMessagePrefab;
    public GameObject aiMessagePrefab;
    public ScrollRect scrollRect;
    public TMP_Text chatButtonText;

    [Header("Context")]
    public string currentOrganContext =
        "human brain";

    private List<string> conversationHistory =
        new List<string>();
    private bool isChatOpen = false;
    private GameObject lastAIBubble;

    void Start()
    {
        if (chatPanel != null)
            chatPanel.SetActive(false);
    }

    public void SetOrganContext(string organName)
    {
        currentOrganContext = organName;
        if (isChatOpen)
            AddSystemMessage(
                "Now discussing: " + organName);
    }

    public void ToggleChat()
    {
        isChatOpen = !isChatOpen;
        if (chatPanel != null)
            chatPanel.SetActive(isChatOpen);
        if (chatButtonText != null)
            chatButtonText.text =
                isChatOpen ? "Close Chat" : "Ask AI";
        if (isChatOpen && inputField != null)
            inputField.Select();
    }

    public void SendMessage()
    {
        if (inputField == null) return;
        string userText = inputField.text.Trim();
        if (string.IsNullOrEmpty(userText)) return;

        inputField.text = "";
        AddUserMessage(userText);
        conversationHistory.Add(
            "Student: " + userText);

        StartCoroutine(GetAIResponse(userText));
    }

    public void OnInputEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
            SendMessage();
    }

    IEnumerator GetAIResponse(string userMessage)
    {
        lastAIBubble = AddAIMessage("...");

        string fullPrompt = BuildFullPrompt(
            userMessage);

        yield return StartCoroutine(
            geminiClient.SendChat(
                fullPrompt,
                (response) =>
                {
                    UpdateBubble(
                        lastAIBubble, response);
                    conversationHistory.Add(
                        "Teacher: " + response);
                    ScrollToBottom();
                },
                (error) =>
                {
                    UpdateBubble(
                        lastAIBubble, error);
                    ScrollToBottom();
                }
            )
        );
    }

    string BuildFullPrompt(string userMessage)
    {
        string history = "";
        int start = Mathf.Max(
            0, conversationHistory.Count - 6);
        for (int i = start;
             i < conversationHistory.Count; i++)
            history += conversationHistory[i] + "\n";

        return
            "You are a friendly anatomy teacher " +
            "helping a medical student learn about " +
            "the " + currentOrganContext + ".\n" +
            "Keep answers under 4 sentences.\n" +
            "Be clear, accurate, and encouraging.\n\n" +
            (history.Length > 0
                ? "Conversation so far:\n" +
                  history + "\n"
                : "") +
            "Student asks: " + userMessage;
    }

    void AddUserMessage(string text)
    {
        if (userMessagePrefab == null ||
            messageContainer == null) return;
        GameObject bubble = Instantiate(
            userMessagePrefab, messageContainer);
        TMP_Text tmp = bubble
            .GetComponentInChildren<TMP_Text>();
        if (tmp != null) tmp.text = text;
        ScrollToBottom();
    }

    GameObject AddAIMessage(string text)
    {
        if (aiMessagePrefab == null ||
            messageContainer == null) return null;
        GameObject bubble = Instantiate(
            aiMessagePrefab, messageContainer);
        TMP_Text tmp = bubble
            .GetComponentInChildren<TMP_Text>();
        if (tmp != null) tmp.text = text;
        ScrollToBottom();
        return bubble;
    }

    void UpdateBubble(
        GameObject bubble, string text)
    {
        if (bubble == null) return;
        TMP_Text tmp = bubble
            .GetComponentInChildren<TMP_Text>();
        if (tmp != null) tmp.text = text;
    }

    void AddSystemMessage(string text)
    {
        AddAIMessage(
            "[Context switched: " + text + "]");
    }

    void ScrollToBottom()
    {
        StartCoroutine(ScrollNextFrame());
    }

    IEnumerator ScrollNextFrame()
    {
        yield return new WaitForEndOfFrame();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition
                = 0f;
    }
}