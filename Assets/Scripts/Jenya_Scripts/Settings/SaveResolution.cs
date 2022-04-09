using UnityEngine.Events;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class SaveResolution : MonoBehaviour
{
    const string PrefName = "resolutionValue";
    private TMP_Dropdown dropdown;



    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(PrefName, dropdown.value);
            PlayerPrefs.Save();
        }));
    }

    void Start()
    {
        dropdown.value = PlayerPrefs.GetInt(PrefName);
    }
}
