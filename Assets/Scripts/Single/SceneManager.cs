using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SceneManager : ManagerClassBase<SceneManager>
{

	// 로드할 씬
	public string nextSceneName { get; private set; } = "TitleScene";

	public override void InitializeManagerClass()
	{
	}

	public void LoadScene(string nextScene)
	{
		nextSceneName = nextScene;
		GameManager.GetGameManager().SceneLoadStarted();
		UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
	}

	public override void OnSceneChanged(string newSceneName)
	{
		LevelInstance.ClearLevelInstance();
	}






}
