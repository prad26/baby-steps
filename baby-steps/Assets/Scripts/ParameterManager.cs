using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

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
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    public List<Material> blenderMaterials;
    public GameObject smoothieModel;
    public Material smoothieMaterial;
    public List<PlayableDirector> basketFruitsTimelines;
    public PlayableDirector smoothieTimeline;
    public Window_Confetti confettiCelebrations;
    public Material basketMaterial;
 
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

        setBlenderVibrate(0);
        smoothieModel.SetActive(false);
        confettiCelebrations.checker = 0;
    }

    public void welcomeButtonOnClick()
    {
        homeParameterCanvas.SetActive(false);
        //fruitPickerCanvas.SetActive(true);
        playMe(audioClips[0]);
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
                playMe(audioClips[1]);
                break;

            case 1:
                fruitText.text = "B - BANANA";
                playMe(audioClips[3]);
                break;

            case 2:
                fruitText.text = "O - ORANGE";
                playMe(audioClips[2]);
                break;
            
            case 3:
                fruitText.text = "M - MILK";
                playMe(audioClips[4]);
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
                if( p == 4 )
                    p = 0;
                pickFruit(p);
                p++;
                break;

            case "basket":
                if( p == 4 )
                    break;
                storeText.text = "TAP ON THE BASKET";
                sendToBlender(p);
                p++;
                break;

            case "blender":
                blenderReady();
                break;

            default:
                break;
        }
    }

    public void setBlenderVibrate(int v)
    {
        foreach (var item in blenderMaterials)
        {
            item.SetInt("Boolean_33DD99D2",v);
        }


    }

    void blenderReady()
    {
        playMe(audioClips[12]);

        foreach (var item in blenderMaterials)
        {
            item.SetInt("Boolean_E96899F",1);
        }
        
        foreach (var item in basketFruits)
        {
            item.SetActive(false);
        }
        setBlenderVibrate(1);
        foreach (var item in blenderMaterials)
            {
                item.SetInt("Boolean_E96899F",0);
            }
        storeText.text = "PREPARING YOUR SMOOTHIE!!!";
        StartCoroutine(WaitForBlender(6));

    }

    void smoothieReady()
    {
        storeText.text = "YOUR SMOOTHIE IS READY!! \nCONGRATULATIONS";
        playMe(audioClips[13]);
        basketModel.SetActive(false);
        smoothieModel.SetActive(true);

        smoothieTimeline.Play();

        confettiCelebrations.checker = 1;
        //material = smoothieMaterial;
        //StartCoroutine(SmoothieME());

        // enable main smooothi bside belnder with dissolve
    }

    void shakeME()
    {
        material.SetInt("Boolean_33A9C5F7",1);
        StartCoroutine(ShakeThatFruit());
    }

    void sendToBlender(int p)
    {

        basketFruitsTimelines[p].Play();
        //material = basketFruitMaterials[p];
        //StartCoroutine(DissolveAnim());

        if( p == 3)
        {
            playMe(audioClips[11]);
            storeText.text = "TAP ON THE BLENDER";
            basketMaterial.SetInt("Boolean_E96899F",0);
            //basketModel.SetActive(false);
            foreach (var item in blenderMaterials)
            {
                item.SetInt("Boolean_E96899F",1);
            }
        }
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
            fruitsReady();
    }

    bool calledme = false;

    void fruitsReady()
    {

        basketModel.SetActive(true);
        storeText.text = "TAP THE BASKET TO ADD FRUITS TO BLENDER";

        if( calledme == false)
        {
            //basketMaterial.SetInt("Boolean_E96899F",1);
            calledme = true;
        }

        foreach (var item in basketFruits)
        {
            item.SetActive(true);
        }

        playMe(audioClips[8]);

        p = 0;

    }

    void disableIngreScene()
    {
        disableAllModels();
        //fruitPickerCanvas.SetActive(false);

        models[2].SetActive(true);
        setStoreText();

        playMe(audioClips[5]);
    }

    void playMe(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    IEnumerator SmoothieME() 
    {
        material.SetFloat("Vector1_FEFF47F1", 1.0f);
        for(float t = 1; t >= 0; t -= Time.deltaTime)
        {
            yield return null; // wait 1 frame

            // here is where the weird generated property name goes
            material.SetFloat("Vector1_FEFF47F1", t);
        }
        material.SetFloat("Vector1_FEFF47F1", 0.0f);
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

    IEnumerator WaitForBlender(int sec = 5)
    {
        yield return new WaitForSeconds(sec);
        setBlenderVibrate(0);
        smoothieReady();
    }

    IEnumerator ShakeThatFruit(int sec = 2)
    {
        yield return new WaitForSeconds(sec);
        material.SetInt("Boolean_33A9C5F7",0);
    }
}