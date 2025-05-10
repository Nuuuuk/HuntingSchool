using UnityEngine;

public class DoorOpenTrigger : MonoBehaviour, ILevelReset
{
    public DoorRotAnim doorAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnim.OpenDoor();
            GetComponent<Collider>().enabled = false;
        }
    }

    public void LevelReset()
    {
        GetComponent<Collider>().enabled = true;
    }
}
