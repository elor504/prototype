using UnityEngine.UI;
using UnityEngine;

public class SettingsToggleLight : MonoBehaviour
{
    public Toggle toggle;
    public GameObject toggleLampMask;


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
}
