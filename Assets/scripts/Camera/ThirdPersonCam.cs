using UnityEngine;
using System.Collections;

public class ThirdPersonCam : MonoBehaviour {
	
	float moveSpeed = 45f;
	float rot  = 219.1001f;
	float horizontalSpeed = 5.0f;
	float verticalSpeed = 2.0f;
	public Transform target = null;
	LaraStatePlayer anim = null; //if target is Lara
 	
	float camrot = 0.0f;
	Vector3 forward = Vector3.forward;
	//Vector3 position = Vector3.zero;
	float distance = 0.0f;
	float fieldofview = 0.0f;
	float lookup = 0.0f;
	bool  binit = false;
	
	float mouse_dx = 0;
	float mouse_dy = 0;
	

	
	// Use this for initialization
	void Start () 
	{
		Camera camera = GetComponent<Camera>();
		fieldofview = camera.fieldOfView;
		Mouse.m_OnMouseMove += OnMouseMove;
	}
	
	void OnMouseMove(float dx, float dy)
	{
		mouse_dx = dx;
		mouse_dy = dy;
	}
	// Update is called once per frame
	void Update () 
	{
	
    //transform.Rotate (v, h, 0);
	if(target == null) 
	{
		GameObject lara = GameObject.Find("Lara");
		if(lara!=null)	
		{
			target = lara.transform;	
		}
		else
		{
			Debug.Log("ThirdPersonCam: Target Not Found!");
			return;
		}
	}
					
	if(!binit)
	{
		transform.position = target.position + (target.position - transform.position).normalized * 2048;
		anim = target.GetComponent<LaraStatePlayer>();
		binit = true;
	}
	
	
	//Vector3 forward = target.forward;
	if(anim!=null && anim.OnAir)
    {
			//camrot += Input.GetAxis("Mouse X") * 10.0f;
			forward = Quaternion.Euler(0, camrot,0) * Vector3.forward;
			//camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, fieldofview * 1.2f, Time.deltaTime * 5.0f);
	}
	else
	{
			
			forward = target.forward;
			//position = new Vector3(target.position.x, target.position.y + 700, target.position.z);
			//distance = (position - transform.position).magnitude;
			camrot = transform.eulerAngles.y;
			//camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, fieldofview , Time.deltaTime * 10.0f);
			
	}
		
	RaycastHit hit = new RaycastHit();
	if(Physics.Raycast(target.position, -forward, out hit,1500))
	{
		distance = hit.distance;
	}
	else
	{
		distance = 1500;
	}
		
		
	if(anim!=null && anim.OnAir)
	{
		/*transform.position = position - forward * distance;
		forward = (target.position - transform.position).normalized;
		transform.forward = Vector3.Lerp(transform.forward ,forward,Time.deltaTime * 0.5f);*/
			
			transform.forward = Vector3.Lerp(transform.forward , target.forward,Time.deltaTime * 5.0f);
			Vector3 pos = target.position - forward * distance;
			transform.position = Vector3.Lerp(transform.position,new Vector3(pos.x,pos.y + 700, pos.z),Time.deltaTime * 2.0f);

		
			//Debug.Log(camrot);
	}
	else
	{
			lookup += mouse_dy * 0.01f;
			mouse_dy = 0;
			transform.forward = Vector3.Lerp(transform.forward , target.forward + Vector3.up * lookup,Time.deltaTime * 5.0f) ;
			Vector3 pos = target.position - forward * distance;
			transform.position = Vector3.Lerp(transform.position,new Vector3(pos.x,pos.y + 700, pos.z),Time.deltaTime * 2.0f);

	}
	}
}
