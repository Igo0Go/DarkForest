using System.Collections;
using UnityEngine;

/// <summary>
/// ������ ������ �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1llgWK3zJK7km7DMyi_GHh63LZUJngJppIuZIfccWwtc/edit?usp=sharing")]
public class PlayerLook : MonoBehaviour
{
    [Tooltip("������ - ������")] public Transform cam;
    [SerializeField, Tooltip("������ - ��������, � ������� ��������� ������")] private Transform camBufer;
    [SerializeField, Range(0, 2), Tooltip("���������������� ������ �� �����������")]
    private float sensitivityHor = 0.5f;
    [SerializeField, Range(0, 2), Tooltip("���������������� ������ �� ���������")]
    private float sensitivityVert = 0.5f;
    [SerializeField, Tooltip("����������� ���� ������ �����"), Range(-90, 0)] private float minimumVert = -45.0f;
    [SerializeField, Tooltip("����������� ���� ������ ������"), Range(0, 90)] private float maximumVert = 45.0f;
    public LayerMask ignoreMask;

    private float _rotationX = 0;
    private const float multiplicator = 100;
    private float delta;

    private void Start()
    {
        SetCursorVisible(false);
    }

    private void Update()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * Time.deltaTime * multiplicator;
        delta = Input.GetAxis("Mouse X") * sensitivityHor * Time.deltaTime * multiplicator;
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
    /// ������ ��������� �������
    /// </summary>
    /// <param name="value">����� �� ������������ ��������</param>
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
}
