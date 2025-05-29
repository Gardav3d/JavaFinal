using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!");
            GameManager.Instance.ResetScore(); // We'll add this method next
        }
    }
}
