using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParameterManager : MonoBehaviour
{
    public GameObject homeParameterCanvas;
    //public GameObject fruitPickerCanvas;
    public List<GameObject> models;
    public TextMeshPro fruitText;
    public List<GameObject> fruits;
    public Camera arCamera = default;
    public TextMeshPro storeText;
    public TextMeshPro testText;
    public GameObject basketModel;
    public List<GameObject> basketFruits;
    public List<Material> basketFruitMaterials;
 
    private Vector2 touchPosition = default;
    private Material material;
    int appleCounter = 0, bananaCounter = 0, orangeCounter = 0, milkCounter = 0, p = 0;
    //List<int> totalFruits = new List<int>{2,3,4,2};

    // Start is called before the first frame update
    void Start()
    {
        homeParameterCanvas.SetActive(true);        
        //fruitPickerCanvas.SetActive(false);

        disableAllModels();
        models[0].SetActive(true);

        disableAllFruits();

        storeText.text = "";
        foreach (var item in basketFruits)
        {
            item.SetActive(false);
        }

        foreach (var item in basketFruitMaterials)
        {
            item.SetFloat("Vector1_FEFF47F1", 0.0f);
        }
    }

    public void welcomeButtonOnClick()
    {
        homeParameterCanvas.SetActive(false);
        //fruitPickerCanvas.SetActive(true);
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

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    testText.text = hitObject.transform.name;
                    fruitChecker(hitObject);
                }
            }
        }
    }

    void fruitChecker(RaycastHit hitObject)
    {
        MeshRenderer mesh = hitObject.transform.GetComponent<MeshRenderer>();
        material = mesh.material;
        
        switch(hitObject.transform.name)
        {
            case "Apple":
                appleCounter++;
                if(appleCounter > 2)
                {
                    shakeME();
                }
                else
                {
                    StartCoroutine(DissolveAnim(hitObject.transform.gameObject));
                    setStoreText();
                }
                break;

            case "Banana":
                bananaCounter++;
                if(bananaCounter > 4)
                {
                    shakeME();
                }
                else
                {
                    StartCoroutine(DissolveAnim(hitObject.transform.gameObject));
                    setStoreText();
                }
                break;
            
            case "Orange":
                orangeCounter++;
                if(orangeCounter > 3)
                {
                    shakeME();
                }
                else
                {
                    StartCoroutine(DissolveAnim(hitObject.transform.gameObject));
                    setStoreText();
                }
                break;
            
            case "milk":
                milkCounter++;
                if(milkCounter > 2)
                {
                    shakeME();
                }
                else
                {
                    StartCoroutine(DissolveAnim(hitObject.transform.gameObject));
                    setStoreText();
                }
                break;

            case "Skip":
                disableIngreScene();
                break;
            
            case "Next":
                if( p == 3 )
                    p = 0;
                pickFruit(p);
                p++;
                break;

            case "basket":
                if( p == 4 )
                    break;
                sendToBlender(p);
                p++;
                break;

            default:
                break;
        }
    }

    void shakeME()
    {
        material.SetInt("Boolean_33A9C5F7",1);
        StartCoroutine(WaitFor());
        material.SetInt("Boolean_33A9C5F7",0);
    }

    void sendToBlender(int p)
    {
        material = basketFruitMaterials[p];
        StartCoroutine(DissolveAnim());

        if( p == 3)
            startBlender();
    }

    void startBlender()
    {

    }

    void setStoreText()
    {
        int apple = 2 - appleCounter;
        int orange = 3 - orangeCounter;
        int banana = 4 - bananaCounter;
        int milk = 2 - milkCounter;

        if(apple < 0)
            apple = 0;
        if(orange < 0)
            orange = 0;
        if(banana < 0)
            banana = 0;
        if(milk < 0)
            milk = 0;

        storeText.text = "PLEASE PICK INGREDIENTS: \nAPPLE: " + apple + "         ORANGE: " + orange + " \nBANANA: " + banana + "     MILK: "+ milk;

        if( apple == 0 && orange == 0 && banana == 0 && milk == 0)
            blenderReady();
    }

    void blenderReady()
    {
        basketModel.SetActive(true);
        storeText.text = "";

        foreach (var item in basketFruits)
        {
            item.SetActive(true);
        }

        p = 0;

    }

    void disableIngreScene()
    {
        disableAllModels();
        //fruitPickerCanvas.SetActive(false);

        models[2].SetActive(true);
        setStoreText();
    }

    IEnumerator DissolveAnim(GameObject hitObject = null) 
    {
        material.SetFloat("Vector1_FEFF47F1", 0.0f);
        for(float t = 0; t <= 1; t += Time.deltaTime)
        {
            yield return null; // wait 1 frame

            // here is where the weird generated property name goes
            material.SetFloat("Vector1_FEFF47F1", t);
        }
        material.SetFloat("Vector1_FEFF47F1", 1.0f);

        if(hitObject != null)
            hitObject.SetActive(false);
    }

    IEnumerator WaitFor(int sec = 5)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(sec);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}