using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickControll : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler 
{
    private Image joystickBorder;
    private Image joystickCircle;
    private Vector2 inputVector;

    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";

    void OnEnable()
    {
        CreateVirtualAxes();
    }

    void CreateVirtualAxes()
    {
        // Cegah duplikasi axis
        if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
        }
        if (CrossPlatformInputManager.AxisExists(verticalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
        }

        // Daftarkan axis baru
        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);

        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    private void Start()
    {
        joystickBorder = GetComponent<Image>();
        joystickCircle = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnPointerDown(PointerEventData stick)
    {
        OnDrag(stick);
    }

    public virtual void OnPointerUp(PointerEventData stick)
    {
        inputVector = Vector2.zero;
        UpdateAxis(inputVector);
        joystickCircle.rectTransform.anchoredPosition = Vector2.zero;
    }

    public virtual void OnDrag(PointerEventData stick)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBorder.rectTransform, stick.position, stick.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBorder.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBorder.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x , pos.y );
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystickCircle.rectTransform.anchoredPosition = 
                new Vector2(inputVector.x*(joystickBorder.rectTransform.sizeDelta.x/2), 
                            inputVector.y*(joystickBorder.rectTransform.sizeDelta.y/2));
        }

        UpdateAxis(inputVector);
    }

    private void UpdateAxis(Vector2 axis)
    {
        if (m_HorizontalVirtualAxis != null) m_HorizontalVirtualAxis.Update(axis.x);
        if (m_VerticalVirtualAxis != null) m_VerticalVirtualAxis.Update(axis.y);
    }

    void OnDisable()
    {
        if (m_HorizontalVirtualAxis != null) m_HorizontalVirtualAxis.Remove();     
        if (m_VerticalVirtualAxis != null) m_VerticalVirtualAxis.Remove();
    }
}
