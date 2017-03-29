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
			SceneManager.UnloadSceneAsync (playSceneNum);
		}
		playSceneNum = 2;
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
			SceneManager.LoadScene (1, LoadSceneMode.Additive);
			GameManager.gameProgress = GameManager.GAMEPROGRESS.wait;
		}

	}

	public static Transform GetLevelSceneRoot()
	{
		GameObject[] newscene_root = SceneManager.GetSceneAt (SceneManager.sceneCount - 1).GetRootGameObjects ();
		return newscene_root [0].transform;
	}
}
