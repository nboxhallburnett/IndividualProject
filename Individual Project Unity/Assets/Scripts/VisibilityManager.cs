using UnityEngine;
using System;

public abstract class VisibilityManager : MonoBehaviour {

    Camera gameCamera;
    bool seenInvisible = false;
    bool seenVisible = false;
    public Collider objCollider;

    // Use this for initialization
    void Start () {
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
        processEffect(SetInvisible, SetVisible);
    }

    /// <summary>
    /// Function to call when the object transitions to the 'invisible' state
    /// </summary>
    public abstract void SetInvisible();

    /// <summary>
    /// Function to call when the object transitions to the 'visible' state
    /// </summary>
    public abstract void SetVisible();

    bool InCamera () {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(gameCamera), objCollider.bounds);
    }

    bool IsVisible () {
        return GetComponent<MeshRenderer>().enabled;
    }

    void processEffect(Action invisibleState, Action visibleState) {
        if (InCamera() && !seenVisible && IsVisible()) {
            seenVisible = true;
        } else if (!InCamera() && seenVisible && IsVisible()) {
            invisibleState();
            seenVisible = false;
        } else if (InCamera() && !seenInvisible && !IsVisible()) {
            seenInvisible = true;
        } else if (!InCamera() && seenInvisible && !IsVisible()) {
            visibleState();
            seenInvisible = false;
        }
    }
}
