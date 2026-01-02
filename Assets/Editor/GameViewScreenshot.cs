using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameViewScreenshot
{
	[MenuItem("Tools/Capture Game View Screenshot")]
	static void TakeScreenshot()
	{
		if (!Application.isPlaying)
		{
			Debug.LogError("You must be in Play Mode.");
			return;
		}

		string sceneName = SceneManager.GetActiveScene().name;
		if (string.IsNullOrEmpty(sceneName))
			sceneName = "UntitledScene";

		string folderPath = Application.dataPath + "/Screenshots";
		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

		string path = Path.Combine(folderPath, sceneName + ".png");
		int counter = 1;
		while (File.Exists(path))
		{
			path = Path.Combine(folderPath, $"{sceneName}_{counter}.png");
			counter++;
		}

		// THIS captures the Game View, nothing else does
		ScreenCapture.CaptureScreenshot(path);

		Debug.Log("Game View screenshot saved to: " + path);
		AssetDatabase.Refresh();
	}
}
