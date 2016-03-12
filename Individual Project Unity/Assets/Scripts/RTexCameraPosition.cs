using UnityEngine;
using System.Collections;

namespace NEGeo {
    [RequireComponent(typeof(Camera))]
    public class RTexCameraPosition : MonoBehaviour {

        public Transform PointOfView;
        public Transform RenderPosition;
        Transform _player;
        Vector3 _defaultRot;
        Vector3 _normalisedDefaultRot;
        Camera cam;

        public static Vector3 rotationOffset = Vector3.zero;

        // Use this for initialization
        void Start () {
            _player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            _defaultRot = transform.parent.rotation.eulerAngles;
            _normalisedDefaultRot = new Vector3(0, _defaultRot.y, 0);
            cam = GetComponent<Camera>();
            cam.fieldOfView = _player.GetComponent<Camera>().fieldOfView;
        }

        // Update is called once per frame
        void Update () {
            Vector3 offset;
            offset = PointOfView.position - _player.position;

            transform.parent.position = RenderPosition.position - offset;
            transform.parent.rotation = Quaternion.Euler(rotationOffset + _defaultRot - _normalisedDefaultRot);
        }
    }
}