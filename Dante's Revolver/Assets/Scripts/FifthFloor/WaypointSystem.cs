using UnityEngine;
using System.Collections.Generic;

public class WaypointSystem : MonoBehaviour
{
    [Header("Waypoints Configuration")]
    public List<Transform> waypoints = new List<Transform>();
    public bool loopPath = true;
    public float waypointRadius = 1.0f;

    public Vector3 GetWaypointPosition(int index)
    {
        if (waypoints.Count == 0 || waypoints[index] == null)
            return transform.position;

        return waypoints[index].position;
    }

    public int GetNextWaypointIndex(int currentIndex)
    {
        if (waypoints.Count == 0) return 0;

        int nextIndex = currentIndex + 1;

        if (nextIndex >= waypoints.Count)
        {
            return loopPath ? 0 : waypoints.Count - 1;
        }

        return nextIndex;
    }

    public bool HasReachedWaypoint(Vector3 currentPosition, int targetWaypointIndex)
    {
        if (waypoints.Count == 0 || targetWaypointIndex >= waypoints.Count || waypoints[targetWaypointIndex] == null)
            return false;

        Vector3 targetPosition = GetWaypointPosition(targetWaypointIndex);
        return Vector3.Distance(currentPosition, targetPosition) <= waypointRadius;
    }

    public int WaypointsCount
    {
        get { return waypoints.Count; }
    }

    // Debug visualization
    private void OnDrawGizmos()
    {
        if (waypoints.Count == 0) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;

            // Draw waypoint sphere
            Gizmos.DrawWireSphere(waypoints[i].position, waypointRadius);

            // Draw line to next waypoint
            if (i < waypoints.Count - 1 && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
            else if (loopPath && waypoints[0] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }
        }
    }
}