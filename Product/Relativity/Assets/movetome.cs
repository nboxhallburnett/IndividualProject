using UnityEngine;

public class movetome : MonoBehaviour
{

    public Transform pos;
    public Transform lobbyCamera;
    public bool move = true;

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position = pos.position;
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.collider.GetComponent<TransportAreaVive>() != null)
        {
            CameraFollowResult _result = col.collider.GetComponent<TransportAreaVive>().Transport(pos.position, pos.forward);
            if (_result.move)
            {
                Debug.Log("We've Moved!");
                if (_result.toLobby)
                {
                    pos = lobbyCamera;
                }
                else
                {
                    pos = _result.cam.transform;
                }
            }
        }
    }
}
