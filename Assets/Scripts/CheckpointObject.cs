using UnityEngine;

public class CheckpointObject : MonoBehaviour
{
    [SerializeField] private Transform canvasTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetComponent<Collider>().enabled = false;
            GameManager.instance.SetPlayerCheckpoint(other.transform.position);
            canvasTransform.gameObject.SetActive(false);
        }
    }

    private Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }

    private void RotateCanvas()
    {
        Vector3 direction = target.position - transform.position;
        canvasTransform.forward = -direction.normalized;
    }

    private void Update()
    {
        RotateCanvas();
    }
}
