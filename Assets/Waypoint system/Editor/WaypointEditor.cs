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
            Color waypointColor = Color.yellow;
            
            if (waypoint is Cover)
            {
                waypointColor = Color.green;   
            }

            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = waypointColor;
            }
            else
            {
                Gizmos.color = waypointColor * 0.7f;
            }
            
            Gizmos.DrawSphere(waypoint.transform.position, 0.1f);

            Gizmos.color = Color.black;
            Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
                waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

            
            if (waypoint.links != null)
            {
                foreach (var link in waypoint.links)
                {
                    Gizmos.color = Color.red;
                    Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
                    Vector3 offsetTo = link.transform.right * link.width / 2f;
                
                    Gizmos.DrawLine(waypoint.transform.position + offset, link.transform.position + offsetTo);
                    
                    
                    Gizmos.color = Color.green;
                    offset = waypoint.transform.right * waypoint.width / -2f;
                    offsetTo = link.transform.right * link.width / -2f;
                
                    Gizmos.DrawLine(waypoint.transform.position + offset, link.transform.position + offsetTo);
                }
            }
        }
    }
}
