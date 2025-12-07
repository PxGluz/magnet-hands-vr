using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            GameManager.instance.LevelFailed();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Magnetic"))
        {
            MagneticObject magObj = collision.gameObject.GetComponent<MagneticObject>();
            if (magObj != null && magObj.isPullable)
                magObj.RespawnItem();
        }
    }
}
