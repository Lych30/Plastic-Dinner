using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodList", menuName = "ScriptableObjects/FoodList")]
public class FoodList : ScriptableObject
{
    public List<Food> foods = new List<Food>();
}
