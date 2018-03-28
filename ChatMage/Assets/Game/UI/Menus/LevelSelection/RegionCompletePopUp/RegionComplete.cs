using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCC.UI;

public class RegionComplete : WindowAnimation {

	public const string SCENENAME = "RegionComplete";
	private bool isQuitting = false;

    public SceneInfo endlessStageSelectionScene;

	public void CloseMenu()
	{
		if (isQuitting)
			return;
		Exit();
	}

	public void KeepPlaying()
	{
        Exit();
	}
	public void EndlessMode()
	{
        DefaultAudioSources.StopMusicFaded(1);
        LoadingScreen.TransitionTo(endlessStageSelectionScene.SceneName, null);
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
}
