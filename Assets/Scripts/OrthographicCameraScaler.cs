using UnityEngine;

/// <summary>
/// [Answer to Question 3A – Explanation]
/// When rotating the simulator (or a mobile device) between Landscape and Portrait,
/// the screen’s aspect ratio changes. Since the camera is set to **Orthographic** mode,
/// the `orthographicSize` property determines **half the height** of the visible world units.
///
/// However, Unity stretches the horizontal view to fit the screen width,
/// which leads to a visual side effect: objects **appear larger or smaller** depending on the orientation,
/// even though their scale hasn’t changed.
///
/// This happens because a fixed `orthographicSize` doesn't adapt to the new screen ratio.
/// In Portrait mode, the screen is taller and narrower — which results in **less of the scene being visible**,
/// so everything looks zoomed-in.
///
/// [Goal of this script]
/// This component dynamically adjusts the camera's `orthographicSize` in real-time,
/// based on the current screen orientation and aspect ratio,
/// to **maintain a consistent visual size of all objects** regardless of screen rotation.
/// </summary>

public class OrthographicCameraScaler : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private Camera targetCamera;
    
    [Header("Simple Mode - Set fixed sizes for each orientation")]
    [SerializeField] private bool useSimpleMode = false;
    [SerializeField] private float portraitCameraSize = 5f;
    [SerializeField] private float landscapeCameraSize = 11f; // Increased to make objects smaller in landscape
    
    [Header("Base Aspect Ratio - Adjust this to fine-tune size balance")]
    [SerializeField] private float baseAspectRatio = 0.6f; // Try values between 0.5-1.0
    
    [Header("Base Orthographic Size - Tune for good Portrait appearance")]
    [SerializeField] private float baseOrthographicSize = 5f;
    
    [Header("Advanced Settings")]
    [SerializeField] private bool updateOnStart = true;
    [SerializeField] private bool debugMode = false;
    
    // Private variables
    private float lastScreenWidth;
    private float lastScreenHeight;
    private bool isInitialized = false;

    void Start()
    {
        // Initialize camera if not set
        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();
        
        if (targetCamera == null)
        {
            Debug.LogError("No camera found! Please assign a camera to the script or place it on a GameObject with a camera.");
            return;
        }

        // Save current base size if not set beforehand
        if (baseOrthographicSize <= 0)
        {
            baseOrthographicSize = targetCamera.orthographicSize;
        }

        // Initial update
        if (updateOnStart)
        {
            UpdateCameraSize();
        }
        
        // Save current resolution
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        isInitialized = true;

        if (debugMode)
        {
            Debug.Log($"OrthographicCameraScaler initialized: BaseAspect={baseAspectRatio:F3}, BaseSize={baseOrthographicSize}");
        }
    }

    void Update()
    {
        // Check if resolution changed (screen rotation)
        if (isInitialized && (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight))
        {
            UpdateCameraSize();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            
            if (debugMode)
            {
                Debug.Log($"Resolution change detected: {Screen.width}x{Screen.height}");
            }
        }
    }

    /// <summary>
    /// Update camera size according to current screen aspect ratio
    /// </summary>
    public void UpdateCameraSize()
    {
        if (targetCamera == null) return;

        if (useSimpleMode)
        {
            // Simple mode - use fixed sizes for each orientation
            bool isLandscape = Screen.width > Screen.height;
            targetCamera.orthographicSize = isLandscape ? landscapeCameraSize : portraitCameraSize;
            
            if (debugMode)
            {
                Debug.Log($"Simple Mode: {(isLandscape ? "Landscape" : "Portrait")} size = {targetCamera.orthographicSize}");
            }
        }
        else
        {
            // Advanced mode - calculate based on aspect ratio
            float currentAspectRatio = (float)Screen.height / (float)Screen.width;
            float scaleFactor = baseAspectRatio / currentAspectRatio;
            float newOrthographicSize = baseOrthographicSize * scaleFactor;
            targetCamera.orthographicSize = newOrthographicSize;

            if (debugMode)
            {
                Debug.Log($"Advanced Mode: CurrentAspect={currentAspectRatio:F3}, ScaleFactor={scaleFactor:F3}, NewSize={newOrthographicSize:F2}");
                Debug.Log($"Screen orientation: {(Screen.width > Screen.height ? "Landscape" : "Portrait")}");
            }
        }
    }

    /// <summary>
    /// Set new base aspect ratio
    /// </summary>
    /// <param name="newBaseAspect">New aspect ratio (height/width)</param>
    public void SetBaseAspectRatio(float newBaseAspect)
    {
        baseAspectRatio = newBaseAspect;
        UpdateCameraSize();
    }

    /// <summary>
    /// Set new base orthographic size
    /// </summary>
    /// <param name="newBaseSize">New base size</param>
    public void SetBaseOrthographicSize(float newBaseSize)
    {
        baseOrthographicSize = newBaseSize;
        UpdateCameraSize();
    }

    /// <summary>
    /// Reset to current size as new base
    /// </summary>
    public void ResetToCurrentSize()
    {
        if (targetCamera != null)
        {
            baseOrthographicSize = targetCamera.orthographicSize;
            baseAspectRatio = (float)Screen.height / (float)Screen.width;
            
            if (debugMode)
            {
                Debug.Log($"Reset to current values: BaseAspect={baseAspectRatio:F3}, BaseSize={baseOrthographicSize}");
            }
        }
    }

    /// <summary>
    /// Force manual update
    /// </summary>
    [ContextMenu("Update Camera Size")]
    public void ForceUpdate()
    {
        UpdateCameraSize();
    }

    // Display debug info
    void OnGUI()
    {
        if (!debugMode) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.white;

        float currentAspect = (float)Screen.height / (float)Screen.width;
        string orientation = Screen.width > Screen.height ? "Landscape" : "Portrait";
        
        GUI.Label(new Rect(10, 10, 300, 30), $"Resolution: {Screen.width}x{Screen.height}", style);
        GUI.Label(new Rect(10, 30, 300, 30), $"Orientation: {orientation}", style);
        GUI.Label(new Rect(10, 50, 300, 30), $"Current Aspect: {currentAspect:F3}", style);
        GUI.Label(new Rect(10, 70, 300, 30), $"Base Aspect: {baseAspectRatio:F3}", style);
        GUI.Label(new Rect(10, 90, 300, 30), $"Camera Size: {targetCamera.orthographicSize:F2}", style);
    }
}

/// <summary>
/// Common aspect ratios for different devices
/// </summary>
public static class CommonAspectRatios
{
    // Portrait (height/width)
    public const float iPhone12_Portrait = 19.5f / 9f;      // ~2.167
    public const float iPhone8_Portrait = 16f / 9f;         // ~1.778
    public const float iPadPro_Portrait = 4f / 3f;          // ~1.333
    public const float AndroidStandard_Portrait = 18f / 9f; // 2.0
    
    // Landscape (width/height) - inverse of Portrait
    public const float iPhone12_Landscape = 9f / 19.5f;     // ~0.462
    public const float iPhone8_Landscape = 9f / 16f;        // ~0.563
    public const float iPadPro_Landscape = 3f / 4f;         // 0.75
    public const float AndroidStandard_Landscape = 9f / 18f; // 0.5
}