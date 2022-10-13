using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
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
        // TO DO : Add a script to table -> and at awake make them subscribe to npcmanager
        // in subscribe function in npcmanager -> add the table in table list inside npc manager
        Tables = GameObject.FindGameObjectsWithTag("Table");

        // TO DO (link to NPC Controller): 
        //  Spawn NPC after OrderUI Has created a new order
        // OR
        //  Add a timer before spawn an npc
        //      when timer is finished
        //          spawn an npc
        //              when npc has finished spawning and is at destination
        //                  TakeOrder
        //                      -> in CommandSystem -> call AddCommandToDo
        // I think the second solution is better because more logical
        
            

        
    }

    public void SpawnNPC()
    {
        if (NPC_List.Count < MaxNPCs)
        {
            GameObject SpawnedNPC = Instantiate(npcGameObject, spawnTransform.position, Quaternion.identity);
            NPCcontroler SpawnedNPCcontroler = SpawnedNPC.GetComponent<NPCcontroler>();
            SpawnedNPCcontroler.Destination = SetDestination();
            NPC_List.Add(SpawnedNPCcontroler);
        }

    }

    Vector3 SetDestination()
    {
        Vector3 Destination = new Vector3(0,0,0);
        int randomTable = Random.Range(0, Tables.Length);
        Destination = Tables[randomTable].transform.position;
        return Destination;
    }
}
