using System.Collections;
using UnityEngine;

namespace NEGeo {
    [RequireComponent(typeof(Camera))]
    public class CameraRenderPosition : MonoBehaviour {

        // Whether or not the render this portals view
        public bool Render = true;

        // Whether to render the portal as a continuation or a shortcut
        public bool Inverse = false;

        public Transform PointOfView; // Conntected To
        public Transform RenderPosition; // This
        Transform _player;
        Camera _playerCam;
        Vector3 _defaultRot;
        Vector3 _normalisedDefaultRot;
        Camera _cam;

        public Transform[] Bounds;
        Vector3[] _bounds;
        CameraRenderPosition _linkedScript;
        bool _interruptDisable = false;

        GameObject[] _additionalDepthRenderers;

        Quaternion _relativePlayerRot;
        Quaternion _relativePortalRot;

        public static Vector3 rotationOffset = Vector3.zero;

        OVRPlayerController _playerControl;

        // Use this for initialization
        void Start () {
            _player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            _playerCam = _player.GetComponent<Camera>();
            _defaultRot = transform.parent.rotation.eulerAngles;
            _normalisedDefaultRot = new Vector3(0, _defaultRot.y, 0);
            _cam = GetComponent<Camera>();
            _cam.fieldOfView = _player.GetComponent<Camera>().fieldOfView;

            _bounds = new Vector3[Bounds.Length];
            for (int i = 0; i < Bounds.Length; i++) {
                _bounds[i] = Bounds[i].position;
            }

            _additionalDepthRenderers = new GameObject[Helper.renderDepth];
            for (int i = 0; i < Helper.renderDepth; i++) {
                _additionalDepthRenderers[i] = Instantiate(transform.parent.gameObject);
                _additionalDepthRenderers[i].GetComponentInChildren<CameraRenderPosition>().enabled = false;
                _additionalDepthRenderers[i].GetComponentInChildren<Camera>().depth = -(i + 2);
                _additionalDepthRenderers[i].transform.parent = transform.parent.parent;
            }

            _relativePlayerRot = Quaternion.FromToRotation(RenderPosition.forward, -PointOfView.forward);
            _relativePortalRot = Quaternion.FromToRotation(RenderPosition.forward, PointOfView.forward);

            _linkedScript = PointOfView.GetComponentInChildren<CameraRenderPosition>();

            _playerControl = _player.GetComponentInParent<OVRPlayerController>();
        }

        // Update is called once per frame
        void LateUpdate () {
            if (Render && (PointOfView.GetComponentInChildren<CameraRenderPosition>().inScreenView() || (!PointOfView.GetComponentInChildren<CameraRenderPosition>().inScreenView() && _cam.enabled))) {
                if (PointOfView.GetComponentInChildren<CameraRenderPosition>().inScreenView()) {
                    _cam.enabled = true;
                    _interruptDisable = true;
                }

                Vector3 offset = PointOfView.position - _player.position;

                // Position and rotate the cameras depending on the type of illusion they are going for
                if (!Inverse) {
                    transform.parent.position = Helper.RotatePointAroundPivot(RenderPosition.position - offset, RenderPosition.position, _relativePortalRot.eulerAngles);
                    transform.parent.rotation = _relativePortalRot * Quaternion.Euler(rotationOffset + _defaultRot - _normalisedDefaultRot);
                } else {
                    transform.parent.position = RenderPosition.position - offset;
                    transform.parent.rotation = _relativePortalRot * Quaternion.Euler((RenderPosition.transform.up == Vector3.up ? rotationOffset : -rotationOffset) + _defaultRot - _normalisedDefaultRot + new Vector3(0, 180f, 0));
                }

                // Set the near clipping plane of the camera to only render starting from the closest visible area
                //_cam.nearClipPlane = Mathf.Max((Helper.FindClosestPoint(PointOfView.GetComponentInChildren<CameraRenderPosition>().GetBounds(), _playerCam.transform.position) - _playerCam.transform.position).magnitude - 0.3f, 0.1f);

                // Set the near clipping plane of the camera to only render starting from the closest visible area
                _cam.nearClipPlane = (Helper.FindClosestPoint(_bounds, transform.position) - transform.position).magnitude / 2f;

                // If there are additional depth cameras to render, enable and position them
                for (int i = 0; i < Helper.renderDepth; i++) {
                    _additionalDepthRenderers[i].GetComponentInChildren<Camera>().enabled = true;
                    _additionalDepthRenderers[i].transform.position = Helper.RotatePointAroundPivot(RenderPosition.position - offset + ((transform.parent.position - _player.position) * (i + 1)), RenderPosition.position, _relativePortalRot.eulerAngles);
                    _additionalDepthRenderers[i].transform.rotation = _relativePortalRot * Quaternion.Euler(rotationOffset + _defaultRot - _normalisedDefaultRot);
                }
            } else {
                _interruptDisable = false;
                if (_cam.enabled) {
                    StartCoroutine(cameraOff());

                    foreach (GameObject camera in _additionalDepthRenderers) {
                        StartCoroutine(cameraOff(camera.GetComponentInChildren<Camera>()));
                    }
                }
            }

            // Set the depth of the cameras to be relavent to their position to the player. This ensures the closest connected point always renders first
            _linkedScript.SetDepth(-((RenderPosition.position - _player.position).magnitude) / 10f);

            // Draw a line to show the linked portals
            Debug.DrawLine(PointOfView.position, RenderPosition.position, Color.red);
            // Draw a line to point in the direction of the portal
            Debug.DrawRay(RenderPosition.position, RenderPosition.forward, Color.green);
        }

        /// <summary>
        /// Re-Position the player from their current position to the equivelant position of it's point of view, depending on their relative position
        /// </summary>
        /// <param name="player">Player to potentially transport</param>
        /// <param name="centre">Centre of the currently colliding object</param>
        /// <param name="forward">Forward vector of the currently colliding object</param>
        public void positionPlayer (Collider player, Vector3 centre, Vector3 forward) {
            Vector3 distance = player.transform.position - centre;
            Quaternion inverseFlip = Quaternion.Euler(0, 0, 0);

            if (Inverse != _linkedScript.Inverse) {
                inverseFlip = Quaternion.Euler(0, 180f, 0);
            }

            // If the player is more than half way through the object, transport them to the linked area
            if (Vector3.Dot(distance.normalized, forward) < 0) {
                distance = Helper.RotatePointAroundPivot(distance, Vector3.zero, (_relativePlayerRot.eulerAngles + inverseFlip.eulerAngles));
                player.transform.position = PointOfView.position;
                player.transform.position += distance;

                rotationOffset += _relativePlayerRot.eulerAngles + inverseFlip.eulerAngles;
                player.transform.rotation = _relativePlayerRot * inverseFlip * player.transform.rotation;

                _playerControl.UpdateMoveThrottle(_relativePlayerRot * inverseFlip * _playerControl.GetMoveThrottle());
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

                    // Only render it the camera can directly see the render area
                    RaycastHit hit;
                    if (Physics.Raycast(camera.transform.position, (point - camera.transform.position).normalized, out hit)) {
                        if (hit.collider.GetComponentInChildren<CameraRenderPosition>() != null) {
                            Debug.DrawLine(camera.transform.position, hit.collider.transform.position, Color.yellow);
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
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
        /// <returns>Whether the object is within view of the player</returns>
        public bool inScreenView () {
            return inScreenView(_bounds, _playerCam);
        }

        /// <summary>
        /// Returns the bounds of the portal
        /// </summary>
        /// <returns>The bounds of the portal</returns>
        public Vector3[] GetBounds () {
            return _bounds;
        }

        /// <summary>
        /// Set the render depth for the camera
        /// </summary>
        /// <param name="depth">Depth to use for the camera</param>
        public void SetDepth (float depth) {
            // The depth should always be <= 0, and more than -100
            _cam.depth = Mathf.Max(Mathf.Min(depth, 0f), -100f);
        }

        /// <summary>
        /// Disables the camera after a set interval
        /// </summary>
        /// <returns></returns>
        IEnumerator cameraOff () {
            yield return new WaitForSeconds(0.1f);
            if (!_interruptDisable) {
                _cam.enabled = false;
            }
        }

        /// <summary>
        /// Disabled the supplied camera after a set interval
        /// </summary>
        /// <param name="cam">Camera to disable</param>
        /// <returns></returns>
        IEnumerator cameraOff (Camera cam) {
            yield return new WaitForSeconds(0.1f);
            if (!_interruptDisable) {
                cam.enabled = false;
            }
        }
    }
}