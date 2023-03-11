using System.Collections.Generic;
using UnityEngine;

namespace Waypoint_system
{
    public class WaypointRouteFinder
    {
        private static Dictionary<Waypoint, (float weight, Waypoint cameFrom)> openList =
            new Dictionary<Waypoint, (float weight, Waypoint cameFrom)>();
            
        private static Dictionary<Waypoint, (float weight, Waypoint cameFrom)> closeList =
            new Dictionary<Waypoint, (float weight, Waypoint cameFrom)>();
        
        public static void FindRoute(out List<Waypoint> resultRoute, Waypoint begin, Waypoint target)//A* алгоритм
        {
            resultRoute = new List<Waypoint>();

            //closeList.Add(begin, (0, null));
            RecursiveStepAStar(begin, target);

            Waypoint to = target;
            Waypoint from = closeList[target].cameFrom;
            
            while (from != begin)
            {
                resultRoute.Add(to);
                to = from;
                from = closeList[to].cameFrom;
            }
            
            resultRoute.Add(from);
        }

        private static void RecursiveStepAStar(Waypoint current, Waypoint target)
        {
            float minWeight = float.MaxValue;
            Waypoint minWeightWaypoint = null;
            
            foreach (var link in current.links)
            {
                bool updated = false;
                float weight = FindWeight(link, current, target);
                if ((!openList.ContainsKey(link) && !closeList.ContainsKey(link)))
                {
                    openList.Add(link, (weight, current));
                    updated = true;

                    if (link == target)
                    {
                        closeList.Add(link, (weight, current));
                        MoveFromOpenToCloseLIst(current);
                        return;
                    }
                }
                else if (openList.ContainsKey(link))
                {
                    if (weight < openList[link].weight)
                    {
                        openList[link] = (weight, current);
                        updated = true;
                    }
                }

                if (weight < minWeight && updated)
                {
                    minWeight = weight;
                    minWeightWaypoint = link;
                }
            }

            if (openList.ContainsKey(current))
            {
                MoveFromOpenToCloseLIst(current);
            }

            RecursiveStepAStar(minWeightWaypoint ,target);
        }

        private static void MoveFromOpenToCloseLIst(Waypoint waypoint)
        {
            var value = openList[waypoint];
            openList.Remove(waypoint);
            
            closeList.Add(waypoint, value);
        }

        private static float FindWeight(Waypoint to, Waypoint cameFrom, Waypoint target)
        {
            float weight = Vector3.Distance(cameFrom.transform.position, to.transform.position) +
                           Vector3.Distance(to.transform.position, target.transform.position);
            return weight;
        }
    }
}
