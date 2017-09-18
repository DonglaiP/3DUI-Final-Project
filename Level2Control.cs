using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Control : MonoBehaviour {
	public GameObject terrorist1, terrorist2, terrorist3, terrorist4, terrorist5;
	private uint timer;
	private int bomberCount;
	public int points;
	public int terroristKilled, bombDefused;
	private Touch oldTouch1; 
	private Touch oldTouch2;
	private Transform workspace, spot1, spot2, spot3, spot4;

	private List<GameObject> people;

	private Vector3 originalScale;
	// Use this for initialization
	void Start () {
		timer = 0;
		bomberCount = 0;
		terroristKilled = 0;
		bombDefused = 0;
		points = PlayerPrefs.GetInt("points");
		terroristKilled = PlayerPrefs.GetInt("tk");
		spot1 = GameObject.Find ("spot1").transform;
		spot2 = GameObject.Find ("spot2").transform;
		spot3 = GameObject.Find ("spot3").transform;
		spot4 = GameObject.Find ("spot4").transform;

		workspace = GameObject.Find ("Workspace").transform;
		originalScale = workspace.transform.localScale;

		people = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (points < 0)
			points = 0;

		if (bombDefused == 3)
		{
			PlayerPrefs.SetInt("result", 0);
			PlayerPrefs.SetInt("points", points);
			Application.LoadLevel("end");
		}

		if (this.transform.tag == "over") {
			PlayerPrefs.SetInt("result", 1);
			PlayerPrefs.SetInt ("points", points);
			Application.LoadLevel("end");
		}



		timer++;
		if (timer % 150 == 0) {
			int counter = Random.Range (2, 5);
			GameObject newTerrorist = null;
			switch (counter) {
			case 2:
				newTerrorist = (GameObject)GameObject.Instantiate (terrorist2);
				newTerrorist.transform.position = spot1.position;
				break;
			case 3:
				newTerrorist = (GameObject)GameObject.Instantiate (terrorist3);
				newTerrorist.transform.position = spot2.position;
				break;
			case 4:
				newTerrorist = (GameObject)GameObject.Instantiate (terrorist4);
				newTerrorist.transform.position = spot3.position;
				break;
			case 5:
				newTerrorist = (GameObject)GameObject.Instantiate (terrorist5);
				newTerrorist.transform.position = spot4.position;
				break;
			}
			newTerrorist.transform.localScale *= (workspace.transform.localScale.x/originalScale.x);
			newTerrorist.transform.SetParent(workspace);
			people.Add (newTerrorist);
		}

		if (timer % 600 == 0) {
			GameObject newBomber = (GameObject)GameObject.Instantiate (terrorist1);
			newBomber.transform.SetParent(workspace);
			int ran = Random.Range(1, 4);
			switch (ran) {
			case 1:
				newBomber.transform.position = spot1.position;
				break;
			case 2:
				newBomber.transform.position = spot2.position;
				break;
			case 3:
				newBomber.transform.position = spot3.position;
				break;
			case 4:
				newBomber.transform.position = spot4.position;
				break;
			}
			newBomber.transform.localScale *= (workspace.transform.localScale.x/originalScale.x);
			bomberCount++;
			newBomber.GetComponent<put_bomb>().count = bomberCount;
			people.Add (newBomber);
		}

		if(Input.touchCount == 1){  
			Touch touch = Input.GetTouch (0);  
			Vector2 deltaPos = touch.deltaPosition;           
			workspace.Rotate(Vector3.down  * deltaPos.x , Space.World);  
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

		float scaleFactor = offset / 50f;  

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
		GUI.Box (new Rect (Screen.width * 1/ 18, Screen.height * 1 / 9, Screen.width *  1/ 6, Screen.height * 1/ 6), "Terrorists killed: " + terroristKilled,bb);
		GUI.Box (new Rect (Screen.width * 1 / 18, Screen.height * 1.5f / 9, Screen.width * 1 / 6, Screen.height * 1 / 6), "Bomb defused: " + bombDefused, bb);
	}

}
