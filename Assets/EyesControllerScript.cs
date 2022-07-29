using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesControllerScript : MonoBehaviour
{
    [SerializeField] Transform pupilTranform;
    [SerializeField] Transform eyeTranform;

    [SerializeField] Transform ghosyBoyTranform;
    public float eyeOffset;
    [SerializeField] Animator animator;
    public bool isPupilFollow;

	private void Awake()
	{
        ghosyBoyTranform = GameObject.Find("Ghostboy_onlyidle").transform;

    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPupilFollow)
        {
            Vector2 dir = (ghosyBoyTranform.position - this.transform.position).normalized;
            animator.SetFloat("X", dir.x);
            animator.SetFloat("Y", dir.y);
        }
        else
        {
            eyeTranform.right = ghosyBoyTranform.position - eyeTranform.position;
        }
        //Debug.Log(eyeTranform.eulerAngles.z);

        //eyeTranform.eulerAngles = new Vector3(0,0,Mathf.Clamp(eyeTranform.eulerAngles.z, 330, 30));
        

    }

}
