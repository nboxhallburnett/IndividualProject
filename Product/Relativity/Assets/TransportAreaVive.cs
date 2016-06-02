using UnityEngine;

public struct CameraFollowResult
{
    public bool move { get; set; }
    public Camera cam { get; set; }
    public bool toLobby { get; set; }
}

public class TransportAreaVive : MonoBehaviour {

    public Camera cameraTo;
    public bool toLobby;

    public int cameraFromDepth, cameraToDepth = -1;

    void Start () {
        if (cameraTo == null) {
            Debug.LogError("Error: cameraTo must be set on " + gameObject.name);
            return;
        }
    }

    public CameraFollowResult Transport(Vector3 centre, Vector3 forward)
    {
        CameraFollowResult _result = new CameraFollowResult();
        _result.move = false;
        _result.cam = cameraTo;
        _result.toLobby = toLobby;
        Vector3 distance = transform.position - centre;
        //Debug.Log("cameraTo.depth = " + cameraTo.depth + ". cameraFromDepth = " + cameraFromDepth + ". Dot Result = " + Vector3.Dot(distance.normalized, transform.forward).ToString());
        if (cameraTo.depth == cameraFromDepth) // && Vector3.Dot(distance.normalized, transform.forward) < 0)
        {
            //Debug.Log("cameraTo.depth = " + cameraTo.depth + ". cameraFromDepth = " + cameraFromDepth + ". Dot Result = " + Vector3.Dot(distance.normalized, transform.forward).ToString());
            Debug.Log("MOVE!");
            cameraTo.depth = cameraToDepth;
            GameObject _go = cameraTo.gameObject;
            //foreach(SteamVR_Camera c in FindObjectsOfType<SteamVR_Camera>())
            //{
            //    c.gameObject.SetActive(false);
            //    c.gameObject.SetActive(true);
            //}
            _go.SetActive(false);
            _go.SetActive(true);
            //cameraTo.enabled = true;
            _result.move = true;
        }
        return _result;
    }

    //void OnCollisionStay (Collision col) {
    //    Vector3 centre = transform.position;
        

        //if (perspective != null && col.tag == "Player")
        //{
        //    perspective.positionPlayer(col, transform.position, transform.forward);
        //}
        //if (persp != null && col.tag == "Player")
        //{
        //    persp.positionPlayer(col, transform.position, transform.forward);
        //}
    //}

    //void OnBecameVisible () {
    //    if (_toCam == null) {
    //        return;
    //    }

    //    _toCam.enabled = true;
    //}

    //void OnBecameInvisible () {
    //    if (_toCam == null) {
    //        return;
    //    }

    //    if (cameraTo.depth == -1) {
    //        _toCam.enabled = true;
    //    } else {
    //        _toCam.enabled = false;
    //    }
    //}
}
