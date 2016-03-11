using UnityEngine;
using System.Collections;

namespace negeo {
    [RequireComponent(typeof(Camera))]
    public class RTexCameraPosition : MonoBehaviour {

        public Transform PointOfView;
        public Transform RenderPosition;
        Transform _playerPos;
        Camera cam;

        // Use this for initialization
        void Start () {
            _playerPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            cam = GetComponent<Camera>();
            //cam.aspect = transform.parent.lossyScale.x / transform.parent.lossyScale.y;
            cam.fieldOfView = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView;
        }

        // Update is called once per frame
        void Update () {
            Vector3 offset;
            offset = PointOfView.position - _playerPos.position;

            transform.parent.position = RenderPosition.position - offset;
            //transform.parent.rotation = _playerPos.rotation;


            //transform.parent.LookAt(RenderPosition);
            //transform.position = RenderPosition.position;
            // TODO: Calculate equivelant FOV of the render area relative to the players position, and set this to equal that
        }
    }
}