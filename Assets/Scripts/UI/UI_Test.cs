using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_Test : MonoBehaviour
{
    [Header("Host and Client")]
    [SerializeField] private Canvas canvas1;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] private Canvas canvas2;
    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;

    private bool isHost;
    private void Start()
    {
        canvas1.enabled = true;
        canvas2.enabled = false;

        hostButton?.onClick.AddListener(OnHostButtonClick);
        clientButton?.onClick.AddListener(OnClientButtonClick);
        player1Button?.onClick.AddListener(OnPlayer1ButtonClick);
        player2Button?.onClick.AddListener(OnPlayer2ButtonClick);

        //var prefabs = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs;
    }

    private void  OnHostButtonClick()
    {
        isHost = true;
        canvas1.enabled = false;
        canvas2.enabled = true;
    }

    private void OnClientButtonClick()
    {
        isHost = false;
        canvas1.enabled = false;
        canvas2.enabled = true;
    }

    private void OnPlayer1ButtonClick()
    {
        SetCharacterAndStart(0);
    }

    private void OnPlayer2ButtonClick()
    {
        SetCharacterAndStart(1);
    }

    private void SetCharacterAndStart(int prefabIndex)
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = new byte[] { (byte)prefabIndex };

        if (isHost)
            NetworkManager.Singleton.StartHost();
        else
            NetworkManager.Singleton.StartClient();

        canvas2.enabled= false;
    }
}
