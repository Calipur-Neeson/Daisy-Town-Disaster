using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public int health;
    public float moveSpeed;
    public WeaponSO weapon;
    public AbilitySO ability;
}
