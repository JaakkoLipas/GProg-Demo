using System.Collections.Generic;
using UnityEngine;
using AG3958;

namespace AG3958
{
    public class DestructibleBlock : MonoBehaviour
    {
        // How much damage it takes for the block to be destroyed
        public float hitPoints = 1.0f;

        // How long the block lives for
        public float lifeTime = 10.0f;

        // Set at instantiation to calculate lifetime
        private float timeOnInstantiate;

        private void Start()
        {
            timeOnInstantiate = Time.time;
        }

        private void Update()
        {
            if (Time.time - timeOnInstantiate >= lifeTime)
            {
                this.gameObject.GetComponentInParent<BlockSpawner>().spawnedItems.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }

        // TODO: actually make these interact with the FPS Microgame damage mechanics
    }

}