using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public int maxNPC = 8;
    public float spawnDelay = 5.0f;
    public Transform spawnPoint = null;
    public Transform exitPoint = null;

    [Header("____________DEBUG___________")]
    public List<GameObject> npcPool = new List<GameObject>();
    public List<GameObject> destinations = new List<GameObject>();
    public float timer = 0.0f;
    public static NPCManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        npcPool.Clear();

        for (int i = 0; i < maxNPC; i++)
        {
            GameObject spawnedNPC = Instantiate(npcPrefab, transform);
            spawnedNPC.name += " " + i.ToString();
            spawnedNPC.SetActive(false);
            npcPool.Add(spawnedNPC);
        }

        timer = spawnDelay;
    }

    private void Update()
    {
        if (timer <= 0.0f)
        {
            timer = spawnDelay;
            SpawnNPC_Impl();
        }
        timer -= Time.deltaTime;
    }

    private void SpawnNPC_Impl()
    {
        foreach (GameObject npc in npcPool)
        {
            if (!npc.activeInHierarchy)
            {
                npc.SetActive(true);
                break;
            }
        }
    }

    private void SubscribeDestination_Impl(GameObject destination)
    {
        destinations.Add(destination);
    }

    public static void SubscribeDestination(GameObject destination)
    {
        Instance.SubscribeDestination_Impl(destination);
    }

    public static Vector3 SetDestination()
    {
        return Instance.SetDestination_Impl();
    }

    public Vector3 SetDestination_Impl()
    {
        if (destinations.Count <= 0)
        {
            return Vector3.zero;
        }

        int rng = Random.Range(0, destinations.Count);
        return destinations[rng].transform.position; ;
    }

    public static void DestroyNPC(Command order, bool isSuccess = true)
    {
        Instance.DestroyNPC_Impl(order, isSuccess);
    }


    private void DestroyNPC_Impl(Command order, bool isSuccess = true)
    {
        foreach (GameObject npc in npcPool)
        {
            if (npc.activeInHierarchy)
            {
                NPCController npcController = npc.GetComponent<NPCController>();
                if (npcController.currentOrder == order)
                {
                    npcController.Destroy(isSuccess);
                    break;
                }
            }
        }
    }

    public static Vector3 SpawnPoint()
    {
        return Instance.spawnPoint.position;
    }

    public static Vector3 ExitPoint()
    {
        return Instance.exitPoint.position;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }

    public Vector3 GetExitPoint()
    {
        return exitPoint.position;
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NPCManager : MonoBehaviour
//{
//    public OrdersUI ordersUI;
//    [Header("NPC")]
//    public List<NPCController> NPC_List;
//    public GameObject npcGameObject;
//    public Transform spawnTransform;
//    public Transform Exit;
//    public int MaxNPCs;

//    private GameObject[] Tables;

//    public static NPCManager Instance { get; private set; }
//    private void Awake()
//    {
//        // If there is an instance, and it's not me, delete myself.

//        if (Instance != null && Instance != this)
//        {
//            Destroy(this);
//        }
//        else
//        {
//            Instance = this;
//        }
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        Tables = GameObject.FindGameObjectsWithTag("Table");  
//    }

//    // OrdersUI -> SpawnNPC -> NPC Controller arrives at destinations -> GetOrder is called -> OrderUI CreateOrder -> Pass to CommandSystem who add it
//    public void SpawnNPC()
//    {
//        if (NPC_List.Count < MaxNPCs)
//        {
//            GameObject SpawnedNPC = Instantiate(npcGameObject, spawnTransform.position, Quaternion.identity);
//            NPCcontroler SpawnedNPCcontroler = SpawnedNPC.GetComponent<NPCcontroler>();
//            SpawnedNPCcontroler.Destination = SetDestination();
//            NPC_List.Add(SpawnedNPCcontroler);
//        }

//    }

//    public void TakeOrder()
//    {
//        ordersUI.CreateOrder();
//    }

//    Vector3 SetDestination()
//    {
//        Vector3 Destination = new Vector3(0,0,0);
//        int randomTable = Random.Range(0, Tables.Length);
//        Destination = Tables[randomTable].transform.position;
//        return Destination;
//    }
//}
