using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Selection : MonoBehaviour
{
    [MenuItem("Assets/获取所有文字")]
    public static void GetPrefabs()
    {
        var obj = UnityEditor.Selection.GetFiltered<Object>(SelectionMode.DeepAssets);
        StringBuilder sb = new StringBuilder();
        foreach (var item in obj)
        {
            var path = AssetDatabase.GetAssetPath(item);
            if (path.EndsWith(".prefab"))
            {
                sb.Append(GetText(path));
            }
        }

        byte[] content = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        
        using (FileStream fileStream = new FileStream("Alltext.txt",FileMode.Create,FileAccess.ReadWrite))
        {
            fileStream.Write(array: content,offset: 0,count: content.Length);
            fileStream.Flush();
        }
    }

    private static string GetText(string path)
    {
        var gameObject = PrefabUtility.LoadPrefabContents(path);
        StringBuilder sb = new StringBuilder();
        foreach (var item in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (item.GetComponent<Text>())
            {
                var content = item.GetComponent<Text>().text;
                if (String.IsNullOrEmpty(content)) 
                {
                    continue;
                }

                sb.Append(content.Replace("\r", "").
                    Replace("\n", "").Replace(" ", "").Replace("\t",""));
            }
        }
        PrefabUtility.UnloadPrefabContents(gameObject);
        return sb.ToString();
    }
}
