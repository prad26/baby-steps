using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParameterManager : MonoBehaviour
{
    public GameObject homeParameterCanvas;
    public GameObject fruitPickerCanvas;
    public List<GameObject> models;

    public TextMeshPro fruitText;
    public List<GameObject> fruits;

    // Start is called before the first frame update
    void Start()
    {
        homeParameterCanvas.SetActive(true);        
        fruitPickerCanvas.SetActive(false);

        disableAllModels();
        models[0].SetActive(true);

        disableAllFruits();
        fruits[0].SetActive(true);
        fruitText.text = "A - APPLE";
    }

    public void welcomeButtonOnClick()
    {
        homeParameterCanvas.SetActive(false);
        fruitPickerCanvas.SetActive(true);
    }

    void disableAllFruits()
    {
        foreach (var item in fruits)
        {
            item.SetActive(false);
        }
    }

    void disableAllModels()
    {
        foreach (var item in models)
        {
            item.SetActive(false);
        }
    }

    public void pickFruit(int p)
    {
        disableAllModels();
        models[1].SetActive(true);

        disableAllFruits();
        fruits[p].SetActive(true);
        switch(p)
        {
            case 0:
                fruitText.text = "A - APPLE";
                break;

            case 1:
                fruitText.text = "B - BANANA";
                break;

            case 2:
                fruitText.text = "O - ORANGE";
                break;
            
            default:
                break;
        }
    }
}
