using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public Image panelBackground;

    [Header("Text")]
    public TMP_Text organNameText;
    public TMP_Text shortFactText;
    public TMP_Text descriptionText;

    [Header("Buttons")]
    public Button closeButton;
    public Button listenButton;

    [Header("Loading")]
    public GameObject loadingSpinner;

    private void Start()
    {
        StylePanel();
        panel.SetActive(false);
    }

    void StylePanel()
    {
        // Panel background - dark semi transparent
        if (panelBackground != null)
            panelBackground.color =
                new Color(0.05f, 0.05f, 0.15f, 0.92f);

        // Organ name - large white bold
        if (organNameText != null)
        {
            organNameText.color = Color.white;
            organNameText.fontSize = 26;
            organNameText.fontStyle =
                FontStyles.Bold;
        }

        // Short fact - yellow subtitle
        if (shortFactText != null)
        {
            shortFactText.color =
                new Color(1f, 0.85f, 0.2f);
            shortFactText.fontSize = 15;
            shortFactText.fontStyle =
                FontStyles.Italic;
        }

        // Description - light grey readable
        if (descriptionText != null)
        {
            descriptionText.color =
                new Color(0.85f, 0.85f, 0.85f);
            descriptionText.fontSize = 14;
        }
    }

    public void ShowPanel(
        string organName,
        string shortFact,
        string description,
        bool isLoading = false)
    {
        organNameText.text = organName;
        shortFactText.text = shortFact;
        descriptionText.text = isLoading ?
            "Loading explanation..." : description;

        if (loadingSpinner != null)
            loadingSpinner.SetActive(isLoading);

        panel.SetActive(true);
    }

    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
        if (loadingSpinner != null)
            loadingSpinner.SetActive(false);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }
}