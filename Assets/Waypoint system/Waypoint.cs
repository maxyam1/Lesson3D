using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Waypoint_system
{
    public class Waypoint : MonoBehaviour
    {
        public List<Waypoint> links = new List<Waypoint>();

        [Range(0f, 5f)] public float width;

        public Vector3 GetPosition()
        {
            Vector3 leftBound = transform.position + transform.right * (-width / 2);
            Vector3 rightBound = transform.position + transform.right * (width / 2);

            return Vector3.Lerp(leftBound , rightBound, Random.Range(0f,1f));
        }

       
    }
}
