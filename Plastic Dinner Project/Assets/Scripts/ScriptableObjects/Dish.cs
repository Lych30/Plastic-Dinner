using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dish", menuName = "ScriptableObjects/Dish")]
public class Dish : ScriptableObject
{
    public List<Food> foods = new List<Food>();
}
