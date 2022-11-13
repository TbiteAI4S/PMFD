using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ScreenShotCapturer : MonoBehaviour
{
    //logテキスト
    [SerializeField]
    Text log;
    //　データの保存先ファイルパス
    private string saveFilePath = "/Projects/ScreenShot";
    //　保存ファイル名
    private string saveFileName = "/screenshot.PNG";

    public void OnScreenShotButtonClicked()
    {
        // スクリーンショットを保存
        CaptureScreenShot();
    }

    // 画面全体のスクリーンショットを保存する
    private void CaptureScreenShot()
    {
        //　スクリーンショットを撮る
        ScreenCapture.CaptureScreenshot(Application.dataPath + saveFilePath + saveFileName);
        log.text = "スクリーンショットを撮りました！\n" + Application.dataPath + saveFilePath + saveFileName + " に保存されました。";
    }

    void Start()
    {
        //　指定したフォルダがない時はAssetsフォルダに保存
        if (!Directory.Exists(Application.dataPath + saveFilePath))
        {
            saveFilePath = "";
        }
    }

}
