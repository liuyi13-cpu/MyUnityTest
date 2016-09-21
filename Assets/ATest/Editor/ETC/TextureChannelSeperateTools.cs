using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// 图片通道分离工具: 添加不打图集的名单
/// </summary>
public class TextureChannelSeperateTools {

    private bool textMipmapEnabled = false;
    /// <summary>
    /// rgb图片的后缀
    /// </summary>
    private string rgbTextNameSuffix = "_RGB";
    /// <summary>
    /// alpha图片的后缀
    /// </summary>
    private string aplhaTextNameSuffix = "_Alpha";
    /// <summary>
    /// 创建图片的格式
    /// </summary>
    private string createTextFormat = "png";
    /// <summary>
    /// 材质球的位置
    /// </summary>
    private string MatShaderName = "A-Ex/ETC1_UI";


    private List<TextureImporter> ItemTextureImLists = new List<TextureImporter>();
    private List<string> TextPathList = new List<string>();
    private List<string> AlphaPathList = new List<string>();

    /// <summary>
    /// ui元素分开所有
    /// </summary>
    /// <param name="fitItems"></param>
    /// <param name="createMatPath"></param>
    /// <param name="createTextPath"></param>
    public void UISeperateAllItem(List<Object> fitItems) {
        CheckListIsNotEmpty(fitItems);
        Debugger.Log("-------------------------Start Departing.");
        InitData();
        int count = fitItems.Count;
        List<TextureImporter> oldTextImLists = new List<TextureImporter>();
        for (int i = 0; i < count; i++) {
            bool isCancel = ShowProgressBar("获取分层的图片：", fitItems[i].name, i, count);
            if (isCancel) {
                return;
            }
            oldTextImLists.Add(SetTextureFormat(fitItems[i]));
        }

        for (int i = 0; i < count; i++) {
            bool isCancel = ShowProgressBar("开始分层：", fitItems[i].name, i, count);
            if (isCancel) {
                return;
            }
            SeperateUIRGBAChannel(i, fitItems[i], oldTextImLists[i]);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();

        for (int i = 0; i < count; i++) {
            bool isCancel = ShowProgressBar("设置Alpha图片：", fitItems[i].name, i, count);
            if (isCancel) {
                return;
            }
            CreateSetedMat(TextPathList[i], fitItems[i].name, AlphaPathList[i]);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debugger.Log("-------------------------Finish Departing.");
    }

    private bool ShowProgressBar(string tips, string curItemName, int index, int total) {
        StringBuilder builder = new StringBuilder();
        builder.Append(tips).Append(index).Append("/").Append(total);
        bool isCancel = EditorUtility.DisplayCancelableProgressBar(builder.ToString(), curItemName, (float)index / total);
        if (isCancel) {
            EditorUtility.ClearProgressBar();
            return true;
        }
        return false;
    }

    /// <summary>
    /// ui元素分离一个
    /// </summary>
    /// <param name="fitItems"></param>
    public void UISeperateItem(Object fitItems) {
        Debugger.Log("-------------------------Start Departing.");
        InitData();
        TextureImporter oldIm = SetTextureFormat(fitItems);
        SeperateUIRGBAChannel(0, fitItems, oldIm);
        AssetDatabase.Refresh();
        CreateSetedMat(TextPathList[0], fitItems.name, AlphaPathList[0]);
        Debugger.Log("-------------------------Finish Departing.");
    }

    private void InitData() {
        ItemTextureImLists.Clear();
        AlphaPathList.Clear();
        TextPathList.Clear();
    }

    /// <summary>
    /// 场景分开所有
    /// </summary>
    /// <param name="fitItems"></param>
    public void SceneSeperateItemList(List<Object> fitItems) {
        CheckListIsNotEmpty(fitItems);
        Debugger.Log("-------------------------Start Departing.");
        InitData();
        int count = fitItems.Count;
        for (int i = 0; i < count; i++) {
            bool isCancel = ShowProgressBar("获取分层的图片：", fitItems[i].name, i, count);
            if (isCancel) {
                return;
            }
            SetTextureFormat(fitItems[i]);
        }
        for (int i = 0; i < count; i++) {
            bool isCancel = ShowProgressBar("开始分层：", fitItems[i].name, i, count);
            if (isCancel) {
                return;
            }
            SeperateSceneRGBAChannel(fitItems[i]);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debugger.Log("-------------------------Finish Departing.");
    }

    /// <summary>
    /// 场景分离一个
    /// </summary>
    /// <param name="fitItems"></param>
    public void SceneSeperateItem(Object fitItems) {
        Debugger.Log("-------------------------Start Departing.");
        SetTextureFormat(fitItems);
        SeperateSceneRGBAChannel(fitItems);
        AssetDatabase.Refresh();
        Debugger.Log("--------------------------Finish Departing.");
    }

    /// <summary>
    /// 检测列表是否为空
    /// </summary>
    /// <param name="fitItems"></param>
    private void CheckListIsNotEmpty(List<Object> fitItems) {
        if (fitItems.Count == 0) {
            Debugger.LogError("-------------------------No UI was selected!------------------------");
            throw new System.FormatException("No UI was selected! size == 0 ");
        }
    }

    /// <summary>
    /// 设置图片的格式
    /// </summary>
    private TextureImporter SetTextureFormat(Object item) {
        string path = AssetDatabase.GetAssetPath(item);
        TextureImporter text = AssetImporter.GetAtPath(path) as TextureImporter;
        ItemTextureImLists.Add(text);
        text = SetTextureReadabel(text);
        return text;
    }

    /// <summary>
    /// 设置图片的为可读为了分图
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private TextureImporter SetTextureReadabel(TextureImporter texture) {
        if (!texture.isReadable) {
            texture.textureType = TextureImporterType.Advanced;
            texture.textureFormat = TextureImporterFormat.AutomaticCompressed;
            texture.isReadable = true;
            texture.mipmapEnabled = textMipmapEnabled;
            texture.alphaIsTransparency = false;
            texture.SaveAndReimport();
        }
        return texture;
    }

    /// <summary>
    /// 分离场景图片的RGB和Alpha通道，保留原图，重新生成两张图片
    /// </summary>
    /// <param name="obj"></param>
    private void SeperateSceneRGBAChannel(Object obj) {
        Texture2D sourcetex = (Texture2D)obj;
        Texture2D rgbText = GetRGBChannelText(sourcetex);
        Texture2D alphaText = GetAlphaChannelText(sourcetex);

        string textName = obj.name;
        //默认生成在同级目录下
        string textPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
        
        string rgbPath = GetAbsoluteAssetPath(textPath, textName, rgbTextNameSuffix, createTextFormat);
        CreateFile(rgbText, rgbPath);

        string aplhaPath = GetAbsoluteAssetPath(textPath, textName, aplhaTextNameSuffix, createTextFormat);
        CreateFile(alphaText, aplhaPath);
    }


    /// <summary>
    /// 分离UI图片的RGB和Alpha通道，直接修改原图为rgb图片，生成对应的材质球
    /// </summary>
    /// <param name="obj"></param>
    private void SeperateUIRGBAChannel(int i, Object obj, TextureImporter orginalTextIm) {
        Debugger.Log("SeperateUIRGBAChannel--- Start: " + obj.name);
        Texture2D sourcetex = (Texture2D)obj;    

        string textName = obj.name;
        string assetPath = AssetDatabase.GetAssetPath(obj);
        string textPath = System.IO.Path.GetDirectoryName(assetPath);//默认生成在同级目录下

        string rgbPath = GetAbsoluteAssetPath(textPath, textName, "", createTextFormat);
        Texture2D rgbText = GetRGBChannelText(sourcetex, true);
        CreateFile(rgbText, rgbPath);            

        string alphaPath = GetAbsoluteAssetPath(textPath, textName, aplhaTextNameSuffix, createTextFormat);
        Texture2D alphaText = GetAlphaChannelText(sourcetex);
        CreateFile(alphaText, alphaPath);


        TextureImporter text = ItemTextureImLists[i];
        text.spritePixelsPerUnit = orginalTextIm.spritePixelsPerUnit;
        text.anisoLevel = orginalTextIm.anisoLevel;
        text.borderMipmap = orginalTextIm.borderMipmap;
        text.maxTextureSize = orginalTextIm.maxTextureSize;
        text.spriteImportMode = orginalTextIm.spriteImportMode;
        text.textureType = TextureImporterType.Advanced;
        text.mipmapEnabled = false;
        text.isReadable = false;
        text.spritesheet = orginalTextIm.spritesheet;
        text.spriteBorder = orginalTextIm.spriteBorder;
        text.SaveAndReimport();
        TextPathList.Add(textPath);
        AlphaPathList.Add(alphaPath);

        Debugger.Log("SeperateUIRGBAChannel--- End: " + obj.name);
    }

    /// <summary>
    /// 创建材质球并且设置材质球的图片
    /// </summary>
    /// <param name="textPath"></param>
    /// <param name="alphaPath"></param>
    /// <param name="textName"></param>
    private void CreateSetedMat(string textPath, string textName, string alphaPath) {
        string shaderPath = GetAbsoluteAssetPath(textPath, textName, "", "mat");
        string aplhaResourcePath = GetResourePathByAbsoluteAssetPath(alphaPath);
        CreateMat(shaderPath, textName, aplhaResourcePath);
        Texture2D alphaObj = Resources.Load<Texture2D>(aplhaResourcePath);
        SetAlphaTexture(alphaObj);
    }

    /// <summary>
    /// 设置alpha图片的数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private TextureImporter SetAlphaTexture(Object obj) {
        string path2 = AssetDatabase.GetAssetPath(obj);
        TextureImporter texture = AssetImporter.GetAtPath(path2) as TextureImporter;
        texture.textureType = TextureImporterType.Advanced;
        texture.textureFormat = TextureImporterFormat.AutomaticCompressed;
        texture.isReadable = false;
        texture.mipmapEnabled = textMipmapEnabled;
        texture.alphaIsTransparency = false;
        texture.SaveAndReimport();
        return texture;
    }
    /// <summary>
    /// 获取图片的RGB通道图片
    /// </summary>
    /// <returns></returns>
    private Texture2D GetRGBChannelText(Texture2D sourceText, bool isAvoidPicCombine = false) {
        Texture2D rgbTex = new Texture2D(sourceText.width, sourceText.height, TextureFormat.RGB24, textMipmapEnabled);
        Color[] rgbColors = sourceText.GetPixels();
        if (isAvoidPicCombine) {
            int count = rgbColors.Length;
            for (int i = 0; i < count; ++i) {
                if (rgbColors[i].r == 1 && rgbColors[i].g == 1 && rgbColors[i].b == 1 && rgbColors[i].a != 0) { //防止白图
                    rgbColors[i].r = 0.96f;
                    rgbColors[i].g = 0.96f;
                    rgbColors[i].b = 0.96f;
                }
                if (rgbColors[i].r == 1 && rgbColors[i].g == 1 && rgbColors[i].b == 1 && rgbColors[i].a == 0) { //防止白图
                    rgbColors[i].r = 0f;
                    rgbColors[i].g = 0f;
                    rgbColors[i].b = 0f;
                }
            }
        }       

        rgbTex.SetPixels(rgbColors);
        rgbTex.Apply();
        return rgbTex;
    }

    /// <summary>
    /// 获取图片的Aplha通道图片
    /// </summary>
    /// <returns></returns>
    private Texture2D GetAlphaChannelText(Texture2D sourceText) {
        Color[] colorsAlpha = new Color[sourceText.width * sourceText.height];
        Color[] original = sourceText.GetPixels();
        int count = original.Length;
        for (int i = 0; i < count; ++i) {
            colorsAlpha[i].r = original[i].a;
            colorsAlpha[i].g = original[i].a;
            colorsAlpha[i].b = original[i].a;
        }

        Texture2D alphaTex = new Texture2D(sourceText.width, sourceText.height, TextureFormat.RGB24, textMipmapEnabled);
        alphaTex.SetPixels(colorsAlpha);
        alphaTex.Apply();
        return alphaTex;
    }

    /// <summary>
    /// 创建图片
    /// </summary>
    /// <param name="text">材质</param>
    /// <param name="createPath">生成路径</param>
    private void CreateFile(Texture2D text, string createAbsolutePath) {
        byte[] bytes = text.EncodeToPNG();
        File.WriteAllBytes(createAbsolutePath, bytes);
    }

    /// <summary>
    /// 获取生成图片的绝对路径
    /// </summary>
    /// <param name="createPath"></param>
    /// <param name="fileName"></param>
    /// <param name="nameSuffix"></param>
    /// <returns></returns>
    private string GetAbsoluteAssetPath(string createPath, string fileName, string nameSuffix, string format) {
        StringBuilder sql = new StringBuilder();
        sql.Append(createPath).Append("/").Append(fileName).Append(nameSuffix).AppendFormat(".").AppendFormat(format);
        return sql.ToString();
    }

    /// <summary>
    /// 根据图片的绝对路径获取Resource路径
    /// </summary>
    /// <returns></returns>
    private string GetResourePathByAbsoluteAssetPath(string absoluteAssetPath) {
        string resourcesFolderName = "Resources/";
        int index = absoluteAssetPath.IndexOf(resourcesFolderName);
        string resourceFilePath = absoluteAssetPath.Remove(0, index + resourcesFolderName.Length);
        return resourceFilePath.Replace("." + createTextFormat, "");
    } 

    /// <summary>
    /// 生成材质球
    /// </summary>
    /// <param name="shaderPath"></param>
    /// <param name="shaderName"></param>
    private void CreateMat(string shaderPath, string shaderName, string alphaTextPath) {
        Material mat = Resources.Load<Material>(alphaTextPath.Replace(aplhaTextNameSuffix, ""));
        if (mat == null) {
            mat = new Material(Shader.Find(MatShaderName));
            AssetDatabase.CreateAsset(mat, shaderPath);
            mat.name = shaderName;
        } else {
        }
        Texture2D alphaText = Resources.Load<Texture2D>(alphaTextPath);// "Atlas/UI3.0/Area_Strategic_Operation/Atlas_Area_Strategic_Operation_02_Alpha");
        mat.SetTexture("_AlphaTex", alphaText);
        EditorUtility.SetDirty(mat);
        AssetDatabase.SaveAssets();
    }
}
