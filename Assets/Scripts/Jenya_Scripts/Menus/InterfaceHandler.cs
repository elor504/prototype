using UnityEngine.UI;
using UnityEngine;

public class InterfaceHandler : MonoBehaviour
{
    private static InterfaceHandler IHinstance;
    public static InterfaceHandler GetInstance => IHinstance;
    [HideInInspector]public UIState uiState;

    public GameObject MainMenu, LevelSelection, Settings, Credits;
    public Button quitB;
    [SerializeField]private int targetFPS;

    private void Awake()
    {
        if(IHinstance == null)
        {
            IHinstance = this;    
        }
        else if(IHinstance != null)
        {
            Destroy(this.gameObject);
        }

        // FPS lock
        Application.targetFrameRate = targetFPS;
    }

}
