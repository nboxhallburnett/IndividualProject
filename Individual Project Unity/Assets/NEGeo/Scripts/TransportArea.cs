using UnityEngine;
using System.Collections;

namespace NEGeo {
    public class TransportArea : MonoBehaviour {

        RTexCameraPosition perspective;

        // Use this for initialization
        void Start () {
            perspective = GetComponentInChildren<RTexCameraPosition>();
            if (perspective == null) {
                Debug.LogError("No RTexCameraPosition script attached to child.");
                return;
            }

        }

        // Update is called once per frame
        void Update () {

        }

        void OnTriggerStay (Collider col) {
            // TODO: Some fancy shmancy stuff in here for player re-location. Probably something along
            //       the lines of 'If the player is > half way through this area, move it relative to the linked one
            if (perspective != null && col.tag == "Player") {
                perspective.positionPlayer(col, transform.position, transform.forward);
            }
        }


    }
}