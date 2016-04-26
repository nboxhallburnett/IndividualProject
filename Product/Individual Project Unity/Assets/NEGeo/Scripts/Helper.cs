using UnityEngine;

namespace NEGeo {
    public static class Helper {

        public static int renderDepth = 0;

        /// <summary>
        /// Returns an array containing all the components of type T of the provided object, which also have the specified tag.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="parent">Object to search the children of</param>
        /// <param name="tag"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the first component of type T contained in the children of the provided object, which has the specified tag.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="parent">Object to seatch the children of</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static T FindComponentInChildrenWithTag<T> (this GameObject parent, string tag) where T : Component {
            foreach (T t in parent.GetComponentsInChildren<T>()) {
                if (t.tag == tag) {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Rotate a point around a pivot in 3D space, by the provided angle.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pivot"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, Vector3 angle) {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angle) * dir;
            return dir + pivot;
        }

        /// <summary>
        /// Sorts the provided Vector3 array by their distance from a provided Vector3
        /// </summary>
        /// <param name="positions">Array to sort</param>
        /// <param name="origin">Point to compare against</param>
        public static void SortDistances (ref Vector3[] positions, Vector3 origin) {
            float[] distances = new float[positions.Length];
            for (int i = 0; i < positions.Length; i++) {
                distances[i] = (positions[i] - origin).sqrMagnitude;
            }
            System.Array.Sort(distances, positions);
        }

        /// <summary>
        /// Returns the point in a Vector3 array which is closest to a specified Vector3
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 FindClosestPoint (Vector3[] positions, Vector3 point) {
            Vector3 closest = positions[0];
            for (int i = 0; i < positions.Length; i++) {
                if (Vector3.Magnitude(positions[i] - point) < Vector3.Magnitude(closest - point)) {
                    closest = positions[i];
                }
            }
            return closest;
        }

        /// <summary>
        /// Returns the distance between the closest point from a Vector3 array, and a specified Vector3
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float FindShortestMagnitude (Vector3[] positions, Vector3 point) {
            Vector3 closest = positions[0];
            float shortest = float.MaxValue;
            for (int i = 0; i < positions.Length; i++) {
                float _shortest = Vector3.Magnitude(positions[i] - point);
                if (_shortest < shortest) {
                    closest = positions[i];
                    shortest = _shortest;
                }
            }
            return shortest;
        }
    }
}