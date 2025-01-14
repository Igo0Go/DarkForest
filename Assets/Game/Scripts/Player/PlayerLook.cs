using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт обзора камерой
/// </summary>
[HelpURL("https://docs.google.com/document/d/1llgWK3zJK7km7DMyi_GHh63LZUJngJppIuZIfccWwtc/edit?usp=sharing")]
public class PlayerLook : PlayerPart
{
    [Tooltip("Объект - камера")]
    public Transform cam;
    [SerializeField, Tooltip("Объект - пустышка, в которой находится камера")] 
    private Transform camBufer;
    [SerializeField, Range(0, 2), Tooltip("Чувствительность камеры по горизонтали")]
    private float sensitivityHor = 0.5f;
    [SerializeField, Range(0, 2), Tooltip("Чувствительность камеры по вертикали")]
    private float sensitivityVert = 0.5f;
    [SerializeField, Tooltip("Ограничение угла камеры снизу"), Range(-90, 0)] private float minimumVert = -45.0f;
    [SerializeField, Tooltip("Ограничение угла камеры сверху"), Range(0, 90)] private float maximumVert = 45.0f;
    public LayerMask ignoreMask;

    private float _rotationX = 0;
    private const float multiplicator = 100;
    private float delta;

    public override void Activate()
    {
        SetCursorVisible(false);
    }

    private void Update()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * Time.deltaTime * multiplicator * GameSettings.sensivity;
        delta = Input.GetAxis("Mouse X") * sensitivityHor * Time.deltaTime * multiplicator * GameSettings.sensivity;
    }

    void LateUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        cam.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }

    /// <summary>
    /// Задать видимость курсора
    /// </summary>
    /// <param name="value">Можно ли пользоваться курсором</param>
    public void SetCursorVisible(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public Vector3 GetSpellTargetPoint()
    {
        Vector3 dir = Vector3.zero;

        if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, 300, ~ignoreMask))
        {
            return hitInfo.point;
        }

        return cam.position + cam.forward*100;
    }

    public Vector3 GetSpellTargetPoint(out Vector3 normal)
    {
        Vector3 dir = Vector3.zero;
        normal = Vector3.up;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, 300, ~ignoreMask))
        {
            normal = hitInfo.normal;
            return hitInfo.point;
        }

        return cam.position + cam.forward * 100;
    }

    public Vector3 GetSpellTargetPoint(out Vector3 normal, LayerMask ignoreMask)
    {
        Vector3 dir = Vector3.zero;
        normal = Vector3.up;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, 300, ~ignoreMask))
        {
            normal = hitInfo.normal;
            return hitInfo.point;
        }

        return cam.position + cam.forward * 100;
    }
}
