using UnityEngine;
using System.Collections;

namespace NEGEo {
    public class FalseWall : MonoBehaviour {

        Camera _cam;
        Bounds _bounds;
        Collider _collider;

        // Use this for initialization
        void Start () {
            _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _collider = GetComponent<Collider>();
            _bounds = _collider.bounds;
        }

        // Update is called once per frame
        void Update () {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_cam);
            // If the object is visible by the camera, make sure the collider is enabled
            if (GeometryUtility.TestPlanesAABB(planes, _bounds)) {
                _collider.enabled = true;
            } else {
                _collider.enabled = false;
            }
        }
    }
}