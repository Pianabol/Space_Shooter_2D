
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerControls : MonoBehaviour
{   
    private Camera mainCam; // // // reference to the main camera
    private Vector3 offset;
    private float maxLeft;
    private float maxRight;
    private float maxUp;
    private float maxDown; 


    [SerializeField] private InputActionReference moveActionToUse; 
    [SerializeField] private float speed;
    
    void Start()
    {
        mainCam = Camera.main;

       /* // --- DİNAMİK BAŞLANGIÇ ÇIPASI (ANCHOR) KODU ---
        // Z derinliğini kameranın Z pozisyonunun mutlak değeri olarak alıyoruz
        float zDepth = Mathf.Abs(mainCam.transform.position.z);
        
        // Ekranın X ekseninde %50'si (tam ortası) ve Y ekseninde %15'i (alt kısmın biraz üstü)
        Vector3 anchorPosition = mainCam.ViewportToWorldPoint(new Vector3(0.1f, 0.05f, zDepth));
        
        // Gemiyi oyun başlar başlamaz bu güvenli başlangıç noktasına yerleştiriyoruz
        transform.position = new Vector3(anchorPosition.x, anchorPosition.y, 0f);
        // ---------------------------------------------- */
        
        StartCoroutine(SetBoundaries());
        StartCoroutine(SetPlane());

    }
     

    void Update()
    { 

       // Vector2 moveDirection= moveActionToUse.action.ReadValue<Vector2>();
        //transform.Translate(moveDirection*speed*Time.deltaTime);


        if(Touch.activeTouches.Count>0)
        {   
            if(Touch.activeTouches[0].finger.index==0)
            {
                Touch myTouch = Touch.activeTouches[0];
                Vector3 touchPos=myTouch.screenPosition;
#if UNITY_EDITOR
                if(touchPos.x==Mathf.Infinity)
                    return;
#endif
                touchPos=mainCam.ScreenToWorldPoint(touchPos);

                if(Touch.activeTouches[0].phase==TouchPhase.Began)
                {
                    offset=touchPos-transform.position;
                }
                if(Touch.activeTouches[0].phase==TouchPhase.Moved)
                {
                    transform.position=new Vector3(touchPos.x-offset.x, touchPos.y-offset.y,0);
                }
                if(Touch.activeTouches[0].phase==TouchPhase.Stationary)
                {
                    transform.position=new Vector3(touchPos.x-offset.x, touchPos.y-offset.y,0);
                }  
            }
            transform.position=new Vector3(Mathf.Clamp(transform.position.x,maxLeft,maxRight), Mathf.Clamp(transform.position.y,maxDown,maxUp),0);

            
        } 
        transform.position=new Vector3(Mathf.Clamp(transform.position.x,maxLeft,maxRight), Mathf.Clamp(transform.position.y,maxDown,maxUp),0);
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private IEnumerator SetBoundaries()
    {
        //yield return new WaitForSeconds(0.4f);

        maxLeft= mainCam.ViewportToWorldPoint(new Vector2(0.1f,0)).x;
        maxRight= mainCam.ViewportToWorldPoint(new Vector2(0.9f,0)).x;

        maxDown=mainCam.ViewportToWorldPoint(new Vector2(0,0.06f)).y;
        maxUp=mainCam.ViewportToWorldPoint(new Vector2(0,0.94f)).y;

        yield break;
    /*
    burada olmadı direkt başka bi fonksiyon yazacağım.
        float zDepth = Mathf.Abs(mainCam.transform.position.z);
        
        // Ekranın X ekseninde %50'si (tam ortası) ve Y ekseninde %15'i (alt kısmın biraz üstü)
        Vector3 anchorPosition = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.15f, zDepth));
        
        // Gemiyi oyun başlar başlamaz bu güvenli başlangıç noktasına yerleştiriyoruz
        transform.position = new Vector3(anchorPosition.x, anchorPosition.y, 0f);
    */
    }

    private IEnumerator SetPlane() 
    { 
        //yield return new WaitForSeconds(0.01f);
        // --- 0 FRAME GECİKME (ANINDA ÇALIŞIR) ---
        float zDepth = Mathf.Abs(mainCam.transform.position.z);
        Vector3 anchorPosition = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, zDepth));
        
        transform.position = new Vector3(anchorPosition.x, anchorPosition.y, 0f);
        
        yield break;
      
    }
}


 