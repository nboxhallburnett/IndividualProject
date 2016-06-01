using UnityEngine;

public class flash : MonoBehaviour {

    Light _light;

    // Use this for initialization
    void Start () {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update () {
        _light.intensity = 0.5f + Mathf.Sin(Time.timeSinceLevelLoad * 5f);
    }
}
