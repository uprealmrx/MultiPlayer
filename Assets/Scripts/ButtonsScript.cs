using Fusion;
using TMPro;
using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    public PlayerRef player;
    public int _myIndex;
    public int currentIndex;

    [SerializeField] private int minIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private TextMeshProUGUI prefIndexText;

    public void Init(PlayerRef owner)
    {
        player = owner;
        UpdateText();
    }

    public void Next()
    {
        if (currentIndex < maxIndex)
            currentIndex++;

        UpdateText();
    }

    public void Previous()
    {
        if (currentIndex > minIndex)
            currentIndex--;

        UpdateText();
    }

    public void Lock()
    {
        Manager.Instance.SetPrefabIndex(player, currentIndex,_myIndex);
    }

    private void UpdateText()
    {
        prefIndexText.text = currentIndex.ToString();
    }
}
