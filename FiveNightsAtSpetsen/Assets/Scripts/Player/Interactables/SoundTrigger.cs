using UnityEngine;

public class SoundTrigger : Interactable
{
    public Transform teacher;
    public TeacherRoomPath room;
    public float soundRange = 20f;

    public override void Interact()
    {
        float distanceToTeacher = Vector3.Distance(teacher.position, transform.position);
        if (distanceToTeacher <= soundRange)
            EventsManager.Instance.teacherEvents.PlayerMadeSound(room);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soundRange);
    }
}