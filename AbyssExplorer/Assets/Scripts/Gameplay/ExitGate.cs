using UnityEngine;

public class ExitGate : MonoBehaviour
{
    public bool isUnlocked = false;

    [Header("Door Settings")]
    public float openAngle = -90f;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool opening = false;

    private void Start()
    {
        closedRotation = transform.rotation;

        openRotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + openAngle,
            transform.eulerAngles.z
        );
    }

    private void Update()
    {
        if (opening)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRotation,
                openSpeed * Time.deltaTime
            );
        }
    }

    public void UnlockExit()
    {
        isUnlocked = true;
        opening = true;

        Debug.Log("Gate opening!");
    }
}