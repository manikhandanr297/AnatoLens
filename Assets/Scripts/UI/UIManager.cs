using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Info Panel")]
    public GameObject panel;
    public TMP_Text organNameText;
    public TMP_Text shortFactText;
    public TMP_Text descriptionText;
    public Button closeButton;
    public Button chatButton;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void ShowPanel(
        string name,
        string fact,
        string desc,
        bool loading = false)
    {
        if (organNameText != null)
            organNameText.text = name;
        if (shortFactText != null)
            shortFactText.text = fact;
        if (descriptionText != null)
            descriptionText.text =
                loading
                ? "Loading explanation..."
                : desc;
        if (panel != null)
            panel.SetActive(true);
    }

    public void UpdateDescription(string desc)
    {
        if (descriptionText != null)
            descriptionText.text = desc;
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}