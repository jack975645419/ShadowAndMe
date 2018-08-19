using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLogic : MonoBehaviour {

    public Rigidbody2D GeneratorRigidbody2D;
    public Transform GeneratorPoint;
    public GameObject HookClass;
    public GameObject hookGameObject = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return))
        {
            Vector2 suddenVelocity = GeneratorRigidbody2D.GetPointVelocity( GeneratorPoint.position );

            Debug.Log("Throw" + suddenVelocity);

            hookGameObject = Instantiate(HookClass, GeneratorPoint.position, Quaternion.identity);
            Rigidbody2D hookRigid2d = hookGameObject.GetComponent<Rigidbody2D>();

            HookComp hookHook = hookGameObject.GetComponent<HookComp>();
            hookRigid2d.velocity = suddenVelocity * hookHook.HookInitialVelocityMultiplication;
            
            hookHook.Generator = GeneratorPoint;
            hookHook.PuppetBody = this.gameObject.transform;
        }

        
    }
}
