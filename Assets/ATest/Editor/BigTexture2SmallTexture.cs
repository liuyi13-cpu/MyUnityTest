using System.IO;
using UnityEditor;
using UnityEngine;

public class BigTexture2SmallTexture
{
    [MenuItem("Assets/Tools/大图转碎图")]
    static void Export()
    {
        Texture2D tex = Selection.activeObject as Texture2D;
        if (tex == null)
        {
            Debugger.LogError("无效的格式");
            return;
        }

        var assetPath = AssetDatabase.GetAssetPath(tex);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        TextureImporterFormat saveFormat = importer.textureFormat;
        importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
        importer.isReadable = true;
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        string targetDirectory = Application.dataPath + "/../Data/Effects/" + tex.name;
        Directory.CreateDirectory(targetDirectory);

        SpriteMetaData[] sheet = importer.spritesheet;
        foreach (var item in sheet)
        {
            string name = item.name;
            int x = (int)item.rect.x;
            int y = (int)item.rect.y;
            int w = (int)item.rect.width;
            int h = (int)item.rect.height;
            Texture2D subTex = new Texture2D(w, h, TextureFormat.ARGB32, false);
            subTex.SetPixels(tex.GetPixels(x, y, w, h));

            File.WriteAllBytes(targetDirectory + "/" + name + ".png", subTex.EncodeToPNG());

            Debugger.Log("生成碎图:" + name);
        }

        // reset
        importer.textureFormat = saveFormat;
        importer.isReadable = false;
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }
}