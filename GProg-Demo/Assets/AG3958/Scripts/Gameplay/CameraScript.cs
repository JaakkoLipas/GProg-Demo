using UnityEngine;

namespace AG3958
{
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
            transform.rotation = Quaternion.Euler(30f, 0f, 0f);
        }
    } 
}