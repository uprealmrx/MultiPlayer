using Fusion;
using TMPro;
using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    public PlayerRef playerRef; // the player this button represents
    public int currentIndex;
    [SerializeField] private int minIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private TextMeshProUGUI prefIndexText;
    [SerializeField] private GameObject SelectionButtons;

    public void Init(PlayerRef refPlayer, int startingIndex = 0)
    {
        playerRef = refPlayer;
        currentIndex = startingIndex;
        UpdateText();
    }

    public void Next()
    {
        if (currentIndex < maxIndex) currentIndex++;
        UpdateText();
    }

    public void Previous()
    {
        if (currentIndex > minIndex) currentIndex--;
        UpdateText();
    }

    public void OnButtons()
    {
        SelectionButtons.SetActive(true);
    }

    public void Lock()
    {
        var runner = FindAnyObjectByType<NetworkRunner>();

        // ONLY HOST can assign selections
        if (!runner.IsServer)
            return;

        LocalManager.Instance.SetSelection(playerRef, currentIndex);
    }

    private void UpdateText()
    {
        prefIndexText.text = currentIndex.ToString();
    }
}
