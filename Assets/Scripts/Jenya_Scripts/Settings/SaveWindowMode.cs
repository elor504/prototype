using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Toggle))]
public class SaveWindowMode : MonoBehaviour
{
    const string PrefName = "fullscreenValue";
    private bool toggleValue;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        

        toggle.onValueChanged.AddListener(new UnityAction<bool>(index => {
            if (toggle.isOn == true)
            {
                toggleValue = true;
            }
            else
            {
                toggleValue = false;
            }

            PlayerPrefs.SetInt(PrefName, boolToInt(toggleValue));
            PlayerPrefs.Save();
        }));
    }


    private void Start()
    {
        toggle.isOn = intToBool(PlayerPrefs.GetInt(PrefName));
    }


    // Converting the bool so player prefs can save the int value
    int boolToInt(bool val)
    {
        if (val== true)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
}
