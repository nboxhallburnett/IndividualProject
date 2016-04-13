using UnityEngine;

namespace NEGeo {
    public static class Helper {

        public static int renderDepth = 0;

        public static T[] FindComponentsInChildrenWithTag<T> (this GameObject parent, string tag) where T : Component {
            T[] results = new T[0];
            foreach (T t in parent.GetComponentsInChildren<T>()) {
                if (t.tag == tag) {
                    System.Array.Resize(ref results, results.Length + 1);
                    results[results.Length - 1] = t;
                }
            }
            return results;
        }

        public static T FindComponentInChildrenWithTag<T> (this GameObject parent, string tag) where T : Component {
            T[] results = new T[0];
            foreach (T t in parent.GetComponentsInChildren<T>()) {
                if (t.tag == tag) {
                    System.Array.Resize(ref results, results.Length + 1);
                    results[results.Length - 1] = t;
                }
            }
            return results[0];
        }

        public static Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, Vector3 angle) {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angle) * dir;
            return dir + pivot;
        }

        public static void SortDistances (ref Vector3[] positions, Vector3 origin) {
            float[] distances = new float[positions.Length];
            for (int i = 0; i < positions.Length; i++) {
                distances[i] = (positions[i] - origin).sqrMagnitude;
            }
            System.Array.Sort(distances, positions);
        }

        public static Vector3 FindClosestPoint (Vector3[] positions, Vector3 point) {
            Vector3 closest = positions[0];
            for (int i = 0; i < positions.Length; i++) {
                if (Vector3.Magnitude(positions[i] - point) < Vector3.Magnitude(closest - point)) {
                    closest = positions[i];
                }
            }
            return closest;
        }

        public static Vector3 FindClosestPoint (Vector3[] positions, Vector3[] points) {
            Vector3 closest = points[0];

            for (int i = 0; i < points.Length; i++) {
                if (Vector3.Magnitude(FindClosestPoint(positions, points[i]) - points[i]) < Vector3.Magnitude(FindClosestPoint(positions, closest) - closest)) {
                    closest = points[i];
                }
            }

            /*for (int i = 0; i < positions.Length; i++) {
                if (Vector3.Magnitude(positions[i] - point) < Vector3.Magnitude(closest - point)) {
                    closest = positions[i];
                }
            }*/
            return closest;
        }

        public static float FindShortestMagnitude (Vector3[] positions, Vector3 point) {
            Vector3 closest = positions[0];
            float shortest = float.MaxValue;
            for (int i = 0; i < positions.Length; i++) {
                if (Vector3.Magnitude(positions[i] - point) < shortest) {
                    closest = positions[i];
                    shortest = Vector3.Magnitude(positions[i] - point);
                }
            }
            return shortest;
        }
    }
}