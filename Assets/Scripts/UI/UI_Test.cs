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
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;

    private bool isHost;
    private void Start()
    {
        canvas1.enabled = true;
        canvas2.enabled = false;

        hostButton?.onClick.AddListener(OnHostButtonClick);
        clientButton?.onClick.AddListener(OnClientButtonClick);
        player1Button?.onClick.AddListener(OnPlayer1ButtonClick);
        player2Button?.onClick.AddListener(OnPlayer2ButtonClick);
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
        SetCharacterAndStart(player1Prefab);
        Debug.Log("revolver");
    }

    private void OnPlayer2ButtonClick()
    {
        SetCharacterAndStart(player2Prefab);
        Debug.Log("rifle");
    }

    private void SetCharacterAndStart(GameObject character)
    {
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = character;

        if (isHost)
            NetworkManager.Singleton.StartHost();
        else
            NetworkManager.Singleton.StartClient();

        canvas2.enabled= false;
    }
}
