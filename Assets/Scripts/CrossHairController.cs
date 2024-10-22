using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


interface Interactable
{
    public void Radio1_Demo_Play();
}
public class CrossHairController : MonoBehaviour
{
    public Transform interact;
    public float interactRange;
    public Image crosshar;
    private Color crossharColorNo = Color.white;
    private Color crossharColorYes = Color.green;

    // Update is called once per frame
    void Update()
    {
        Ray hit = new Ray(interact.position, interact.forward);
        crosshar.color = crossharColorNo;

        if (Physics.Raycast(hit, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.tag == "Radio")
            {
                crosshar.color = crossharColorYes;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent(out Interactable playRadio))//interact with radio
                    {
                        playRadio.Radio1_Demo_Play();
                    }
                }
            }
            
        }
    }
}
