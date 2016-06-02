using UnityEngine;

public class hideDoor : MonoBehaviour {

    public GameObject Reference, Door, Window, Frame, Frame2;
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Vector3.Dot((transform.position - Reference.transform.position).normalized, -Reference.transform.up));
        Debug.Log(Reference.transform.forward);
        Debug.DrawRay(Reference.transform.position, Reference.transform.up);
        if (Vector3.Dot((transform.position - Reference.transform.position).normalized, -Reference.transform.up) > 0)
        {
            Door.SetActive(false);
            Window.SetActive(false);
            Frame.SetActive(false);
            Frame2.SetActive(false);
        }
        else
        {
            Door.SetActive(true);
            Window.SetActive(true);
            Frame.SetActive(true);
            Frame2.SetActive(true);
        }
	}
}
