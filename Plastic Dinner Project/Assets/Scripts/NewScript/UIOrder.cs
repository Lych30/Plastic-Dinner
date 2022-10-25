using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIOrder : MonoBehaviour
{
    public float orderTime = 10.0f;
    public Sprite defaultSprite;
    public Image circle;
    public List<Image> images = new List<Image>();

    [Header("_______________DEBUG____________")]
    public Command currentOrder;
    public float timer = 0;


    private void Awake()
    {
        foreach (Image image in images)
        {
            image.sprite = defaultSprite;
        }
    }

    private void Update()
    {
        if (timer < 0)
        {
            CommandSystem.DestroyOrder(currentOrder, false);
        }

        timer -= Time.deltaTime;

        // VFX Timer
        float ratio = timer / orderTime;
        Color temp = circle.color;
        // At Start RGB = (0, 255, 0);  // Green
        // At Mid RGB = (255, 255, 0);  // Yellow
        // At End RGB = (255, 0, 0);    // Red
        if (ratio > 0.5f)
        {
            temp.r = Mathf.Lerp(1, 0, ratio - 0.5f);    // Add red to bring it to yellow
        }
        else
        {
            temp.g = Mathf.Lerp(0, 1, ratio * 2.0f);    // Reduces green to bring to red
        }
        circle.color = temp;
        circle.fillAmount = ratio;
        // -------
    }

    private void OnEnable()
    {
        timer = orderTime;
    }

    public void Display(Command order)
    {
        currentOrder = order;
        for (int i = 0; i < order.foods.Count; i++)
        {
            images[i].sprite = order.foods[i].sprite;
        }
    }
}
