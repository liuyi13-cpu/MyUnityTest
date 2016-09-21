using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using com.fanxing.protos;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Text;
using com.fanxing.consts;

public class DBAccess : MonoBehaviour {
    public string dbPath;
    public static string tkDbPath;
    public static string localPlayerInfoDbPath;
    private bool istkDbDownload = false;
    private bool islocalPlayerInfoDb = false;

    void Awake() {
        tkDbPath = Common.dbPath + "/tk.db";
        dbPath = tkDbPath;
        localPlayerInfoDbPath = Common.dbPath + "/LocalPlayerInfo.db";
        if (!File.Exists(tkDbPath)) {
            StartDownloadTkDB();
        }
        else {
            istkDbDownload = true;
        }
        if (!File.Exists(localPlayerInfoDbPath)) {
            StartDownloadLocalPlayerInfoDbDB();
        }
        else {
            islocalPlayerInfoDb = true;
        }
    }

    IEnumerator Start() {
        while (!istkDbDownload || !islocalPlayerInfoDb) {
            yield return null;
        }

        // DBHelper.Instance.OpenConnection(tkDbPath);
        // DBHelper.Instance.AttachDb(localPlayerInfoDbPath);
    }

    public void StartDownloadTkDB() {
        StartCoroutine(DownloadTkDB());
    }

    public void StartDownloadLocalPlayerInfoDbDB() {
        StartCoroutine(DownloadLocalPlayerInfoDbDB());
    }


    IEnumerator DownloadTkDB() {
        string path = Application.streamingAssetsPath+"/db/tk.zip";
        string dbZipPath = Path.Combine(Common.dbPath, "tk.zip");
        Debugger.Log(path);
        if (path.Contains("://")) {
            WWW www = new WWW(path);
            yield return www;
            System.IO.File.WriteAllBytes(dbZipPath, www.bytes);
            istkDbDownload = true;
        }
        else {
            System.IO.File.WriteAllBytes(dbZipPath, System.IO.File.ReadAllBytes(path));
            istkDbDownload = true;
        }
        //解压db
        DBHelper.Instance.ExtractDb(dbZipPath);
    }

    IEnumerator DownloadLocalPlayerInfoDbDB() {
        string path = Application.streamingAssetsPath + "/db/LocalPlayerInfo.zip";
        string dbZipPath = Path.Combine(Common.dbPath, "LocalPlayerInfo.zip");
        if (path.Contains("://")) {
            WWW www = new WWW(path);
            yield return www;
            System.IO.File.WriteAllBytes(dbZipPath, www.bytes);
            islocalPlayerInfoDb = true;
        }
        else {
            System.IO.File.WriteAllBytes(dbZipPath, System.IO.File.ReadAllBytes(path));
            islocalPlayerInfoDb = true;
        }
        //解压db
        DBHelper.Instance.ExtractDb(dbZipPath);
    }

    void OnDestory() {
        DBHelper.Instance.CloseConnection();
    }

    void OnApplicationQuit() {
        DBHelper.Instance.CloseConnection();
    }

}
