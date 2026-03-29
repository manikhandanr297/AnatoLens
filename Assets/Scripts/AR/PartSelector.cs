using UnityEngine;

public class PartSelector : MonoBehaviour
{
    [Header("References")]
    public UIManager uiManager;
    public GeminiClient geminiClient;
    public ChatManager chatManager;

    [Header("Config")]
    public string organId = "brain";
    public string customPartName = "";

    [Header("Highlight")]
    public Color highlightColor =
        new Color(1f, 0.85f, 0.1f);

    private Renderer[] renderers;
    private Color[] originalColors;
    public static PartSelector CurrentSelected;

    void Start()
    {
        renderers =
            GetComponentsInChildren<Renderer>();
        originalColors =
            new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            if (renderers[i] != null &&
                renderers[i].material != null)
                originalColors[i] =
                    renderers[i].material.color;
    }

    public void HandleTap()
    {
        if (CurrentSelected != null &&
            CurrentSelected != this)
            CurrentSelected.Deselect();
        Select();
        ShowInfo();
    }

    public void Select()
    {
        CurrentSelected = this;
        foreach (var r in renderers)
            if (r != null && r.material != null)
                r.material.color = highlightColor;
    }

    public void Deselect()
    {
        for (int i = 0;
             i < renderers.Length; i++)
            if (renderers[i] != null &&
                renderers[i].material != null)
                renderers[i].material.color =
                    originalColors[i];
    }

    string GetPartName()
    {
        string mesh = string.IsNullOrEmpty(
            customPartName)
            ? gameObject.name
            : customPartName;

        if (OrganDatabaseManager.Instance != null)
            return OrganDatabaseManager.Instance
                .GetDisplayName(organId, mesh);
        return mesh;
    }

    void ShowInfo()
    {
        string name = GetPartName();
        string fact = "";

        if (OrganDatabaseManager.Instance != null)
            fact = OrganDatabaseManager.Instance
                .GetShortFact(
                    organId, gameObject.name);

        if (uiManager != null)
            uiManager.ShowPanel(
                name, fact, "", true);

        if (geminiClient != null)
            StartCoroutine(
                geminiClient.GetExplanation(
                    name,
                    desc =>
                    {
                        uiManager?
                            .UpdateDescription(desc);
                        chatManager?
                            .SetContext(name, desc);
                    },
                    err =>
                        uiManager?
                        .UpdateDescription(err)));
    }
}