using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public OrdersUI ordersUI;
    [Header("NPC")]
    public List<NPCcontroler> NPC_List;
    public GameObject npcGameObject;
    public Transform spawnTransform;
    public Transform Exit;
    public int MaxNPCs;
    public Material[] materials;
    private GameObject[] Tables;
    int previousTable = 10;
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
    }

    // OrdersUI -> SpawnNPC -> NPC Controller arrives at destinations -> GetOrder is called -> OrderUI CreateOrder -> Pass to CommandSystem who add it
    public void SpawnNPC()
    {
        if (NPC_List.Count < MaxNPCs)
        {
            GameObject SpawnedNPC = Instantiate(npcGameObject, spawnTransform.position, Quaternion.identity);
            NPCcontroler SpawnedNPCcontroler = SpawnedNPC.GetComponent<NPCcontroler>();
            SpawnedNPCcontroler.Destination = SetDestination();
            SpawnedNPC.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = materials[Random.Range(0,materials.Length)];
            SpawnedNPC.transform.GetChild(0).GetChild(3).GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
            SpawnedNPC.transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
            NPC_List.Add(SpawnedNPCcontroler);
        }

    }

    public void TakeOrder()
    {
        ordersUI.CreateOrder();
    }

    Vector3 SetDestination()
    {
        Vector3 Destination = new Vector3(0,0,0);
        int randomTable = Random.Range(0, Tables.Length);
        while (randomTable == previousTable)
        {
           randomTable = Random.Range(0, Tables.Length);
        }
        previousTable = randomTable;
        Destination = Tables[randomTable].transform.position;
        return Destination;
    }
}
