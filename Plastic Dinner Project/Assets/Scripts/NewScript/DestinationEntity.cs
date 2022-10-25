using UnityEngine;
public class DestinationEntity : MonoBehaviour
{
    private void Start()
    {
        NPCManager.SubscribeDestination(gameObject);
    }
}
