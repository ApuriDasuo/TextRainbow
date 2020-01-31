using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextRainbow : MonoBehaviour
{
    // Inspectorで編集する変数
    public bool IsTokeiMawari = true;
    public int ChangeSpeed = 0;
    // 変換管理変数
    public static int _ChangeCnt = 0;
    public static int _NijiStartId = 0;
    // 変換管理定数
    public const string Df_Tag_Hedder = "<color=#Value>";
    public const string Df_Tag_Footer = "</color>";
    // 虹色の中身　※ 2桁づつで区切って[00 00 00]＝[ R, G, B ]の値になっている
    // 　　　　　　※ この配列の要素を追加・編集したらオリジナルの動く色が作れる
    public static string[] Df_ColorTag = new string[] 
    {
        "ff0000",
        "ffff00",
        "00ff00",
        "00ffff",
        "0000ff",
        "ff00ff",
    };

    /// <summary>
    /// 毎割り込みイベント
    /// </summary>
    void Update()
    {
        // 設定したスピードに合わせて色変更処理を呼ぶ
        _ChangeCnt--;
        if (_ChangeCnt <= 0)
        {
            _ChangeCnt = ChangeSpeed;
            SetTextColorChange(IsTokeiMawari, this.GetComponent<Text>());
        }
    }

    /// <summary>
    /// テキスト色を変える処理（毎割り込み）
    /// </summary>
    /// <param name="TxtSet">変更するテキストオブジェクト</param>
    public static void SetTextColorChange(bool IsVecLR,Text TxtSet)
    {
        // テキストの文字を取得※Tag文字を取り除く
        string textNoTag = TxtSet.text;
        textNoTag = textNoTag.Replace(Df_Tag_Footer, "");
        for (int i_ColorId = 0; i_ColorId < Df_ColorTag.Length; i_ColorId++)
        {
            textNoTag = textNoTag.Replace(Df_Tag_Hedder.Replace("Value", Df_ColorTag[i_ColorId]), "");
        }
        // 一文字ずつ色を設定
        int setColorId = _NijiStartId;
        StringBuilder textSet = new StringBuilder();
        for (int i_Word = 0; i_Word < textNoTag.Length; i_Word++)
        {
            textSet.Append(Df_Tag_Hedder.Replace("Value", Df_ColorTag[setColorId]));
            textSet.Append(textNoTag.Substring(i_Word, 1));
            textSet.Append(Df_Tag_Footer);
            if(IsVecLR)
            {
                setColorId--;
                if (setColorId <0)
                {
                    setColorId = Df_ColorTag.Length-1;
                }
            }
            else
            {
                setColorId++;
                if (setColorId >= Df_ColorTag.Length)
                {
                    setColorId = 0;
                }
            }
        }
        // 次回の開始色を更新
        _NijiStartId++;
        if (_NijiStartId >= Df_ColorTag.Length)
        {
            _NijiStartId = 0;
        }
        // テキスト文字を変更
        TxtSet.text = textSet.ToString();
    }
}
