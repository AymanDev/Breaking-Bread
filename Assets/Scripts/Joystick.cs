using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum AxisOption
    {
        Both,
        OnlyHorizontal,
        OnlyVertical
    }

    public int MovementRange = 100;
    public AxisOption axesToUse = AxisOption.Both;

    Vector3 m_StartPos;
    public bool m_UseX;
    public bool m_UseY;
    public float horizontal;
    public float vertical;

    private void OnEnable()
    {
        CreateVirtualAxes();
    }

    private void Start()
    {
        m_StartPos = transform.position;
    }

    private void UpdateVirtualAxes(Vector3 value)
    {
        var delta = m_StartPos - value;
        delta.y = -delta.y;
        delta /= MovementRange;

        if (m_UseX)
        {
            horizontal = -delta.x;
        }

        if (m_UseY)
        {
            vertical = delta.y;
        }
    }

    private void CreateVirtualAxes()
    {
        m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);
    }


    public void OnDrag(PointerEventData data)
    {
        var newPos = Vector3.zero;

        if (m_UseX)
        {
            var delta = data.position.x - m_StartPos.x;
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.x = delta;
        }

        if (m_UseY)
        {
            var delta = data.position.y - m_StartPos.y;
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.y = delta;
        }

        transform.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
        UpdateVirtualAxes(transform.position);
    }


    public void OnPointerUp(PointerEventData data)
    {
        transform.position = m_StartPos;
        UpdateVirtualAxes(m_StartPos);
    }

    public void OnPointerDown(PointerEventData data)
    {
        
    }
}