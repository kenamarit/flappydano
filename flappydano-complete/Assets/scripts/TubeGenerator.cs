using UnityEngine;
using System.Collections;

public class TubeGenerator : MonoBehaviour {

	public GameObject tube;
	GameObject tubes;

	void Start() {
		tubes = GameObject.Find("Tubes");
	}

	public void StartGame() {
		StartCoroutine( CreateNewTube() );
	}

	IEnumerator CreateNewTube() {
		
		GameObject newTube = (GameObject)Instantiate(tube);
		Vector3 startPos = newTube.transform.position;
		startPos.x = 95;
		startPos.y = Random.Range( -57.0f, -17.0f );
		newTube.transform.position = startPos;
		newTube.transform.parent = tubes.transform;

		yield return new WaitForSeconds(2.5f);
		StartCoroutine( CreateNewTube() );
	}

	public void RemoveAllTubes() {
		foreach( Transform t in tubes.transform ) {
			Destroy( t.gameObject );
		}
	}
}