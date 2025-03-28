using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - this.transform.parent.position;
    }

    private void LateUpdate()
    {
        transform.position = this.transform.parent.position + offset;
        transform.rotation = Quaternion.Euler(0f, this.transform.parent.rotation.eulerAngles.y, 0f);
    }
}
