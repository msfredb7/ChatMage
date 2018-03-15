using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCC.UI;

public class RegionComplete : WindowAnimation {

	public const string SCENENAME = "RegionComplete";
	private bool isQuitting = false;

	public void CloseMenu()
	{
		if (isQuitting)
			return;
		Exit();
	}




	public void KeepPlaying()
	{
		Debug.Log("Keep playing");
	}
	public void EndlessMode()
	{
		Debug.Log("Endless Mode");
	}




	private void Exit()
	{
		if (isQuitting) return;

		isQuitting = true;

		if (this != null)
		{
			Close(
				delegate ()
				{
					Scenes.UnloadAsync(SCENENAME);
					isQuitting = false;
				}
			);
		}
		else
		{
			Scenes.UnloadAsync(SCENENAME);
			isQuitting = false;
		}
	}

	public static void OpenIfClosed()
	{
		if (Game.Instance != null)
		{
			Debug.LogWarning("Cannot open RegionComplete if the game is running.");
			return;
		}

		if (Scenes.IsActive(SCENENAME))
			return;
		Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
	}

	/*
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			CloseMenu();
		}
	}
	*/
}
