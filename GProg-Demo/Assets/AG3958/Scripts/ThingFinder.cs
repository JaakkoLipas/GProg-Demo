using UnityEngine;

namespace AG3958
{
    public class ThingFinder : MonoBehaviour
    {
        [SerializeField] private GameObject editorObject; // TODO: connect a GameObject to this script in the Inspector

        private GameObject parentObject;

        private GameObject someOtherObject;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            parentObject = this.transform.parent.gameObject; // this will break if you change anything about the hierarchy
            // parentObject = GetComponentInParent<Transform>().gameObject; -- more useful for other components than Transforms!

            someOtherObject = GameObject.Find(""); // TODO: make this actually find a GameObject by adding its name into the parameter
        }

        // Update is called once per frame
        void Update()
        {
            
            // TODO: do something, anything, with one or more of the found objects, you can use a script you made before
        }
    }

}