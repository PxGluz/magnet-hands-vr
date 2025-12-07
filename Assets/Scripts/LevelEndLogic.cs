using UnityEngine;

public class LevelEndLogic : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            GameManager.instance.LevelCompleted();
    }
}
