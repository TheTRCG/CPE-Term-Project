using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;



public class Measurer : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the button location.")]
    GameObject m_PlacedPrefab;

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at location being looked at.")]
    GameObject l_PlacedPrefab;

    public GameObject placedPrefab_look
    {
        get { return l_PlacedPrefab; }
        set { l_PlacedPrefab = value; }
    }

    [SerializeField]
    LineRenderer linerenderer;

    [SerializeField]
    LineRenderer linerenderermeasure;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    public GameObject spawnedObject_look { get; private set; }

    [SerializeField] 
    private TMP_Text UserMessage;

    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    Vector2 ScreenCenter;
    
    List<GameObject> Points = new List<GameObject>();

    void Start()
    {
        ScreenCenter=new Vector2(Screen.width/2f, Screen.height/2f);

    }

    // Update is called once per frame
    void Update()
    {
        if (m_RaycastManager.Raycast(ScreenCenter, m_Hits)){
            var hitPose = m_Hits[0].pose;
            if(spawnedObject_look==null){
                spawnedObject_look = Instantiate(l_PlacedPrefab, hitPose.position, hitPose.rotation);
            }else{
                spawnedObject_look.transform.position = hitPose.position;
                if(Points.Count!=0){
                    linerenderermeasure.SetPosition(0, hitPose.position);
                    linerenderermeasure.SetPosition(1, linerenderer.GetPosition(linerenderer.positionCount-1));
                    float distance = Vector3.Distance(hitPose.position, linerenderer.GetPosition(linerenderer.positionCount-1));
                    UserMessage.text=distance.ToString("#.00")+"m";
                }else{
                    linerenderermeasure.SetPosition(0, new Vector3(0,0,0));
                    linerenderermeasure.SetPosition(1, new Vector3(0,0,0));
                }
            }
        }else{
            UserMessage.text="Move phone around slowly";
        }
    }

    public void ButtonAdd(){
        if (m_RaycastManager.Raycast(ScreenCenter, m_Hits)){
            var hitPose = m_Hits[0].pose;
            Points.Add(Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation));
            linerenderer.positionCount ++;
            linerenderer.SetPosition(linerenderer.positionCount-1, hitPose.position);
        }
    }
    
    public void ButtonUndo(){
        Destroy(Points[Points.Count - 1]);
        Points.RemoveAt(Points.Count - 1);
        linerenderer.positionCount --;
    }
}

