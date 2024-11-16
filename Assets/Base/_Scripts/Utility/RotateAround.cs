using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;
    private void FixedUpdate()
    {
        if (yAxis)
            transform.Rotate(Vector3.up * Time.fixedDeltaTime * (UIManager.timeScale == 1 ? rotateSpeed : rotateSpeed * 2));

        if (xAxis)
            transform.Rotate(Vector3.right * Time.fixedDeltaTime * (UIManager.timeScale == 1 ? rotateSpeed : rotateSpeed * 2));

        if (zAxis)
            transform.Rotate(Vector3.forward * Time.fixedDeltaTime * (UIManager.timeScale == 1 ? rotateSpeed : rotateSpeed * 2));
    }

}

