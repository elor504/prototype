using UnityEngine.UI;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public GameObject lockLVL2, lockLVL3, lockLVL4, lockLVL5, lockLVL6, lockLVL7, lockLVL8, lockLVL9;
    public Button buttonLVL2, buttonLVL3, buttonLVL4, buttonLVL5, buttonLVL6, buttonLVL7, buttonLVL8, buttonLVL9;
    public Toggle freePlayToggle;
    private bool isFreePlay;

    void OnEnable()
    {
        CheckLevelsStates();
        isFreePlay = false;
    }

    void CheckLevelsStates()
    {
        if (GameManager.completeLVL1 == true || GameManager.completeLVL2 == true || isFreePlay == true)
        {
            lockLVL2.SetActive(false);
            lockLVL2.GetComponent<Button>().interactable = false;
            buttonLVL2.interactable = true;
        }
        else
        {
            lockLVL2.SetActive(true);
            lockLVL2.GetComponent<Button>().interactable = true;
            buttonLVL2.interactable = false;
        }

        if (GameManager.completeLVL2 == true || GameManager.completeLVL3 == true || isFreePlay == true)
        {
            lockLVL3.SetActive(false);
            lockLVL3.GetComponent<Button>().interactable = false;
            buttonLVL3.interactable = true;
        }
        else
        {
            lockLVL3.SetActive(true);
            lockLVL3.GetComponent<Button>().interactable = true;
            buttonLVL3.interactable = false;
        }

        if (GameManager.completeLVL3 == true || GameManager.completeLVL4 == true || isFreePlay == true)
        {
            lockLVL4.SetActive(false);
            lockLVL4.GetComponent<Button>().interactable = false;
            buttonLVL4.interactable = true;
        }
        else
        {
            lockLVL4.SetActive(true);
            lockLVL4.GetComponent<Button>().interactable = true;
            buttonLVL4.interactable = false;
        }

        if (GameManager.completeLVL4 == true || GameManager.completeLVL5 == true || isFreePlay == true)
        {
            lockLVL5.SetActive(false);
            lockLVL5.GetComponent<Button>().interactable = false;
            buttonLVL5.interactable = true;
        }
        else
        {
            lockLVL5.SetActive(true);
            lockLVL5.GetComponent<Button>().interactable = true;
            buttonLVL5.interactable = false;
        }

        if (GameManager.completeLVL5 == true || GameManager.completeLVL6 == true || isFreePlay == true)
        {
            lockLVL6.SetActive(false);
            lockLVL6.GetComponent<Button>().interactable = false;
            buttonLVL6.interactable = true;
        }
        else
        {
            lockLVL6.SetActive(true);
            lockLVL6.GetComponent<Button>().interactable = true;
            buttonLVL6.interactable = false;
        }

        if (GameManager.completeLVL6 == true || GameManager.completeLVL7 == true || isFreePlay == true)
        {
            lockLVL7.SetActive(false);
            lockLVL7.GetComponent<Button>().interactable = false;
            buttonLVL7.interactable = true;
        }
        else
        {
            lockLVL7.SetActive(true);
            lockLVL7.GetComponent<Button>().interactable = true;
            buttonLVL7.interactable = false;
        }

        if (GameManager.completeLVL7 == true || GameManager.completeLVL8 == true || isFreePlay == true)
        {
            lockLVL8.SetActive(false);
            lockLVL8.GetComponent<Button>().interactable = false;
            buttonLVL8.interactable = true;
        }
        else
        {
            lockLVL8.SetActive(true);
            lockLVL8.GetComponent<Button>().interactable = true;
            buttonLVL8.interactable = false;
        }

        if (GameManager.completeLVL8 == true || GameManager.completeLVL9 == true || isFreePlay == true)
        {
            lockLVL9.SetActive(false);
            lockLVL9.GetComponent<Button>().interactable = false;
            buttonLVL9.interactable = true;
        }
        else
        {
            lockLVL9.SetActive(true);
            lockLVL9.GetComponent<Button>().interactable = true;
            buttonLVL9.interactable = false;
        }

        if (GameManager.completeLVL9 == true || isFreePlay == true)
        {
            lockLVL9.SetActive(false);
            lockLVL9.GetComponent<Button>().interactable = false;
            buttonLVL9.interactable = true;
        }
        else
        {
            lockLVL9.SetActive(true);
            lockLVL9.GetComponent<Button>().interactable = true;
            buttonLVL9.interactable = false;
        }
    }
    public void ChangeFreePlay()
    {
        if(freePlayToggle.isOn)
        {
            isFreePlay = true;
            CheckLevelsStates();
        }
        else
        {
            isFreePlay = false;
            CheckLevelsStates();
        }
    }
}
