using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Control : MonoBehaviour {
	public GameObject hostage, terrorist1, terrorist3, terrorist4, terrorist5;
	private int saveCount;
	private uint timer;
	private GameObject hostage1;

	private Transform workspace;
	private Transform Base;

	private Touch oldTouch1; 
	private Touch oldTouch2;
	private Transform spawn;
	public int hostageKilled, hostageEscaped, terroristsKilled, points;

	private List<GameObject> people;

	private Vector3 originalScale;

	// Use this for initialization
	void Start () {		
		spawn = GameObject.FindGameObjectWithTag ("terrorist_spawn_point").transform;
		workspace = GameObject.Find ("Workspace").transform;
		timer = 1;
		GameObject newHostage = Instantiate (hostage,new Vector3((float)-6.5,2,13), Quaternion.Euler(0, 180, 0));
		newHostage.transform.SetParent (workspace);
		GameObject newTerrorist = Instantiate (terrorist1, new Vector3(2,2,0),Quaternion.identity);
		newTerrorist.transform.SetParent (workspace);

		people = new List<GameObject> ();
		people.Add (newHostage);
		people.Add (newTerrorist);
		saveCount = 0;
		hostageKilled = 0;
		hostageEscaped = 0;
		terroristsKilled = 0;
		points = 0;
		originalScale = workspace.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

		if (points < 0)
			points = 0;
		
		if (hostageEscaped == 3) {
			PlayerPrefs.SetInt ("points", points); 
			PlayerPrefs.SetInt ("tk", terroristsKilled);
			Application.LoadLevel ("level2");
		}


		if (hostageKilled == 3)
			gameObject.transform.tag = "over";


		if (this.transform.tag == "over") {
			PlayerPrefs.SetInt ("result", 1);
			PlayerPrefs.SetInt ("points", points);
			Application.LoadLevel ("end");
		}

		timer++;
		hostage1 = GameObject.Find ("hostage1(Clone)");
		if (hostage1 == null) {
			GameObject newHostage = GameObject.Instantiate (hostage, new Vector3 ((float)-6.5, 2, 13), Quaternion.Euler (0, 180, 0));
			newHostage.transform.localScale *= (workspace.transform.localScale.x / originalScale.x);
			newHostage.transform.SetParent (workspace);
		}

		if (timer % 300 == 0) {
			int counter = Random.Range (2, 5);
			GameObject terrorist = null;
			switch (counter) {
			case 2:
				terrorist = GameObject.Instantiate (terrorist1);
				break;
			case 3:
				terrorist = GameObject.Instantiate (terrorist3);
				break;
			case 4:
				terrorist = GameObject.Instantiate (terrorist4);
				break;
			case 5:
				terrorist = GameObject.Instantiate (terrorist5);
				break;
			}
			terrorist.transform.position = spawn.transform.position;
			terrorist.transform.SetParent (workspace);
			terrorist.transform.localScale *= (workspace.transform.localScale.x / originalScale.x);
			//terrorist.transform.SetParent (workspace);
			people.Add (terrorist);
//			int area = Random.Range (0, 1);
//			if (area == 0) {
//				terrorist.transform.position = new Vector3 (Random.Range (-9, -2), 2, Random.Range (3, 11));
//			} else {
			//terrorist.transform.position = new Vector3 (Random.Range (workspace.position.x+3f*workspace.localScale.x, workspace.position.x+11f*workspace.localScale.x), 2, Random.Range (workspace.position.z-15f*workspace.localScale.z, workspace.position.z + 2f*workspace.localScale.z));
//			}

		}

	
		

		if(Input.touchCount == 1){  
			Touch touch = Input.GetTouch (0);  
			Vector2 deltaPos = touch.deltaPosition;           
			workspace.Rotate(0.3f*Vector3.down  * deltaPos.x , Space.World);  
		} 

		if (Input.touchCount < 2)
			return;

		Touch newTouch1 = Input.GetTouch (0);  
		Touch newTouch2 = Input.GetTouch (1);  

		if( newTouch2.phase == TouchPhase.Began ){  
			oldTouch2 = newTouch2;  
			oldTouch1 = newTouch1;  
			return;  
		}  

		float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);  
		float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);  

		float offset = newDistance - oldDistance;  

		float scaleFactor = offset / 200f;

		Vector3 localScale = workspace.localScale;  
		Vector3 scale = new Vector3(localScale.x + scaleFactor,  
			localScale.y + scaleFactor,   
			localScale.z + scaleFactor); 
		
		if (scale.x > 0.7f && scale.y > 0.7f && scale.z > 0.7f) {  
			workspace.localScale = scale; 

			foreach (GameObject person in people) {
				person.transform.localPosition *= (scale.x / originalScale.x);
			}
		}  

		oldTouch1 = newTouch1;  
		oldTouch2 = newTouch2;

	}

	void OnGUI(){
		GUIStyle bb=new GUIStyle(); 
		bb.fontSize = 40;
		GUI.Box (new Rect (Screen.width * 1/ 18, Screen.height * 0.5f / 9, Screen.width *  1/ 6, Screen.height * 1/ 6), "Points: " + points,bb);
		GUI.Box (new Rect (Screen.width * 1/ 18, Screen.height * 1 / 9, Screen.width *  1/ 6, Screen.height * 1/ 6), "Terrorists killed: " + terroristsKilled,bb);
		GUI.Box (new Rect (Screen.width * 1 / 18, Screen.height * 1.5f / 9, Screen.width * 1 / 6, Screen.height * 1 / 6), "Hostage rescued: " + hostageEscaped, bb);
		GUI.Box (new Rect (Screen.width * 1 / 18, Screen.height * 2 / 9, Screen.width * 1 / 6, Screen.height * 1 / 6), "Hostage killed: " + hostageKilled, bb);
	}
}
