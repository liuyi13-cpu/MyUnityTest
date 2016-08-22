using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

// 绑定对象
[CustomEditor(typeof(ATest_UI))]
public class ATest_UI_Editor : Editor
{
    public static bool _toggle_all;
    SerializedObject m_image_ID;

    public override void OnInspectorGUI()
    {
        // 当前被编辑的对象
        // Debugger.Log("OnInspectorGUI" + target);
        // EditorGUILayout.Separator();
        DrawDefaultInspector();

        // EditorGUILayout.Vector3Field("Color", )

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (GUILayout.Button(_toggle_all ? "Untoggle All" : "Toggle All"))
        {
            _toggle_all = !_toggle_all;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("123", GUILayout.Width(150));

        ATest_UI test =  target as ATest_UI;

        test.m_rect = EditorGUILayout.RectField("POS", test.m_rect);
        test.m_texture = EditorGUILayout.ObjectField("Texture", test.m_texture, typeof(Texture), true) as Texture;

        int[] a = { 0, 1, 2 };
        string[] b = { "a", "b", "c" };
        test.m_index = EditorGUILayout.IntPopup(test.m_index, b, a );

        // Repaint();

        var img = test.GetComponent<Image>();
        if (img)
        {
            Debugger.Log("sprite    " + img.sprite);
            Debugger.Log("overrideSprite    " + img.overrideSprite);
            if (img.material)
            {
                Debugger.Log("material  " + img.material.mainTexture);
                img.material.color = img.color;
                Debugger.Log("material color  " + img.material.color);

                // img.material.SetTexture("_AlphaTex", null);
            }
            Debugger.Log("mainTexture  " + img.mainTexture);
            Debugger.Log("color  " + img.color);

            
            var render = img.GetComponent<CanvasRenderer>();
            // render.GetMaterial().SetTexture("Alpha Texture", null);
            // render.SetPropertyBlock();
        }
    }
}
