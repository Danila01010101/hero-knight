using UnityEngine;

[CreateAssetMenu(menuName = "Sword")]
public class Sword : ScriptableObject
{
    public string Name;
    public int Damage;
    public float Cooldown;
}