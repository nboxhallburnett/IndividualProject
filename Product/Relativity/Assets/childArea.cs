using UnityEngine;
using System.Collections;

public class childArea : MonoBehaviour {

    public GameObject ParentArea;
    Vector3 _offset;

	// Use this for initialization
	void Start () {
        _offset = ParentArea.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = ParentArea.transform.position - _offset;
	}
}
