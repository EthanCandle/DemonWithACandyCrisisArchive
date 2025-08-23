using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneViewScreenshot
{
    [MenuItem("Tools/Take Clean Scene View Screenshot 1080p")]
    static void TakeScreenshot()
    {
        int width = 1920;
        int height = 1080;

        // Ensure Scene View exists
        if (SceneView.lastActiveSceneView == null)
        {
            Debug.LogError("No active Scene View found!");
            return;
        }

        SceneView sceneView = SceneView.lastActiveSceneView;
        Camera cam = sceneView.camera;
        if (cam == null)
        {
            Debug.LogError("Scene View has no camera.");
            return;
        }

        // Get current scene name
        string sceneName = SceneManager.GetActiveScene().name;
        if (string.IsNullOrEmpty(sceneName))
            sceneName = "UntitledScene";

        // Make sure the folder exists
        string folderPath = Application.dataPath + "/Screenshots";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Generate a unique file name
        string path = folderPath + "/" + sceneName + ".png";
        int counter = 1;
        while (File.Exists(path))
        {
            path = folderPath + "/" + sceneName + "_" + counter + ".png";
            counter++;
        }

        // Store gizmo state
        bool prevDrawGizmos = sceneView.drawGizmos;
        sceneView.drawGizmos = false; // Hide gizmos

        // Create render texture
        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;

        // Render
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Cleanup
        cam.targetTexture = null;
        RenderTexture.active = null;
        Object.DestroyImmediate(rt);

        // Restore gizmos
        sceneView.drawGizmos = prevDrawGizmos;
        sceneView.Repaint();

        // Save PNG
        File.WriteAllBytes(path, screenshot.EncodeToPNG());
        Debug.Log("Scene View screenshot saved to: " + path);

        // Refresh Unity so file appears in Project view
        AssetDatabase.Refresh();
    }
}
