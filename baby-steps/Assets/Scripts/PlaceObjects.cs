using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjects : MonoBehaviour
{
    [SerializeField]
    bool m_SnapToMesh;

    public bool snapToMesh
    {
        get => m_SnapToMesh;
        set => m_SnapToMesh = value;
    }
    
    public GameObject m_ReticlePrefab;

    [SerializeField]
    bool m_DistanceScale;

    public bool distanceScale
    {
        get => m_DistanceScale;
        set => m_DistanceScale = value;
    }

    [SerializeField]
    Transform m_CameraTransform;

    public Transform cameraTransform
    {
        get => m_CameraTransform;
        set => m_CameraTransform = value;
    }

    CenterScreenHelper m_CenterScreen;
    TrackableType m_RaycastMask;
    float m_CurrentDistance;
    float m_CurrentNormalizedDistance;

    const float k_MinScaleDistance = 0.0f;
    const float k_MaxScaleDistance = 1.0f;
    const float k_ScaleMod = 0.75f;

    //[SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    public GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
/*    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }
*/
    public List<GameObject> zoomedInModels;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    private int objectSpawned = 0;

    private Pose hitPose;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    [SerializeField]
    bool m_CanReposition = true;

    public bool canReposition
    {
        get => m_CanReposition;
        set => m_CanReposition = value;
    }

    public bool disableFeaturePoints;
    public bool disablePlaneRendering;

    private ARPointCloudManager pointCloudManager;
    private ARPlaneManager planeManager;

    void disableTrackables()
    {
        if (disableFeaturePoints)
        {
            pointCloudManager.SetTrackablesActive(false);
            pointCloudManager.enabled = false;
        }

        if (disablePlaneRendering)
        {
            planeManager.SetTrackablesActive(false);
            planeManager.enabled = false;
        }
    }

    void Start()
    {
        m_CenterScreen = CenterScreenHelper.Instance;
        if (m_SnapToMesh)
        {
            m_RaycastMask = TrackableType.PlaneEstimated;
        }
        else
        {
            m_RaycastMask = TrackableType.PlaneWithinPolygon;
        }
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
        planeManager = GetComponent<ARPlaneManager>();
        m_ReticlePrefab.SetActive(false);
        m_PlacedPrefab.SetActive(false);
    }

    void Update()
    {
        if (m_RaycastManager.Raycast(m_CenterScreen.GetCenterScreen(), s_Hits, m_RaycastMask))
        {
            hitPose = s_Hits[0].pose;
            
            if(objectSpawned!=1)
            {
                
                m_ReticlePrefab.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                m_ReticlePrefab.SetActive(true);

                
                if (m_DistanceScale)
                {
                    m_CurrentDistance = Vector3.Distance(m_ReticlePrefab.transform.position, m_CameraTransform.position);
                    m_CurrentNormalizedDistance = ((Mathf.Abs(m_CurrentDistance - k_MinScaleDistance)) / (k_MaxScaleDistance - k_MinScaleDistance))+k_ScaleMod;
                    m_ReticlePrefab.transform.localScale = new Vector3(m_CurrentNormalizedDistance, m_CurrentNormalizedDistance, m_CurrentNormalizedDistance);
                }
            }
            
            if (Input.touchCount>0 && objectSpawned!=1)
            {
                objectSpawned = 1;

                m_PlacedPrefab.SetActive(true);
                m_PlacedPrefab.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                //m_PlacedPrefab.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);

                foreach(var item in zoomedInModels)
                {
                    item.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                    //item.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                }

                m_ReticlePrefab.SetActive(false);

                disableTrackables();
            }
            else if (m_CanReposition && Input.touchCount>0)
            {
               Touch touch = Input.GetTouch(0);
               if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
               {
                   hitPose = s_Hits[0].pose;
                   m_PlacedPrefab.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
               }
            }
            
            if (onPlacedObject != null)
            {
                onPlacedObject();
            }
        }
    } 
}
