using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class SetETCTools {

    public static string relativelyPath = "/Resources/AllSceneCanvas";

    [MenuItem("Assets/ETC1/Set all objects with ETC1 format", priority = 10)]
    /// <summary>
    /// SeperateAllTextChannel 分离多个图片
    /// </summary>
    static void SetAllUIWithETCFormat() {
        Debugger.Log("ETC1 seting start------------------------------");
        List<GameObject> prefabList = GetAllUIPrefab();       

        int prefabCount = prefabList.Count;
        Debugger.Log("prefabCount = " + prefabCount);

        List<Image> allImages = new List<Image>();
        for (int i = prefabCount - 1; i >= 0; i--) {
            List<Image> onePrefabImageList = GetPrefabAllImages(prefabList[i]);
            Debugger.Log("Prefab name = " + prefabList[i].name + ", imageCount: "+onePrefabImageList.Count);
            allImages.AddRange(onePrefabImageList);
        }
        Debugger.Log("Images count: "+ allImages.Count);
        ReplaceImageWithETCFormat(allImages);

        for (int i = prefabCount - 1; i >= 0; i--) {
            EditorUtility.SetDirty(prefabList[i]);
        }
        AssetDatabase.SaveAssets();
        Debugger.Log("ETC1 seting end------------------------------");
    }

    /// <summary>
    /// 获取所有的  
    /// </summary>
    /// <returns></returns>
    static List<GameObject> GetAllUIPrefab() {
        Object[] allObjs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        List<Object> allObjsList = new List<Object>(allObjs);
        List<GameObject> prefabList = new List<GameObject>();
        int count = allObjsList.Count;
        for (int i = count - 1; i >= 0; i--) {
            if (CheckObjectIsUIPrefab(allObjsList[i])) {
                prefabList.Add((GameObject)allObjsList[i]);
            }
        }
        return prefabList;
    }

    /// <summary>
    /// 判断是否为UI预制体
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    static bool CheckObjectIsUIPrefab(Object obj) {
        if (obj.GetType() != typeof(GameObject)) {
            return false;
        }
        GameObject item = (GameObject)obj;
        if (item != null && item.layer != LayerMask.NameToLayer("UI")) {
           // Debugger.Log(obj.name + " is not ui prefab!");
            return false;
        }        
        return true;
    }

    /// <summary>
    /// 获取一个预制体上的图片组件
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    static List<Image> GetPrefabAllImages(GameObject item) {        
        List<Image> imageList = new List<Image>();
        GetImagesComponent(imageList, item.transform);
        return imageList;
    }

    /// <summary>
    /// 获取所有的图片组件
    /// </summary>
    /// <param name="imageList"></param>
    /// <param name="trans"></param>
    static void GetImagesComponent(List<Image> imageList, Transform trans) {
        Image image = trans.gameObject.GetComponent<Image>();
        if (image != null && image.sprite != null) {
            if (image.sprite.name != "UIMask" && image.sprite.name != "UISprite" && image.material == null) {
                imageList.Add(image);
            }                

            if (image.sprite.name != "UIMask" && image.material != null && image.material.name == "Default UI Material") {
                imageList.Add(image);
            }                
            
            if (image.sprite.name != "UISprite" && image.sprite.name != "UIMask" && image.material.name != "Default UI Material" && image.material.shader.name != "A-Ex/ETC1_UI")
                Debugger.LogWarning("path:" + AssetDatabase.GetAssetPath(image) + ", "+image.name + ", matName: " + image.material.name);
        }
        int childCount = trans.childCount;
        if(childCount == 0) {
            return;
        } else {
            for (int i = 0; i < childCount; i++) {
                Transform obj = trans.GetChild(i);
                GetImagesComponent(imageList, obj);
            }
        }        
    }

    /// <summary>
    /// 设置一张图片的格式为ETC,添加材质球
    /// </summary>
    /// <param name="image"></param>
    static void ReplaceImageWithETCFormat(List<Image> imageLists) {
        int count = imageLists.Count;
        for (int i = 0; i < count; i++) {
            Sprite sprite = imageLists[i].sprite;
            Texture2D text2D = sprite.texture;
            Material mat = Resources.Load<Material>(GetResourePath(text2D));
            if(mat != null) {
                Debugger.Log("0000000000000000 path: " + AssetDatabase.GetAssetPath(imageLists[i]) + ",mat: " +mat.name + ", "+ imageLists[i].name);
                imageLists[i].material = mat;
            } else {
                Debugger.LogWarning("Count't find mat :" + AssetDatabase.GetAssetPath(imageLists[i]) + ", " + imageLists[i].name + ", matName: " + imageLists[i].material.name);
            }
        }
    }

    /// <summary>
    /// 获取图片Resource路径
    /// </summary>
    /// <returns></returns>
    static string GetResourePath(Texture2D text2D) {
        string resourcesFolderName = "Resources/";
        string path = AssetDatabase.GetAssetPath(text2D);
        string resourceFilePath = path.Remove(0, path.IndexOf(resourcesFolderName) + resourcesFolderName.Length);
        if (resourceFilePath.IndexOf(".png") != -1) {
            resourceFilePath = resourceFilePath.Replace(".png", "");
        }
        if (resourceFilePath.IndexOf(".PNG") != -1) {
            resourceFilePath = resourceFilePath.Replace(".PNG", "");
        }        
        return resourceFilePath;
    }


    [MenuItem("Assets/ETC1/Set cur object with ETC1 format", priority = 11)]
    static void SetOneUIWithETCFormat() {
        GameObject obj = Selection.activeGameObject;
        if (CheckObjectIsUIPrefab(obj)){
            EditorApplication.SaveAssets();
            List<Image> imageLists = GetPrefabAllImages(obj);           
            ReplaceImageWithETCFormat(imageLists);
            EditorUtility.SetDirty(obj);
          //  AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
          //  EditorApplication.SaveAssets();
        }
    }
}
