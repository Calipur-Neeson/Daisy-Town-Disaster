using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Player got demage, current health is: {health}");

        if (health <= 0)
        {
            Debug.Log(gameObject.name + " has died.");
            Destroy(gameObject);
        }
    }
}
