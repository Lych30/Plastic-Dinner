using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "ScriptableObjects/Food")]
public class Food : ScriptableObject
{
    public Sprite sprite;
    public List<FoodCategory> foodCategory = new List<FoodCategory>();
}
