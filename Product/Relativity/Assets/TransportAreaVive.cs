using UnityEngine;

public class TransportAreaVive : MonoBehaviour {

    public Camera cameraFrom, cameraTo;
    SteamVR_Camera _toCam;

    public int cameraFromDepth, cameraToDepth = -1;

    void Start () {
        if (cameraTo == null) {
            return;
        }
        _toCam = cameraTo.GetComponent<SteamVR_Camera>();
    }

    void OnCollisionEnter (Collision col) {
        Debug.Log("Welcome");
    }

    void OnCollisionStay (Collision col) {
        Debug.Log(col.collider.name);
        Vector3 centre = transform.position;
        Vector3 distance = cameraFrom.transform.position - centre;
        if (cameraFrom.depth == cameraFromDepth && col.collider.GetComponent<Camera>() != null && Vector3.Dot(distance.normalized, transform.forward) < 0) {
            cameraFrom.depth = cameraFromDepth;
            cameraTo.depth = cameraToDepth;
        }

        //if (perspective != null && col.tag == "Player")
        //{
        //    perspective.positionPlayer(col, transform.position, transform.forward);
        //}
        //if (persp != null && col.tag == "Player")
        //{
        //    persp.positionPlayer(col, transform.position, transform.forward);
        //}
    }

    void OnBecameVisible () {
        if (_toCam == null) {
            return;
        }

        _toCam.enabled = true;
    }

    void OnBecameInvisible () {
        if (_toCam == null) {
            return;
        }

        if (cameraTo.depth == -1) {
            _toCam.enabled = true;
        } else {
            _toCam.enabled = false;
        }
    }
}
