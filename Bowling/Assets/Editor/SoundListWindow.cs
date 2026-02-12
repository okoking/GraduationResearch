using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Codice.Client.BaseCommands.Import.Commit;
using static UnityEngine.Splines.SplineInstantiate;

public class SoundListWindow : EditorWindow
{
    private List<SoundList> dialogues;
    private SoundList selected;             //選択しているアセット
    private string search;                  //検索的テキスト
    private string soundSearch;             //検索的テキスト
    private Vector2 listScroll;             //リスト欄のスクロール
    private Vector2 soundScroll;            //サウンドデータのスクロール

    [MenuItem("ツール/サウンドリスト")]
    public static void Open()
    {
        EditorWindow.GetWindow<SoundListWindow>("サウンドリスト");
    }

    void OnEnable()
    {
        Reload();
    }

    void Reload()
    {
        //プロジェクト内のDialogNodeList型のアセットを探してListに変更
        dialogues = AssetDatabase
            .FindAssets("t:SoundList")                             //指定の型のアセットを探す
            .Select(g => AssetDatabase.LoadAssetAtPath<SoundList>( //アセットを読み込む
                AssetDatabase.GUIDToAssetPath(g)))                      //GUID=パスに変更
            .ToList();                                                  //配列をLIstに変更
    }

    void OnGUI()
    {
        EditorGUILayout.Space(5f);

        //横に並べたい項目
        EditorGUILayout.BeginHorizontal();
        {
            //文字列を表示
            EditorGUILayout.LabelField("わくわくサウンドリスト", EditorStyles.boldLabel);

            //余ったスペースを埋める
            GUILayout.FlexibleSpace();

            DrawLineY(2f, 2f);

            //ボタンを表示
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button(text: "リストを更新"))
            {
                Reload();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        DrawLineX(5f, 2f);

        //横に並べたい項目
        EditorGUILayout.BeginHorizontal();
        {
            //左：検索
            DrawDialogueList();
            DrawLineY(2f, 2f);

            //中：入力欄１
            SetDialogNode();
            DrawLineY(2f, 2f);

            //余ったスペースを埋める（左揃え用）
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        //縦に並べたい項目
        EditorGUILayout.BeginVertical();
        {

        }
        EditorGUILayout.EndVertical();

        //余ったスペースを埋める
        GUILayout.FlexibleSpace();
    }

    void DrawDialogueList()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(150));

        //テキストフィールを表示、入力を変換し変数に代入
        EditorGUILayout.LabelField("アセット名で検索");
        search = EditorGUILayout.TextField(search);

        DrawLineX(2f, 2f);

        //スクロールバー
        listScroll = EditorGUILayout.BeginScrollView(listScroll, false, false);

        // 一覧
        foreach (var d in dialogues)
        {
            //検索：大文字小文字関係なく検索
            if (!string.IsNullOrEmpty(search) &&
                !d.name.Contains(search))
                continue;

            if (GUILayout.Button(
                d.name,
                d == selected ? EditorStyles.toolbarButton : GUI.skin.button))
            {
                selected = d;
                soundScroll = Vector2.zero;
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void SetDialogNode()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(500));

        if (selected == null)
        {
            EditorGUILayout.LabelField("アセットを選択してください");
            EditorGUILayout.EndVertical();
            return;
        }

        EditorGUILayout.LabelField(selected.name);

        //テキストフィールを表示、入力を変換し変数に代入
        EditorGUILayout.LabelField("SoundIDで検索");
        soundSearch = EditorGUILayout.TextField(soundSearch);

        DrawLineX(5f, 2f);

        //スクロールバー
        soundScroll = EditorGUILayout.BeginScrollView(soundScroll, false, false);

        for (int i = 0; i < selected.soundList.Count; i++)
        {
            //検索：大文字小文字関係なく検索
            if (!string.IsNullOrEmpty(soundSearch) &&
               !selected.soundList[i].soundID.Contains(soundSearch, StringComparison.OrdinalIgnoreCase))
                continue;

            EditorGUILayout.BeginVertical("box");

            var node = selected.soundList[i];

            // ← ここに ↑ ↓ ボタンを書く
            if (NodeMove(i, selected.soundList))
            {
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                break;
            }

            // ノードの中身入力
            node.soundID = EditorGUILayout.TextField("ID", node.soundID);
            node.audioClip = 
                (AudioClip)EditorGUILayout.ObjectField("サウンドデータ", node.audioClip,typeof(AudioClip),false);
            node.soundVolume = EditorGUILayout.Slider("音量", node.soundVolume, 0f, 1f);
            node.comment = EditorGUILayout.TextField("コメント", node.comment);

            EditorGUILayout.EndVertical();
        }

        //データの増減
        AddAndSubDataList();

        //データを更新
        EditorUtility.SetDirty(selected);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
    //データの増減
    private void AddAndSubDataList()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                selected.soundList.Add(new SoundData());
            }

            GUI.enabled = selected.soundList.Count > 0;
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                if (selected.soundList.Count > 0)
                    selected.soundList.RemoveAt(selected.soundList.Count - 1);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    //テキストデータの移動
    private bool NodeMove<T>(int i, List<T> list)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField($"要素 {i}：{selected.soundList[i].soundID}", EditorStyles.boldLabel);

            GUI.enabled = i > 0;
            if (GUILayout.Button("↑", GUILayout.Width(25)))
            {
                Swap(list, i, i - 1);
                return true;
            }

            GUI.enabled = i < list.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                Swap(list, i, i + 1);
                return true;
            }
            GUI.enabled = true;

            if (GUILayout.Button("×", GUILayout.Width(25)))
            {
                //i番目の要素を削除
                list.RemoveAt(i);
                return true;
            }
        }
        EditorGUILayout.EndHorizontal();
        return false;
    }

    //装飾
    private void DrawLineX(float space, float lineSize, float lineColor = 0.3f)
    {
        EditorGUILayout.Space(space);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, lineSize), new Color(lineColor, lineColor, lineColor));
        EditorGUILayout.Space(space);
    }
    private void DrawLineY(float space, float lineSize, float lineColor = 0.3f)
    {
        EditorGUILayout.Space(space);
        Rect line = EditorGUILayout.GetControlRect(false, GUILayout.Width(lineSize), GUILayout.ExpandHeight(true));
        EditorGUI.DrawRect(line, new Color(lineColor, 0.3f, lineColor));
        EditorGUILayout.Space(space);
    }

    //要素の入れ替え
    void Swap<T>(List<T> list, int a, int b)
    {
        T temp = list[a];
        list[a] = list[b];
        list[b] = temp;

        EditorUtility.SetDirty(selected);
    }
}