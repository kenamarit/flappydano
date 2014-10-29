using UnityEngine;
using System.Collections;

// For storing high score on server
using Parse;

public class Bird : MonoBehaviour {

	float speed = 40;
	float maxSpeed = 1000;
	float gravity = 140;
	float jump = 40;

	int score = 0;
	int hiscore= 0;

	bool canFly = false;
	public bool isDead = false;
	GameObject tubeGenerator;

	Vector3 startPos;
	GameObject introText;
	GameObject scoreText;
	GameObject hiScoreText;

	ParseObject parseHiScore;

	void Start() {
		tubeGenerator = GameObject.Find("TubeGenerator");
		introText = GameObject.Find("IntroText");
		scoreText = GameObject.Find("ScoreText");
		hiScoreText = GameObject.Find("HiScoreText");
		startPos = transform.position;

	 	hiScoreText.guiText.text = "HI SCORE: ";

	 	// FOR PARSE:
	 	///////////////////////////////////////////////////
		ParseQuery<ParseObject> query = ParseObject.GetQuery("hiScore");

		query.GetAsync("WEM1OfdTAa").ContinueWith(t =>
		{
		    parseHiScore = t.Result;
		    hiscore = parseHiScore.Get<int>("hiScore");
		});

		///////////////////////////////////////////////////

	}



	void Update() {
		if( !canFly ) {

			if( hiscore != 0)
				hiScoreText.guiText.text = "HI SCORE: " + hiscore;

			if( Input.GetKeyDown( "space" )) {
				canFly = true;
				tubeGenerator.GetComponent<TubeGenerator>().StartGame();
				introText.active = false;
			}
		} else if( canFly && !isDead ) {

			speed -= gravity * Time.deltaTime;
			if (speed >= maxSpeed) {
				speed = maxSpeed;
			}

			if(Input.GetKeyDown("space")) {
				speed = jump;
			}

			Vector3 curPos = transform.position;
			curPos.y += speed * Time.deltaTime;
			transform.position = curPos;

		} else if( isDead ) {
			speed -= gravity * Time.deltaTime;

			Vector3 curPos = transform.position;
			curPos.y += speed * Time.deltaTime;
			transform.position = curPos;
		}
	}

	void OnTriggerEnter( Collider c ) {

		if( c.transform.name == "ScoreTrigger") {

			score += 1;
			//Debug.Log("hit. score= " + score);
		}

		if( c.transform.name == "TubeTop" || c.transform.name == "TubeBottom" || c.transform.name == "Floor" ) {
			
			//Debug.Log("hiscore= " + hiscore);
			// FOR PARSE:
	 		///////////////////////////////////////////////////
			if( score > hiscore ) {
				hiscore = score;

			    parseHiScore.SaveAsync().ContinueWith(t => {
			    	parseHiScore["hiScore"] = hiscore; 
			    	parseHiScore.SaveAsync();
				});

				hiScoreText.guiText.text = "HI SCORE: " + hiscore; // NOT PARSE
			}
			///////////////////////////////////////////////////

			scoreText.guiText.text = "SCORE: " + score; 

			isDead = true;
			StopAllCoroutines();
			tubeGenerator.GetComponent<TubeGenerator>().StopAllCoroutines();

			StartCoroutine( ResetGame() );
		}
	}	



	IEnumerator ResetGame() {
		yield return new WaitForSeconds( 1.5f );
		tubeGenerator.GetComponent<TubeGenerator>().RemoveAllTubes();
		speed = 40;
		maxSpeed = 1000;
		gravity = 140;
		jump = 40;
		score = 0;

		canFly = false;
		isDead = false;

		introText.active = true;

		transform.position = startPos;
	}

}