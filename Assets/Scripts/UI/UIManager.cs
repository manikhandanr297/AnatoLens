using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Info Panel")]
    public GameObject panel;
    public Image panelBackground;
    public TMP_Text organNameText;
    public TMP_Text shortFactText;
    public TMP_Text descriptionText;
    public Button closeButton;
    public GameObject loadingIndicator;

    void Start()
    {
        StylePanel();
        if (panel != null)
            panel.SetActive(false);
        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
    }

    void StylePanel()
    {
        if (panelBackground != null)
            panelBackground.color =
                new Color(0.05f, 0.05f, 0.15f, 0.93f);
        if (organNameText != null)
        {
            organNameText.color = Color.white;
            organNameText.fontSize = 24;
            organNameText.fontStyle =
                FontStyles.Bold;
        }
        if (shortFactText != null)
        {
            shortFactText.color =
                new Color(1f, 0.85f, 0.2f, 1f);
            shortFactText.fontSize = 14;
            shortFactText.fontStyle =
                FontStyles.Italic;
        }
        if (descriptionText != null)
        {
            descriptionText.color =
                new Color(0.85f, 0.85f, 0.85f, 1f);
            descriptionText.fontSize = 13;
        }
    }

    public void ShowPanel(
        string organName,
        string shortFact,
        string description,
        bool isLoading = false)
    {
        if (organNameText != null)
            organNameText.text = organName;
        if (shortFactText != null)
            shortFactText.text = shortFact;
        if (descriptionText != null)
            descriptionText.text = isLoading
                ? "Loading explanation..."
                : description;
        if (loadingIndicator != null)
            loadingIndicator.SetActive(isLoading);
        if (panel != null)
            panel.SetActive(true);
    }

    public void UpdateDescription(
        string description)
    {
        if (descriptionText != null)
            descriptionText.text = description;
        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}