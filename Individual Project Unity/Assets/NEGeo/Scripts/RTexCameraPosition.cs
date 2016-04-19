using System.Collections;
using UnityEngine;

namespace NEGeo {
    [RequireComponent(typeof(Camera))]
    public class RTexCameraPosition : MonoBehaviour {

        public GameObject RenderPlane;

        public Transform PointOfView;
        public Transform RenderPosition;
        Transform _player;
        Camera _playerCam;
        Vector3 _defaultRot;
        Vector3 _normalisedDefaultRot;
        Camera cam;

        public Transform[] Bounds;
        Vector3[] _bounds;
        bool interruptDisable = false;

        GameObject[] additionalDepthRenderers;

        Quaternion _relativePlayerRot;
        Quaternion _relativePortalRot;

        RenderTexture _rtex;

        public static Vector3 rotationOffset = Vector3.zero;

        // Use this for initialization
        void Start () {
            _player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            _playerCam = _player.GetComponent<Camera>();
            _defaultRot = transform.parent.rotation.eulerAngles;
            _normalisedDefaultRot = new Vector3(0, _defaultRot.y, 0);
            cam = GetComponent<Camera>();
            cam.fieldOfView = _player.GetComponent<Camera>().fieldOfView;

            _bounds = new Vector3[Bounds.Length];
            for (int i = 0; i < Bounds.Length; i++) {
                _bounds[i] = Bounds[i].position;
            }

            _relativePlayerRot = Quaternion.FromToRotation(RenderPosition.forward, -PointOfView.forward);
            _relativePortalRot = Quaternion.FromToRotation(-PointOfView.forward, RenderPosition.forward);

            cam.aspect = RenderPlane.transform.localScale.x / RenderPlane.transform.localScale.y;

            _rtex = new RenderTexture(1024, 1024, 24);

            RenderPlane.GetComponent<MeshRenderer>().material.mainTexture = _rtex;

            cam.targetTexture = _rtex;
        }

        // Update is called once per frame
        void Update () {
            if (PointOfView.GetComponentInChildren<RTexCameraPosition>().inScreenView()) {
                cam.enabled = true;
                interruptDisable = true;

                Vector3 offset;
                offset = PointOfView.position - _player.position;

                transform.parent.position = RenderPosition.position - offset;
                transform.parent.rotation = Quaternion.Euler(rotationOffset + _defaultRot - _normalisedDefaultRot + new Vector3(0, 180, 0));

            } else {
                interruptDisable = false;
                if (cam.enabled) {
                    StartCoroutine(cameraOff());
                }
            }

        }

        /// <summary>
        /// Re-Position the player from their current position to the equivelant position of it's point of view, depending on their relative position
        /// </summary>
        /// <param name="player">Player to potentially transport</param>
        /// <param name="centre">Centre of the currently colliding object</param>
        /// <param name="forward">Forward vector of the currently colliding object</param>
        public void positionPlayer (Collider player, Vector3 centre, Vector3 forward) {
            Vector3 distance = player.transform.position - centre;

            // If the player is more than half way through the object, transport them to the linked area
            if (Vector3.Dot(distance.normalized, forward) < 0) {
                player.transform.rotation = _relativePlayerRot * player.transform.rotation;

                //distance = Helper.RotatePointAroundPivot(distance, Vector3.zero, new Vector3(0, 0, 180f));
                player.transform.position = PointOfView.position;
                player.transform.position += _relativePlayerRot * distance;
            }
        }

        /// <summary>
        /// Calculates whether a point is in view of the screen
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <param camera="camera">Camera to check if the point is rendered by</param>
        /// <returns>Whether or not the point is in view of the camera</returns>
        private bool inScreenView (Vector3 point, Camera camera) {
            Vector3 relativePos = camera.WorldToScreenPoint(point);

            // If the relative position is negative in Z, then it is behind the player
            if (relativePos.z > -0.5f) {
                // Otherwise, if it is within the screen bounds, then it is visible
                if (relativePos.x > 0f && relativePos.x < Screen.width &&
                    relativePos.y > 0f && relativePos.y < Screen.height) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        /// <summary>
        /// Calculates whether a point is in view of the screen
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <returns>Whether or not the point is in view of the camera</returns>
        private bool inScreenView (Vector3 point) {
            return inScreenView(point, _playerCam);
        }

        /// <summary>
        /// Calculates whether any of the points in an array of vectors3s is in view of the camera
        /// </summary>
        /// <param name="points">Array of points to check</param>
        /// <returns>Whether any of the points are in view of the camera</returns>
        private bool inScreenView (Vector3[] points, Camera camera) {
            bool inView = false;
            foreach (Vector3 point in points) {
                if (inScreenView(point, camera)) {
                    return true;
                }
            }
            return inView;
        }

        /// <summary>
        /// Check whether the bounds of this object are in view of the camera
        /// </summary>
        /// <returns></returns>
        public bool inScreenView () {
            return true;
            //return inScreenView(_bounds, _playerCam);
        }

        IEnumerator cameraOff () {
            yield return new WaitForSeconds(0.1f);
            if (!interruptDisable) {
                cam.enabled = false;
            }
        }

        IEnumerator cameraOff (Camera _cam) {
            yield return new WaitForSeconds(0.1f);
            if (!interruptDisable) {
                _cam.enabled = false;
            }
        }
    }
}