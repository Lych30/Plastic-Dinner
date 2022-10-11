using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Command", menuName = "ScriptableObjects/Command")]
public class Command : ScriptableObject
{
    public List<Food> foods = new List<Food>();
}
