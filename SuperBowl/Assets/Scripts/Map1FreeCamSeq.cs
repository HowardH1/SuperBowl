using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class Map1FreeCamSeq : MonoBehaviour
{
    public GameObject overheadCam;
    public GameObject mapCam;
    public GameObject mainCam;
    public GameObject PlayerUI;
    public Flowchart animFlow = new Flowchart();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CamSequence());
        
    }

    private IEnumerator CamSequence()
    {
        animFlow.SetBooleanVariable("isAnimFinished", false);
        overheadCam.SetActive(true);
        mainCam.SetActive(false);

        yield return new WaitForSeconds(4);

        mapCam.SetActive(true);
        overheadCam.SetActive(false);

        yield return new WaitForSeconds(4);

        mainCam.SetActive(true);
        mapCam.SetActive(false);
        animFlow.SetBooleanVariable("isAnimFinished", true);
    }

    public void setUIActive()
    {
        PlayerUI.SetActive(true);
    }

    public void setUIInactive()
    {
        PlayerUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
