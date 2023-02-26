using UnityEditor;
using UnityEngine;

namespace Waypoint_system.Editor
{
    [InitializeOnLoad()]
    public class WaypointEditor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.yellow * 0.5f;
            }
            
            Gizmos.DrawSphere(waypoint.transform.position, 0.1f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
                waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

            if (waypoint.prev != null)
            {
                Gizmos.color = Color.red;
                Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
                Vector3 offsetTo = waypoint.prev.transform.right * waypoint.prev.width / 2f;
                
                Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.prev.transform.position + offsetTo);
            }
            
            if (waypoint.next != null)
            {
                Gizmos.color = Color.green;
                Vector3 offset = waypoint.transform.right * waypoint.width / -2f;
                Vector3 offsetTo = waypoint.next.transform.right * waypoint.next.width / -2f;
                
                Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.next.transform.position + offsetTo);
            }

            if (waypoint.brunches != null)
            {
                foreach (var brunch in waypoint.brunches)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(waypoint.transform.position, brunch.transform.position);
                }
            }
        }
    }
}
