using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBaseState : MonoBehaviour
{
   
    protected Camera mainCam; // // // reference to the main camera
    protected float maxLeft;
    protected float maxRight;
    protected float maxUp;
    protected float maxDown; 
    protected BossController bossController;
    private void Awake()
    {
        bossController = GetComponent<BossController>();
        mainCam = Camera.main;
        

    }
    protected virtual void Start()
    {
        maxLeft= mainCam.ViewportToWorldPoint(new Vector2(0.12f,0)).x;
        maxRight= mainCam.ViewportToWorldPoint(new Vector2(0.88f,0)).x;

        maxDown=mainCam.ViewportToWorldPoint(new Vector2(0,0.4f)).y;
        maxUp=mainCam.ViewportToWorldPoint(new Vector2(0,0.7f)).y;       
    }

    public virtual void RunState()
    {
        
    }
    public virtual void StopState()
    {
        StopAllCoroutines();
    }
}
