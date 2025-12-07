using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            GameManager.instance.LevelFailed();
    }
}
