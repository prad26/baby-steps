using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterManager : MonoBehaviour
{

    public GameObject homeParameterCanvas;
    public GameObject fruitPickerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        homeParameterCanvas.SetActive(true);        
        fruitPickerCanvas.SetActive(false);
    }

    public void welcomeButtonOnClick()
    {
        homeParameterCanvas.SetActive(false);
        fruitPickerCanvas.SetActive(true);
    }
}
