using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public CommandSystem commandSystem;

    [Header("NPC")]
    public List<NPCcontroler> NPC_List;
    public GameObject npcGameObject;
    public Transform spawnTransform;
    public Transform Exit;
    public int MaxNPCs;

    private GameObject[] Tables;

    public static NPCManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Tables = GameObject.FindGameObjectsWithTag("Table");

        if (NPC_List.Count < MaxNPCs)
            for (int i = NPC_List.Count; i < MaxNPCs; i++)
            {
                SpawnNPC();
            }
        
    }

    void SpawnNPC()
    {
        GameObject SpawnedNPC = Instantiate(npcGameObject, spawnTransform.position, Quaternion.identity);
        NPCcontroler SpawnedNPCcontroler = SpawnedNPC.GetComponent<NPCcontroler>();
        SpawnedNPCcontroler.Destination = SetDestination();
        NPC_List.Add(SpawnedNPCcontroler);
    }

    Vector3 SetDestination()
    {
        Vector3 Destination = new Vector3(0,0,0);
        int randomTable = Random.Range(0, Tables.Length);
        Destination = Tables[randomTable].transform.position;
        return Destination;
    }
}
