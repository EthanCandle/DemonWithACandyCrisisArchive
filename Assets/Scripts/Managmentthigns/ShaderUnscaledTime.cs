using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderUnscaledTime : MonoBehaviour
{
    public string targetShaderName = "Shaders/WaterLavaShader"; // Change to match your shader

    public Shader targetShader;
    public List<Material> matchingMaterials = new List<Material>();

    public static ShaderUnscaledTime Instance;
    void Awake()
    {
        if (transform.parent != null)
        {
            //transform.SetParent(null); // Unparent it to make it a root GameObject
        }
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
        //targetShader = Shader.Find(targetShaderName);
        if (targetShader == null)
        {
            Debug.LogError("Shader not found: " + targetShaderName);
            enabled = false;
            return;
        }

       // FindMatchingMaterials();
    }

    void Update()
    {
        float unscaled = Time.unscaledTime;
        foreach (Material mat in matchingMaterials)
        {
            if (mat != null)
                mat.SetFloat("_UnscaledTime", unscaled);
        }
    }

    void FindMatchingMaterials()
    {
        matchingMaterials.Clear();

        // This will find all materials currently loaded in memory
        var allMaterials = Resources.FindObjectsOfTypeAll<Material>();
        foreach (var mat in allMaterials)
        {
            if (mat.shader == targetShader)
                matchingMaterials.Add(mat);
        }
    }
}
