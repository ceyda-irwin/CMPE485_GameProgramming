using UnityEngine;
using UnityEditor;

/// <summary>
/// Sahneyi küçük duvarlarla doldurur. Sonra Hierarchy'den veya Scene view'da
/// istediğin duvarları seçip Delete ile silebilirsin - labirent oluşturmak için.
/// </summary>
public class MazeWallGenerator : EditorWindow
{
    // WallsIn klasöründeki duvarlarla aynı: scale (5, 6, 1), Y merkez 3, Cube + URP Lit
    int gridWidth = 10;
    int gridDepth = 10;
    float wallWidth = 5f;
    float wallHeight = 6f;
    float wallThickness = 1f;
    Vector3 startPosition = new Vector3(40, 3, -20);
    bool useURPMaterial = true;

    [MenuItem("Tools/Maze Wall Generator")]
    public static void ShowWindow()
    {
        GetWindow<MazeWallGenerator>("Labirent Duvar Oluşturucu");
    }

    void OnGUI()
    {
        GUILayout.Label("Labirent Duvar Grid'i", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        gridWidth = EditorGUILayout.IntField("Grid Genişlik (X)", gridWidth);
        gridDepth = EditorGUILayout.IntField("Grid Derinlik (Z)", gridDepth);
        EditorGUILayout.Space(3);
        wallWidth = EditorGUILayout.FloatField("Duvar Genişlik (X)", wallWidth);
        wallHeight = EditorGUILayout.FloatField("Duvar Yükseklik (Y)", wallHeight);
        wallThickness = EditorGUILayout.FloatField("Duvar Kalınlık (Z)", wallThickness);
        startPosition = EditorGUILayout.Vector3Field("Başlangıç Pozisyonu (merkez)", startPosition);
        useURPMaterial = EditorGUILayout.Toggle("URP Materyali Kullan (WallsIn ile aynı)", useURPMaterial);

        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox(
            "Bu araç sahneye grid şeklinde küçük duvarlar yerleştirir. " +
            "Labirent oluşturmak için Hierarchy veya Scene view'da istediğin duvarları " +
            "seçip Delete tuşuyla silebilirsin.",
            MessageType.Info);

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Sahneyi Duvar Grid ile Doldur", GUILayout.Height(35)))
        {
            FillSceneWithWalls();
        }
    }

    void FillSceneWithWalls()
    {
        if (gridWidth <= 0 || gridDepth <= 0 || wallWidth <= 0 || wallHeight <= 0 || wallThickness <= 0)
        {
            EditorUtility.DisplayDialog("Hata", "Grid ve duvar boyutları 0'dan büyük olmalı.", "Tamam");
            return;
        }

        GameObject parent = new GameObject("MazeWalls_Grid");
        Undo.RegisterCreatedObjectUndo(parent, "Create Maze Walls");

        Material mat = null;
        if (useURPMaterial)
        {
            string path = AssetDatabase.GUIDToAssetPath("31321ba15b8f8eb4c954353edc038b1d");
            if (!string.IsNullOrEmpty(path))
                mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
                mat = AssetDatabase.LoadAssetAtPath<Material>(
                    "Packages/com.unity.render-pipelines.universal/Runtime/Materials/Lit.mat");
        }
        if (mat == null)
            mat = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");

        int count = 0;

        // Yatay duvarlar (X yönünde): köşe (x,z) ile (x+1,z) arasında, merkez (x+0.5)*w
        for (int z = 0; z <= gridDepth; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector3 pos = startPosition + new Vector3((x + 0.5f) * wallWidth, 0f, z * wallWidth);
                GameObject wall = CreateWall(parent, pos, new Vector3(wallWidth, wallHeight, wallThickness), mat, $"WallX_{x}_{z}");
                Undo.RegisterCreatedObjectUndo(wall, "Create Wall");
                count++;
            }
        }

        // Dikey duvarlar (Z yönünde): köşe (x,z) ile (x,z+1) arasında, merkez (z+0.5)*w
        for (int x = 0; x <= gridWidth; x++)
        {
            for (int z = 0; z < gridDepth; z++)
            {
                Vector3 pos = startPosition + new Vector3(x * wallWidth, 0f, (z + 0.5f) * wallWidth);
                GameObject wall = CreateWall(parent, pos, new Vector3(wallThickness, wallHeight, wallWidth), mat, $"WallZ_{x}_{z}");
                Undo.RegisterCreatedObjectUndo(wall, "Create Wall");
                count++;
            }
        }

        Selection.activeGameObject = parent;
        SceneView.lastActiveSceneView?.FrameSelected();
        EditorUtility.DisplayDialog("Tamamlandı", $"{count} duvar oluşturuldu (X ve Z eksenlerinde).\n\nLabirent yapmak için Hierarchy'den veya Scene view'dan duvarları seçip Delete ile sil.", "Tamam");
    }

    GameObject CreateWall(GameObject parent, Vector3 pos, Vector3 scale, Material mat, string name)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.SetParent(parent.transform);
        wall.transform.localPosition = pos;
        wall.transform.localRotation = Quaternion.identity;
        wall.transform.localScale = scale;
        if (mat != null)
        {
            var renderer = wall.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.sharedMaterial = mat;
        }
        return wall;
    }
}
