using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitPicker : MonoBehaviour
{

    public TextMeshPro fruitText;
    public List<GameObject> fruits;

    // Start is called before the first frame update
    void Start()
    {
        disableAllFruits();
        fruits[0].SetActive(true);
        fruitText.text = "A - APPLE";
    }

    void disableAllFruits()
    {
        foreach (var item in fruits)
        {
            item.SetActive(false);
        }
    }

    public void pickFruit(int p)
    {
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
