using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialTextureForETC1
{
    /// <summary>
    ///  不使用ETC1格式的目录
    /// </summary>
    private static string notETCFolderNames = "_TrueColor";

    private static string pictureFitStr = "_ETC1";

    [MenuItem("Assets/ETC1/Scene Seperate ALL", priority = 1)]
    /// <summary>
    /// AllSceneTextureSeperateChannel 分离多个图片，后缀名字为pictureFitStr
    /// </summary>
    static void AllSceneTextureSeperateChannel(){
        pictureFitStr = "_ETC1";
        TextureChannelSeperateTools tools = new TextureChannelSeperateTools();
        List<Object> fitItems = GetSeletedFitItem();
        tools.SceneSeperateItemList(fitItems);
    }

	[MenuItem("Assets/ETC1/Scene Seperate Single", priority = 2)]
    /// <summary>
    /// SingleSceneTextureSeperateChannel 分离单个图片，后缀名字为pictureFitStr
    /// </summary>
    static void SingleSceneTextureSeperateChannel() {
        pictureFitStr = "_ETC1";
        Object curSelectObj = Selection.activeObject;
        if(!IsCurSelectedObjFit(curSelectObj)){
            return;
        }
        TextureChannelSeperateTools tools = new TextureChannelSeperateTools();
        tools.SceneSeperateItem(curSelectObj);
    }


    [MenuItem("Assets/ETC1/UI Seperate ALL", priority = 3)]
    /// <summary>
    /// SingleSceneTextureSeperateChannel 分离多个图片，并生成对应的材质球，名字无要求，但是大小必须为2的次幂
    /// </summary>   
    static void AllUITextureSeperateChannel() {
        pictureFitStr = "";
        TextureChannelSeperateTools tools = new TextureChannelSeperateTools();
        List<Object> fitItems = GetSeletedFitItem();
        tools.UISeperateAllItem(fitItems);
    }

    [MenuItem("Assets/ETC1/UI Seperate Single", priority = 4)]
    /// <summary>
    /// SingleSceneTextureSeperateChannel 分离单个图片
    /// </summary> 
    static void SingleUITextureSeperateChannel() {
        pictureFitStr = "";
        TextureChannelSeperateTools tools = new TextureChannelSeperateTools();
        Object curSelectObj = Selection.activeObject;
        if (!IsCurSelectedObjFit(curSelectObj)) {
            return;
        }
        tools.UISeperateItem(curSelectObj);
    }

    /// <summary>
    /// SingleSceneTextureSeperateChannel 分离多个图片，并生成对应的材质球，名字无要求，但是大小必须为2的次幂
    /// </summary>   
    static void UISeperateChannelByPath() {
        List<string> texturePathList = GetTexturePathStr();
        pictureFitStr = "";
        List<Texture2D> textures = new List<Texture2D>();
        for (int i = 0; i< texturePathList.Count; i++) {
            textures.AddRange(Resources.LoadAll<Texture2D>(texturePathList[i]));
        }

        List<Object> textLists = new List<Object>(textures.ToArray());
        int count = textLists.Count;
        for (int i = count - 1; i >= 0; i--) {
            if (!IsFitSeparateRuleItem(textLists[i])) {
                textLists.RemoveAt(i);
            }
        }
        TextureChannelSeperateTools tools = new TextureChannelSeperateTools();        
  //      List<Object> fitItems = GetSeletedFitItem();
        tools.UISeperateAllItem(textLists);
    }

    /// <summary>
    /// 获取命令行的路径
    /// </summary>
    /// <returns></returns>
    static List<string> GetTexturePathStr() {
        string[] args = System.Environment.GetCommandLineArgs();
        int count = args.Length;
        string startParam = "StartParam";
        string endParam = "EndParam";

        List<string> texturePathList = new List<string>();
        bool isStartRecord = false;
        for (int i = 0; i < count; i++) {
            if (args[i] == endParam) {
                isStartRecord = false;
            }

            if (isStartRecord) {
                texturePathList.Add(args[i]);
            }
            if (args[i] == startParam) {
                isStartRecord = true;
            }
           
        }
        return texturePathList;
    }

    /// <summary>
    /// 判断当前选中的物体是否符合规则
    /// </summary>
    /// <param name="curSelectObj"></param>
    /// <returns></returns>
    static bool IsCurSelectedObjFit(Object curSelectObj) {
        if (curSelectObj == null) {
            Debugger.LogError("You have not selected anything!");
            return false;
        }
        if ((Texture)curSelectObj == null) {
            Debugger.LogError("You did not choose a single image!:"+ curSelectObj.name);
            return false;
        }

        if (!IsFitSeparateRuleItem(curSelectObj)) {
            return false;
        }
        return true;
    }

    #region GetSeletedFitItem: 获取当前选中目录下的所有符合分层要求的图片
    /// <summary>
    /// 获取当前选中目录下的所有符合分层要求的图片
    /// </summary>
    static List<Object> GetSeletedFitItem() {
        Object[] allTexts = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
        if (allTexts == null) {
            Debugger.LogError("The currently selected object has no pictures inside!");
        }
        return GetNeedSeparateItemLists(allTexts);
    }

    /// <summary>
    /// 获取需要分层的图片或图集
    /// </summary>
    /// <param name="allTexts"></param>
    /// <returns></returns>
    static List<Object> GetNeedSeparateItemLists(Object[] allTexts) {
        List<Object> allTextLists = new List<Object>(allTexts);
        int count = allTextLists.Count;
        for (int i = count - 1; i >= 0; i--) {
            if (!IsFitSeparateRuleItem(allTextLists[i])) {
                allTextLists.RemoveAt(i);
            }
        }
        return allTextLists;
    }

    /// <summary>
    /// 是否符合分层规则的物体：图集的后缀为_use， 不为_alpha,则分层（并且图片大小必须为2的次幂）,
    /// </summary>
    static bool IsFitSeparateRuleItem(Object item) {
        bool isFitSize = IsTextSizeFitPower(item);
        bool isFitName = IsNameFitRule(item);        
        
        string path = AssetDatabase.GetAssetPath(item);

        if (path.Contains(notETCFolderNames)) {
            Debugger.Log("不能设置该图片：" + item.name);
            return false;
        }

        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        if (textureImporter != null && !textureImporter.DoesSourceTextureHaveAlpha()) {
            Debugger.Log("The images are not transparent layer, there is no need stratification!!" + item.name);
            return false;
        }

        SpriteMetaData[] spriteMetaDatas = textureImporter.spritesheet;
        Texture2D sourcetex = (Texture2D)item;
        if (spriteMetaDatas != null) {
            int count = spriteMetaDatas.Length;
            for (int i = 0; i < count; i++) {
                if (spriteMetaDatas[i].rect.width > sourcetex.width || spriteMetaDatas[i].rect.height > sourcetex.height
                    || spriteMetaDatas[i].rect.x > sourcetex.width || spriteMetaDatas[i].rect.y > sourcetex.height) {
                    Debugger.LogError("Cur Texture size is wrong!" + sourcetex.name);
                    return false;
                }
            }
        }
        
        return isFitSize && isFitName;
    }

    /// <summary>
    /// 判断名字是否符合规则
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    static bool IsNameFitRule(Object item) {
        if (item.name.EndsWith("_Alpha")) {
            Debugger.Log("This is a transparent channel image, there is no need stratification" + item.name);
            return false;
        }

        if (item.name.EndsWith("_RGB")) {
            Debugger.Log("The images are not transparent layer, there is no need stratification!" + item.name);
            return false;
        }

        if (pictureFitStr == "") {
            return true;
        }

        if (item.name.EndsWith(pictureFitStr)) {
            return true;
        }else {
            Debugger.Log("The name of the image does not meet the requirements of the suffix! Picturesuffix: " + pictureFitStr + ", name: " + item.name);
        }
        return false;
    }


    /// <summary>
    /// 检测图片的尺寸是否符合2的次幂
    /// </summary>
    /// <param name="text"></param>
    static bool IsTextSizeFitPower(object obj) {
        Texture2D text = (Texture2D)obj;
        int width = text.width;
        int height = text.height;
        if (Mathf.IsPowerOfTwo(width) && Mathf.IsPowerOfTwo(height)) {
            return true;
        }

        Debugger.Log("The size of the current picture does not meet the power requirements of 2!: " + text.name);
        return false;
        /*
        if ((width & (width - 1)) != 0 || (height & (height - 1)) != 0) {
            Debugger.Log("The size of the current picture does not meet the power requirements of 2!: "+ text.name);
            return false;
        }*/
        return true;
    }
    #endregion   
}