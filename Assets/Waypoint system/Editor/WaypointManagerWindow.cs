using System;
using UnityEditor;
using UnityEngine;

namespace Waypoint_system.Editor
{
    public class WaypointManagerWindow : EditorWindow
    {
        [MenuItem("Tools/Waypoint Editor")]
        public static void Open()
        {
            GetWindow<WaypointManagerWindow>();
        }

        public Transform waypointRoot;

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);
            EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

            if (waypointRoot == null)
            {
                EditorGUILayout.HelpBox("Root transform cannot be null", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                DrawButtons();
                EditorGUILayout.EndVertical();
            }

            obj.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Create Waypoint"))
            {
                CreateWayPoint();
            }
            


            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Join Cover"))
                {
                    JoinCover();
                }
                if (GUILayout.Button("Join Waypoint"))
                {
                    JoinWaypoint();
                }
                if (GUILayout.Button("Delete waypoint"))
                {
                    DeleteWayPoint();
                }
            }
        }
        
        private void CreateWayPoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot,false);

            Selection.activeObject = waypointObject;
        }

        private void JoinWaypoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot,false);

            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            
            waypoint.links.Add(selectedWaypoint);
            selectedWaypoint.links.Add(waypoint);
            
            waypoint.width = selectedWaypoint.width;

            waypoint.transform.position = selectedWaypoint.transform.position;
            waypoint.transform.rotation = selectedWaypoint.transform.rotation;

            Selection.activeObject = waypointObject;
        }

        private void JoinCover()
        {
            GameObject waypointObject = new GameObject("Cover " + waypointRoot.childCount, typeof(Cover));
            waypointObject.transform.SetParent(waypointRoot,false);

            Cover waypoint = waypointObject.GetComponent<Cover>();
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            
            waypoint.links.Add(selectedWaypoint);
            selectedWaypoint.links.Add(waypoint);
            
            waypoint.width = selectedWaypoint.width;

            waypoint.transform.position = selectedWaypoint.transform.position;
            waypoint.transform.rotation = selectedWaypoint.transform.rotation;

            Selection.activeObject = waypointObject;
        }
        
        
        private void DeleteWayPoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            foreach (var link in selectedWaypoint.links)
            {
                link.links.Remove(selectedWaypoint);
            }

            DestroyImmediate(selectedWaypoint.gameObject);
        }
    }
}
