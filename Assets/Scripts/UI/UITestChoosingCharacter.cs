using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UITestChoosingCharacter : MonoBehaviour
{
    public PlayerSpawner playerSpawner;

    private int selectedCharacterId = 0;

    [Header("Buttons")]
    public Button hostButton;
    public Button clientButton;
    public Button player01Button;
    public Button player02Button;
    
    void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            RequestSpawn();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            RequestSpawn();
        });

        player01Button.onClick.AddListener(() => SelectCharacter(0));
        player02Button.onClick.AddListener(() => SelectCharacter(1));
    }
    
    void SelectCharacter(int id)
    {
        selectedCharacterId = id;
    }

    void RequestSpawn()
    {
        Invoke(nameof(SendSpawnRequest), 1f);
    }

    void SendSpawnRequest()
    {
        if (playerSpawner != null)
        {
            playerSpawner.SetPlayerPrefab_ServerRpc(selectedCharacterId);
        }
        else
        {
            Debug.LogError("Fuck!!!!!");
        }
        
        gameObject.SetActive(false);
    }
}
