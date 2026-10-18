using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider m_slider;

    /// <summary>
    /// Expects a value between 0 and 1.
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        m_slider.SetValueWithoutNotify(Mathf.Clamp(value, 0f, 1f));
    }
}
