using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public TMP_Text organNameText;
    public TMP_Text descriptionText;

    [Header("Loading Message")]
    private string currentOrgan;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show(string organName, string description)
    {
        currentOrgan = organName;
        organNameText.text = organName;
        descriptionText.text = description;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void ShowLoading(string organName)
    {
        Show(organName, "Loading explanation...");
    }

    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
}