using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeacherPathManager : MonoBehaviour
{
    public TeacherWaypointsManager waypoints;

    public List<TeacherRoomPath> rooms = new();
    public int currentRoom = 0;

    public bool inRoom = false;
    public List<int> interRoomPath = new() { 0, 1, 2, 3 };
    public int interRoomIndex = 0;
    public int roomDir = 1;

    public TeacherRoomPath target;
    public TeacherRoomPath lastRoom;
    public float roomEntryChance = 0.8f;

    public Color interRoomColor = Color.blue;

    List<(TeacherRoomPath room, int repetitions)> route = new();
    public int routeIndex = 0;
    public TeacherRoomPath tempRoomTarget;

    void OnValidate()
    {
        rooms.Clear();

        foreach (Transform child in transform)
            rooms.Add(child.GetComponent<TeacherRoomPath>());
    }

    public void SetRoute(List<(string rid, int reps)> routeData)
    {
        routeData.ForEach(e => route.Add((GetRoom(e.rid), e.reps)));
    }

    public void Next()
    {
        if (!inRoom)
        {
            if (target == null)
            {
                if (tempRoomTarget != null)
                {
                    if (tempRoomTarget.exitPoint == interRoomPath[interRoomIndex])
                    {
                        inRoom = true;
                        currentRoom = rooms.IndexOf(tempRoomTarget);
                        rooms[currentRoom].repetitions = route[routeIndex].repetitions;
                        EventsManager.Instance.teacherEvents.TeacherEnteredRoom(tempRoomTarget, false);

                        routeIndex = (routeIndex + 1) % route.Count;
                        tempRoomTarget = null;
                        return;
                    }
                }
                else if (route[routeIndex].room.exitPoint == interRoomPath[interRoomIndex])
                {
                    inRoom = true;
                    currentRoom = rooms.IndexOf(route[routeIndex].room);
                    rooms[currentRoom].repetitions = route[routeIndex].repetitions;
                    EventsManager.Instance.teacherEvents.TeacherEnteredRoom(route[routeIndex].room, false);

                    routeIndex = (routeIndex + 1) % route.Count;
                    return;
                }
            }
            else
            {
                if (target.exitPoint == interRoomPath[interRoomIndex])
                {
                    inRoom = true;
                    currentRoom = rooms.IndexOf(target);
                    EventsManager.Instance.teacherEvents.TeacherEnteredRoom(rooms[currentRoom], true);
                    target = null;
                    return;
                }
            }

            if (roomDir == 1 && interRoomIndex == interRoomPath.Count - 1)
                roomDir = -1;
            else if (roomDir == -1 && interRoomIndex == 0)
                roomDir = 1;

            interRoomIndex += roomDir;
        }
        else
        {
            rooms[currentRoom].Next();
        }
    }

    public void TargetRoom((TeacherRoomPath room, bool isRandom) room, bool isRoute = false)
    {
        if (!isRoute)
        {
            target = room.room;
            if (inRoom)
                rooms[currentRoom].repetitions = 0;
        }

        int i = interRoomIndex;
        int ti = interRoomPath.IndexOf(room.room.exitPoint);

        roomDir = ((ti >= i) ? 1 : -1);

        if (room.isRandom)
            tempRoomTarget = room.room;
    }

    public void RoomFinished(TeacherRoomPath room)
    {
        lastRoom = room;
        inRoom = false;
        interRoomIndex = interRoomPath.IndexOf(room.exitPoint);
        TargetRoom(GetRouteTarget(), true);
    }

    public Vector3 GetPos()
    {
        if (!inRoom)
            return waypoints.waypoints[interRoomPath[interRoomIndex]].transform.position;
        else
            return rooms[currentRoom].GetPos();
    }

    public TeacherWaypoint GetWaypoint()
    {
        if (!inRoom)
            return waypoints.waypoints[interRoomPath[interRoomIndex]];
        else
            return rooms[currentRoom].GetWaypoint();
    }

    public TeacherRoomPath GetRoom(string id)
    {
        foreach (TeacherRoomPath room in rooms)
            if (id == room.id) return room;
        return null;
    }

    (TeacherRoomPath room, bool isRandom) GetRouteTarget()
    {
        if (route[routeIndex].room != null)
            return (route[routeIndex].room, false);

        // pos = average of last room's exitpoint and next room's exit point,
        // or just last room's exitpoint if the next room also is random
        Vector3 pos = waypoints.waypoints[lastRoom.exitPoint].transform.position
                     + (route[(routeIndex + 1) % route.Count].room == null
                     ? waypoints.waypoints[lastRoom.exitPoint].transform.position
                     : waypoints.waypoints[route[(routeIndex + 1) % route.Count].room.exitPoint].transform.position)
                     / 2;

        List<(TeacherRoomPath room, float distance)> dists = rooms.Select(r => (r, Vector3.Distance(pos, r.pos))).ToList();
        dists = dists.Where(d => d.room != lastRoom).OrderByDescending(d => d.distance).ToList();

        float totalWeight = dists.Sum(d => 1f / Mathf.Max(d.distance, 0.1f));

        float randomPoint = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        TeacherRoomPath selected = null;
        foreach (var dist in dists)
        {
            cumulativeWeight += 1f / Mathf.Max(dist.distance, 0.1f);
            if (randomPoint <= cumulativeWeight)
            {
                selected = dist.room;
                break;
            }
        }

        // Fallback
        selected ??= dists.OrderBy(d => d.distance).ToList()[0].room;

        // if selected room is next on the route, ignore this random step
        if (selected == route[(routeIndex + 1) % route.Count].room)
        {
            routeIndex = (routeIndex + 1) % route.Count;
            return (selected, false);
        }

        return (selected, true);
    }

    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.waypoints.Contains(null)) return;

        for (int i = 0; i < interRoomPath.Count - 1; i++)
        {
            Vector3 start = GizmoGetPos(i);
            Vector3 end = GizmoGetPos(i + 1);

            Gizmos.color = interRoomColor;

            Gizmos.DrawLine(start, end);
        }
    }

    public Vector3 GizmoGetPos(int i)
    {
        return waypoints.waypoints[interRoomPath[i]].transform.position;
    }
}