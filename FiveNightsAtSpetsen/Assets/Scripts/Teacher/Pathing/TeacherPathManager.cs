using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeacherPathManager : MonoBehaviour
{
    public TeacherWaypointsManager waypoints;

    public Teacher teacher;

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

    List<(TeacherRoomPath room, int repetitions)> altRoute = new();
    bool useAltRoute = false;
    bool useAltRoutePermanent = false;
    int initialAltRouteIndex;

    void OnValidate()
    {
        rooms.Clear();

        foreach (Transform child in transform)
        {
            TeacherRoomPath room = child.GetComponent<TeacherRoomPath>();
            rooms.Add(room);
            room.manager = this;
        }
    }

    public void SetRoute(Dictionary<string, List<(string rid, int reps)>> routeData)
    {
        routeData["Main"].ForEach(e => route.Add((GetRoom(e.rid), e.reps)));
        routeData["Alt"].ForEach(e => altRoute.Add((GetRoom(e.rid), e.reps)));
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
                        rooms[currentRoom].repetitions = GetRoute()[routeIndex].repetitions;
                        EventsManager.Instance.teacherEvents.TeacherEnteredRoom(tempRoomTarget, false);

                        routeIndex = (routeIndex + 1) % route.Count;
                        tempRoomTarget = null;
                        return;
                    }
                }
                else if (GetRoute()[routeIndex].room.exitPoint == interRoomPath[interRoomIndex])
                {
                    inRoom = true;
                    currentRoom = rooms.IndexOf(GetRoute()[routeIndex].room);
                    rooms[currentRoom].repetitions = GetRoute()[routeIndex].repetitions;
                    EventsManager.Instance.teacherEvents.TeacherEnteredRoom(GetRoute()[routeIndex].room, false);

                    routeIndex = (routeIndex + 1) % GetRoute().Count;
                    if (useAltRoute && !useAltRoutePermanent && routeIndex == initialAltRouteIndex) {
                        teacher.TempAltRouteCompleted();
                        SetMainRoute();
                    }
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
        
        if (target == null)
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

    public void SetAltRoute(bool permanent) {
        if (useAltRoute) {
            useAltRoutePermanent = permanent;
            return;
        }

        useAltRoute = true;
        useAltRoutePermanent = permanent;

        (int i, float dist) closest = (-1, 0);
        for (int j = 0; j < altRoute.Count; j++) {
            TeacherRoomPath room = altRoute[j].room;
            float distance = Vector3.Distance(waypoints.waypoints[room.exitPoint].transform.position,
                                              teacher.transform.position);

            if (distance < closest.dist || closest.i == -1)
                closest = (j, distance);
        }

        routeIndex = closest.i;
        initialAltRouteIndex = routeIndex;
    }

    public void SetMainRoute() {
        useAltRoute = false;

        (int i, float dist) closest = (-1, 0);
        for (int j = 0; j < route.Count; j++) {
            TeacherRoomPath room = route[j].room;
            if (room == null) continue;
            float distance = Vector3.Distance(waypoints.waypoints[room.exitPoint].transform.position,
                                              teacher.transform.position);

            if (distance < closest.dist || closest.i == -1)
                closest = (j, distance);
        }

        routeIndex = closest.i;
    }

    List<(TeacherRoomPath room, int repetitions)> GetRoute() {
        return (!useAltRoute ? route : altRoute);
    } 

    (TeacherRoomPath room, bool isRandom) GetRouteTarget()
    {
        if (GetRoute()[routeIndex].room != null)
            return (GetRoute()[routeIndex].room, false);

        // pos = average of last room's exitpoint and next room's exit point,
        // or just last room's exitpoint if the next room also is random
        Vector3 pos = waypoints.waypoints[lastRoom.exitPoint].transform.position
                     + (GetRoute()[(routeIndex + 1) % GetRoute().Count].room == null
                     ? waypoints.waypoints[lastRoom.exitPoint].transform.position
                     : waypoints.waypoints[GetRoute()[(routeIndex + 1) % GetRoute().Count].room.exitPoint].transform.position)
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
        if (selected == GetRoute()[(routeIndex + 1) % GetRoute().Count].room)
        {
            routeIndex = (routeIndex + 1) % GetRoute().Count;
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