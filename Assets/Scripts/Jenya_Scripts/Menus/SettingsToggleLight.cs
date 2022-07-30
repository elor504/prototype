using UnityEngine.UI;
using UnityEngine;

public class SettingsToggleLight : MonoBehaviour
{
    public Toggle toggle;
    public GameObject toggleLampMask;
    public GameObject dropdownItemHighlight;


    public void ChangeToggleVisuals()
    {
        if (toggle.isOn)
        {
            toggleLampMask.SetActive(true);
        }
        else
        {
            toggleLampMask.SetActive(false);
        }
    }

    public void ChangeItemHighlight(bool show)
    {
        if (show)
        {
            dropdownItemHighlight.SetActive(true);
        }
        else
        {
            dropdownItemHighlight.SetActive(false);
        }
    }
}
