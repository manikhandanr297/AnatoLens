using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ChatMessage
{
    public string role;
    public string text;
    public ChatMessage(string r, string t)
    { role = r; text = t; }
}

public class ChatManager : MonoBehaviour
{
    [Header("References")]
    public GeminiClient geminiClient;
    public GameObject chatPanel;
    public ScrollRect scrollRect;
    public Transform messageContainer;
    public TMP_InputField inputField;
    public Button sendButton;

    [Header("Prefabs")]
    public GameObject userBubble;
    public GameObject aiBubble;

    [Header("Split Screen")]
    public RectTransform arViewport;

    private string currentPart = "brain";
    private string currentContext = "";
    private List<ChatMessage> history =
        new List<ChatMessage>();
    private bool waiting = false;

    void Start()
    {
        if (chatPanel != null)
            chatPanel.SetActive(false);
    }

    public void SetContext(
        string part, string context)
    {
        currentPart = part;
        currentContext = context;
    }

    public void OpenChat()
    {
        history.Clear();
        ClearMessages();

        string system =
            "You are AnatoLens AI, a friendly " +
            "anatomy tutor. Student is studying " +
            currentPart + ". " +
            (string.IsNullOrEmpty(currentContext)
                ? ""
                : "Context: " + currentContext +
                  ". ") +
            "Keep answers under 4 sentences. " +
            "Be clear and encouraging.";

        history.Add(new ChatMessage("user", system));
        history.Add(new ChatMessage("model",
            "Hi! Ask me anything about " +
            currentPart + "!"));

        AddBubble(aiBubble,
            "Hi! I am ready to help you " +
            "learn about " + currentPart + "!");

        chatPanel.SetActive(true);
        EnterSplitMode();
        inputField?.Select();
    }

    public void CloseChat()
    {
        chatPanel.SetActive(false);
        ExitSplitMode();
    }

    void EnterSplitMode()
    {
        if (arViewport == null) return;
        arViewport.anchorMin = new Vector2(0, 0);
        arViewport.anchorMax =
            new Vector2(0.45f, 1);
        arViewport.offsetMin = Vector2.zero;
        arViewport.offsetMax = Vector2.zero;
    }

    void ExitSplitMode()
    {
        if (arViewport == null) return;
        arViewport.anchorMin = new Vector2(0, 0);
        arViewport.anchorMax = new Vector2(1, 1);
        arViewport.offsetMin = Vector2.zero;
        arViewport.offsetMax = Vector2.zero;
    }

    public void OnSendPressed()
    {
        if (waiting || inputField == null) return;
        string text = inputField.text.Trim();
        if (string.IsNullOrEmpty(text)) return;
        inputField.text = "";
        AddBubble(userBubble, text);
        history.Add(new ChatMessage("user", text));
        StartCoroutine(GetResponse());
    }

    IEnumerator GetResponse()
    {
        waiting = true;
        if (sendButton != null)
            sendButton.interactable = false;

        AddBubble(aiBubble, "...");

        string prompt = BuildPrompt();

        bool done = false;
        string result = "";

        yield return StartCoroutine(
            geminiClient.SendChat(prompt,
                r => { result = r; done = true; },
                e => { result = e; done = true; }));

        UpdateLastBubble(result);
        history.Add(new ChatMessage(
            "model", result));

        waiting = false;
        if (sendButton != null)
            sendButton.interactable = true;

        StartCoroutine(ScrollBottom());
    }

    string BuildPrompt()
    {
        var sb =
            new System.Text.StringBuilder();
        sb.Append(
            "You are AnatoLens AI anatomy tutor. ");
        sb.Append(
            "Keep answers under 4 sentences. ");
        sb.Append("Conversation:\n");

        int start = Mathf.Max(
            0, history.Count - 8);
        for (int i = start;
             i < history.Count; i++)
        {
            sb.Append(history[i].role == "user"
                ? "Student: " : "Teacher: ");
            sb.Append(history[i].text);
            sb.Append("\n");
        }
        sb.Append("Teacher:");
        return sb.ToString();
    }

    void AddBubble(
        GameObject prefab, string text)
    {
        if (prefab == null ||
            messageContainer == null) return;
        GameObject b = Instantiate(
            prefab, messageContainer);
        b.GetComponentInChildren<TMP_Text>()
            .text = text;
        StartCoroutine(ScrollBottom());
    }

    void UpdateLastBubble(string text)
    {
        if (messageContainer == null ||
            messageContainer.childCount == 0)
            return;
        Transform last = messageContainer
            .GetChild(
                messageContainer.childCount - 1);
        TMP_Text t =
            last.GetComponentInChildren<TMP_Text>();
        if (t != null) t.text = text;
    }

    void ClearMessages()
    {
        if (messageContainer == null) return;
        foreach (Transform c in messageContainer)
            Destroy(c.gameObject);
    }

    IEnumerator ScrollBottom()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        if (scrollRect != null)
            scrollRect
                .verticalNormalizedPosition = 0f;
    }
}