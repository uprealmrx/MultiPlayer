using Fusion;
using System.Threading;
using TMPro;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject _lobbyPanel;
    public GameObject _loadingPanel;
    public GameObject _gamePanel;
    public TMP_InputField _roomID;
    public TextMeshProUGUI _roomText;

    private CancellationTokenSource _cancellationTokenSource;
    public async void OnHostClicked()
    {
        _loadingPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _lobbyPanel.SetActive(false);

        _cancellationTokenSource = new CancellationTokenSource();

        var cancellationToken = _cancellationTokenSource.Token;

        bool success = await Manager.Instance._networkManager.StartGame(GameMode.Host, _roomID.text, cancellationToken);

        if (!success)
        {
            // cancel or error → return to lobby
            _loadingPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _lobbyPanel.SetActive(true);
            _roomText.text = "Hosting Session";
            return;
        }

    }
    public async void OnJoinClicked()
    {
        _loadingPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _lobbyPanel.SetActive(false);

        _cancellationTokenSource = new CancellationTokenSource();

        var cancellationToken = _cancellationTokenSource.Token;

        bool success =  await Manager.Instance._networkManager.StartGame(GameMode.Client, _roomID.text, cancellationToken);
        if (!success)
        {
            // cancel or error → return to lobby
            _loadingPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _lobbyPanel.SetActive(true);
            _roomText.text = "Joined Session";
            return;
        }
    }
    public void OnClickCancel()
    {
        _roomID.text = string.Empty;

        _cancellationTokenSource.Cancel();

        _loadingPanel.SetActive(false);
        _gamePanel.SetActive(false);
        _lobbyPanel.SetActive(true);
    }
}
