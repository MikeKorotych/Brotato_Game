using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Transform target;


    [Header(" Settings ")]
    [SerializeField] private Vector2 minMaxXY;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target has been specified");
            return;
        }



        Vector3 targetPostion = target.position;
        targetPostion.z = transform.position.z;

        targetPostion.x = Mathf.Clamp(targetPostion.x, -minMaxXY.x, minMaxXY.x);
        targetPostion.y = Mathf.Clamp(targetPostion.y, -minMaxXY.y, minMaxXY.y);

        transform.position = targetPostion;
    }
}
