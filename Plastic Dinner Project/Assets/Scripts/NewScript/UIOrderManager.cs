using System.Collections.Generic;
using UnityEngine;

public class UIOrderManager : MonoBehaviour
{
    public GameObject uIOrderPrefab;

    [Header("______________DEBUG_____________")]
    public List<UIOrder> uIOrderList = new List<UIOrder>();
    public int orderCount = 0;
    private void Awake()
    {
        uIOrderList.Clear();
    }

    public void DisplayOrder(Command order)
    {
        GameObject spawnedUIOrder = Instantiate(uIOrderPrefab, transform);
        spawnedUIOrder.name += " " + orderCount++;
        UIOrder uIOrder = spawnedUIOrder.GetComponent<UIOrder>();
        uIOrder.Display(order);
        uIOrderList.Add(uIOrder);
    }

    public void DestroyOrder(Command order)
    {
        foreach (UIOrder uIOrder in uIOrderList)
        {
            if (uIOrder.currentOrder == order)
            {
                uIOrderList.Remove(uIOrder);
                Destroy(uIOrder.gameObject);
                break;
            }
        }
    }
}
