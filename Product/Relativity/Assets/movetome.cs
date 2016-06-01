using UnityEngine;

public class movetome : MonoBehaviour {

    public Transform pos;
	
	// Update is called once per frame
	void Update () {
        transform.position = pos.position;
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.collider.name);
    }
}
