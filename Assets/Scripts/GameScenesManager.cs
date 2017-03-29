using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Load and remove scenes*/

public class GameScenesManager : MonoBehaviour 
{
	public static int maxSceneNum;
	public static int playSceneNum;

	public static void InitData()
	{
		maxSceneNum = 6;
		playSceneNum = 2;
		SceneManager.LoadScene (playSceneNum, LoadSceneMode.Additive);	// when game starts, load level 1
		playSceneNum++;
	}

	public static void ResetData()
	{
		if (playSceneNum > 2) 
		{
			SceneManager.UnloadSceneAsync (playSceneNum - 1);
		}
		playSceneNum = 2;
		SceneManager.LoadScene (playSceneNum, LoadSceneMode.Additive);	// when game starts, load level 1
		playSceneNum++;
	}

	public static void LoadNewScene()
	{
		SceneManager.UnloadSceneAsync (playSceneNum - 1);
		Debug.Log (playSceneNum);
		if (playSceneNum < maxSceneNum) 
		{
			SceneManager.LoadScene (playSceneNum, LoadSceneMode.Additive);
			playSceneNum++;
		}
		else
		{
			SceneManager.LoadScene (2, LoadSceneMode.Additive);
			GameManager.gameProgress = GameManager.GAMEPROGRESS.wait;
		}

	}

	public static Transform GetLevelSceneRoot()
	{
		if (GameManager.gameProgress == GameManager.GAMEPROGRESS.gaming) 
		{
			Debug.Log ("Gaming");
			GameObject[] newscene_root = SceneManager.GetSceneAt (SceneManager.sceneCount - 1).GetRootGameObjects ();
			return newscene_root [0].transform;
		}
	
		return null;
	}
}
