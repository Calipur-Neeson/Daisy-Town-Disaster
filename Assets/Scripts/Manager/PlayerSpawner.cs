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
    
    public void SpawnPlayer(PlayerController player)
    {
        if (!IsServer) return;
        
        ulong playerId = networkManager.LocalClientId;
        players[playerId] = player;

        SetPlayerPrefab_ServerRpc(playerId);
    }

   [ServerRpc(RequireOwnership = false)]
   public void SetPlayerPrefab_ServerRpc(ulong id)
    {
        PlayerController yourPlayer = Instantiate(players[id]);
        yourPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(id);
    }

    public PlayerController GetPlayerPrefab(int i) => playerPrefabs[i];
}
