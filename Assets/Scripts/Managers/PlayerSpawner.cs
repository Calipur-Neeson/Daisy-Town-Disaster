using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private PlayerController[] playerPrefabs;

    private Dictionary<ulong, PlayerController> players = new Dictionary<ulong, PlayerController>();

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;
    }

   [ServerRpc(RequireOwnership = false)]
   public void SetPlayerPrefab_ServerRpc(int characterId, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (characterId < 0 || characterId >= playerPrefabs.Length)
        {
            Debug.LogWarning($"Invalid role ID {characterId}");
            return;
        }
        
        Vector3 pos = GetSpawnPosition(characterId);
        PlayerController player = Instantiate(playerPrefabs[characterId], pos, Quaternion.identity);
        //player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, false);
        var netObj = player.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(clientId);

    }

    public PlayerController GetPlayerPrefab(int i) => playerPrefabs[i];
    
    private Vector3 GetSpawnPosition(int i)
    {
        return new Vector3(i * 2, 2, 0);
    }
}
