using UnityEngine;

namespace NEGeo {
    public class TransportArea : MonoBehaviour {

        RTexCameraPosition perspective;
        CameraRenderPosition persp;

        // Use this for initialization
        void Start () {
            perspective = GetComponentInChildren<RTexCameraPosition>();
            persp = GetComponentInChildren<CameraRenderPosition>();
            if (perspective == null && persp == null) {
                Debug.LogError("No RTexCameraPosition or CameraRenderPosition script attached to child.");
                return;
            }

        }

        void OnTriggerStay (Collider col) {
            if (perspective != null && col.tag == "Player") {
                perspective.positionPlayer(col, transform.position, transform.forward);
            }
            if (persp != null && col.tag == "Player") {
                persp.positionPlayer(col, transform.position, transform.forward);
            }
        }


    }
}