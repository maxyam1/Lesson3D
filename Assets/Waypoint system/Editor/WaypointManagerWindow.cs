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
                if (GUILayout.Button("Add brunch"))
                {
                    CreateBrunch();
                }

                if (GUILayout.Button("Create Waypoint Before"))
                {
                    CreateWayPointBefore();
                }
                
                if (GUILayout.Button("Create Waypoint After"))
                {
                    CreateWayPointAfter();
                }
                
                if (GUILayout.Button("Delete Waypoint"))
                {
                    DeleteWayPoint();
                }
            }
        }

        private void CreateBrunch()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot,false);

            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

            Waypoint brunchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();
            brunchedFrom.brunches.Add(waypoint);

            waypoint.transform.position = brunchedFrom.transform.position;
            waypoint.transform.forward = brunchedFrom.transform.forward;

            Selection.activeGameObject = waypoint.gameObject;
        }

        private void CreateWayPoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot,false);

            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
            if (waypointRoot.childCount > 1)
            {
                waypoint.prev = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
                waypoint.prev.next = waypoint;
                waypoint.width = waypoint.prev.width;

                waypoint.transform.position = waypoint.prev.transform.position;
                waypoint.transform.forward = waypoint.prev.transform.forward;
            }

            Selection.activeObject = waypoint.gameObject;
        }

        private void CreateWayPointAfter()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot,false);

            Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            waypointObject.transform.position = selectedWaypoint.transform.position;
            waypointObject.transform.position = selectedWaypoint.transform.forward;

            newWaypoint.prev = selectedWaypoint;

            if (selectedWaypoint.next != null)
            {
                selectedWaypoint.next.prev = newWaypoint;
                newWaypoint.next = selectedWaypoint.next;
            }

            selectedWaypoint.next = newWaypoint;
            newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
            Selection.activeObject = newWaypoint.gameObject;
        }
        
        private void CreateWayPointBefore()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot,false);

            Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            waypointObject.transform.position = selectedWaypoint.transform.position;
            waypointObject.transform.position = selectedWaypoint.transform.forward;

            if (selectedWaypoint.prev != null)
            {
                newWaypoint.prev = selectedWaypoint.prev;
                selectedWaypoint.prev.next = newWaypoint;
            }

            newWaypoint.next = selectedWaypoint;
            selectedWaypoint.prev = newWaypoint;
            
            newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

            Selection.activeObject = newWaypoint.gameObject;
        }
        
        private void DeleteWayPoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            if (selectedWaypoint.next != null)
            {
                selectedWaypoint.next.prev = selectedWaypoint.prev;
            }

            if (selectedWaypoint.prev != null)
            {
                selectedWaypoint.prev.next = selectedWaypoint.next;
                Selection.activeObject = selectedWaypoint.prev.gameObject;
            }
            
            DestroyImmediate(selectedWaypoint.gameObject);
        }
    }
}
