using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEditor;

/// <summary>
/// 资源检查
/// </summary>
public class EditorResourceCheck : EditorWindow
{
    Vector3 scrollPos = Vector3.zero;             // 滚动视图当前显示位置
    static string builtin = "builtin";            // 内置资源名称子串
    static string replaceSpritePath = "Assets/Textures/apple.png";  // 替换图的路径
    static Sprite whiteSprite;                    // 替换图的实例
    static int nodeWidth = 200;                   // 节点宽度
    static List<Node> res = new List<Node>();     // 节点列表
    static List<string> atlasPack = new List<string>();    // 图集打包文件夹名称列表
    static Dictionary<string, List<string>> sameNameDict = new Dictionary<string, List<string>>();

    /// <summary>
    /// 打开检查窗口
    /// </summary>
    [MenuItem("检查工具/UI 图片源检查")]
    public static void OpenCheckImageWindow()
    {
        // 创建窗口对象
        EditorResourceCheck window = GetWindow<EditorResourceCheck>();
        // 设置窗口标题，最小尺寸，位置
        window.titleContent = new GUIContent("UI 图片源检查");
        window.minSize = new Vector2(350f, 400f);
        window.position = new Rect(100f, 100f, 600f, 700f);
        // 显示窗口
        window.Show();
    }

    /// <summary>
    /// 绘制窗口
    /// </summary>
    void OnGUI()
    {
        // 按钮样式
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14;
        buttonStyle.margin.left = 20;
        buttonStyle.margin.right = 20;
        buttonStyle.margin.top = 20;
        buttonStyle.margin.bottom = 20;

        // 文本样式
        var labelStyle = new GUIStyle(EditorStyles.boldLabel);
        labelStyle.fontSize = 14;
        labelStyle.margin.left = 20;
        labelStyle.margin.right = 20;
        labelStyle.margin.top = 20;
        labelStyle.margin.bottom = 20;

        // 文本输入框样式
        var textFieldStyle = new GUIStyle(EditorStyles.toolbarTextField);
        textFieldStyle.fontSize = 14;
        textFieldStyle.margin.left = 20;
        textFieldStyle.margin.right = 20;
        textFieldStyle.margin.top = 20;
        textFieldStyle.margin.bottom = 20;
        textFieldStyle.fixedHeight = 20f;

        // 滚动视图样式
        var scrollViewStyle = new GUIStyle(GUI.skin.scrollView);
        scrollViewStyle.fontSize = 14;
        scrollViewStyle.margin.left = 20;
        scrollViewStyle.margin.right = 20;
        scrollViewStyle.margin.top = 20;
        scrollViewStyle.margin.bottom = 20;

        // 绘制按钮
        if (GUILayout.Button("检查 Assets 里的所有 prefabs 的 Image 组件是否使用 None 图片", buttonStyle, GUILayout.Height(40)))
        {
            res.Clear();
            CheckNoneSprite();
        }

        if (GUILayout.Button("检查 Assets 里的所有 prefabs 的 Image 组件是否使用 unity 内置 图片", buttonStyle, GUILayout.Height(40)))
        {
            res.Clear();
            CheckBuildinResource();
        }

        if (GUILayout.Button("检查 GameUI 场景里的所有 Image 组件是否使用 非图集 图片", buttonStyle, GUILayout.Height(40)))
        {
            res.Clear();
            FindAtlas();
            CheckGameUINotAtlasSprite();
        }

        if (GUILayout.Button("检查 Assets 里的所有 material 使用的 shader", buttonStyle, GUILayout.Height(40)))
        {
            res.Clear();
            CheckShader();
        }

        if (GUILayout.Button("检查 Assets 里的重复名称美术资源", buttonStyle, GUILayout.Height(40)))
        {
            res.Clear();
            CheckSameNameRes();
        }

        if (GUILayout.Button("替换图片", buttonStyle, GUILayout.Height(40)))
        {
            ReplaceSprite();
        }

        if (GUILayout.Button("清空列表", buttonStyle, GUILayout.Height(40)))
        {
            ClearRes();
        }

        // 绘制文本输入框
        GUILayout.Space(10);
        replaceSpritePath = EditorGUILayout.TextField("替换图的路径", replaceSpritePath, textFieldStyle, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth));
        GUILayout.Space(10);

        // 绘制文本
        GUILayout.Space(10);
        GUILayout.Label("搜索结果列表", labelStyle);
        GUILayout.Space(10);

        // 绘制滚动视图
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, scrollViewStyle);
        EditorGUILayout.BeginVertical();

        // 绘制查询到的预制体和图片组件节点
        foreach (var item in res)
        {
            EditorGUILayout.BeginHorizontal();
            DrawNextNode(item);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 检查 Assets 里的所有 prefabs 的 Image 组件是否使用 None 图片
    /// </summary>
    static void CheckNoneSprite()
    {
        // 查找路径，Assets 下的所有文件
        string path = "Assets/";
        // 查找所有 .prefab 文件，存储文件路径
        var allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("prefab")).ToArray();
        // 遍历所有 prefab 的路径列表
        foreach (var item in allfiles)
        {
            // 通过资源路径加载 GameObject
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(item);
            if (go)
            {
                // 获取 Image 数组（包括未启用的），预防多个 Image 的情况
                Image[] images = go.GetComponentsInChildren<Image>(true);
                // 遍历该预制体上的所有 Image 组件
                foreach (var img in images)
                {
                    // 获取图片源路径
                    string spritePath = AssetDatabase.GetAssetPath(img.sprite);
                    if (spritePath.Equals(string.Empty))
                    {
                        Node node = new Node(go, "prefab");
                        node.Add(img, "image").Add(img.sprite, "sprite");
                        res.Add(node);
                    }
                }
            }
        }

        nodeWidth = 200;

        if (res.Count == 0)
        {
            EditorUtility.DisplayDialog("温馨提示", "没有符合条件的图片", "okk");
        }
    }

    /// <summary>
    /// 检查 Assets 里的所有 prefabs 的 Image 组件是否使用 unity 内置 图片
    /// </summary>
    static void CheckBuildinResource()
    {
        // 查找路径，Assets 下的所有文件
        string path = "Assets/";
        // 查找所有 .prefab 文件，存储文件路径
        var allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("prefab")).ToArray();
        // 遍历所有 prefab 的路径列表
        foreach (var item in allfiles)
        {
            // 通过资源路径加载 GameObject
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(item);
            if (go)
            {
                // 获取 Image 数组（包括未启用的），预防多个 Image 的情况
                Image[] images = go.GetComponentsInChildren<Image>(true);
                // 遍历该预制体上的所有 Image 组件
                foreach (var img in images)
                {
                    // 获取图片源路径
                    string spritePath = AssetDatabase.GetAssetPath(img.sprite);
                    if (spritePath.Contains(builtin))
                    {
                        Node node = new Node(go, "prefab");
                        node.Add(img, "image").Add(img.sprite, "sprite");
                        res.Add(node);
                    }
                }
            }
        }

        nodeWidth = 200;

        if (res.Count == 0)
        {
            EditorUtility.DisplayDialog("温馨提示", "没有符合条件的图片", "okk");
        }
    }

    /// <summary>
    /// 检查 GameUI 场景里的所有 Image 组件是否使用 非图集 图片
    /// </summary>
    static void CheckGameUINotAtlasSprite()
    {
        var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var previousSelection = Selection.objects;
        Selection.objects = allGos;
        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;
        foreach (var trans in selectedTransforms)
        {
            // 获取 Image 数组（包括未启用的），预防多个 Image 的情况
            Image[] images = trans.GetComponentsInChildren<Image>(true);
            // 遍历该预制体上的所有 Image 组件
            foreach (var img in images)
            {
                // 获取图片源路径
                string spritePath = AssetDatabase.GetAssetPath(img.sprite);
                // 是否包含在图集文件夹内
                bool isIncludeAtlas = false;
                foreach (var item in atlasPack)
                {
                    if (spritePath.Contains(item))
                    {
                        isIncludeAtlas = true;
                    }
                }
                // 非图集文件夹内的图片
                if (!isIncludeAtlas)
                {
                    Node node = new Node(trans.gameObject, "gameobject");
                    node.Add(img, "image").Add(img.sprite, "sprite");
                    res.Add(node);
                }
            }
        }

        nodeWidth = 200;

        if (res.Count == 0)
        {
            EditorUtility.DisplayDialog("温馨提示", "没有符合条件的图片", "okk");
        }
    }

    /// <summary>
    /// 检查 Assets 里的所有 material 使用的 shader
    /// </summary>
    static void CheckShader()
    {
        // 查找路径，Assets 下的所有文件
        string path = "Assets/";
        // 查找所有 .prefab 文件，存储文件路径
        var allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("mat")).ToArray();
        // 遍历所有 prefab 的路径列表
        foreach (var item in allfiles)
        {
            // 通过资源路径加载 GameObject
            Material mt = AssetDatabase.LoadAssetAtPath<Material>(item);
            if (mt)
            {
                // 获取 shader 源路径
                string shaderPath = AssetDatabase.GetAssetPath(mt.shader);
                Node node = new Node(mt, "material");
                node.Add(mt.shader, "shader");
                res.Add(node);
            }
        }

        nodeWidth = 400;

        if (res.Count == 0)
        {
            EditorUtility.DisplayDialog("温馨提示", "没有 shader", "okk");
        }
    }

    /// <summary>
    /// 查找图集
    /// </summary>
    static void FindAtlas()
    {
        // 查找路径，Assets 下的所有文件
        string path = "Assets/";
        // 查找所有 .prefab 文件，存储文件路径
        var allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("spriteatlas")).ToArray();
        // 遍历所有 prefab 的路径列表
        foreach (var item in allfiles)
        {
            // 通过资源路径加载 GameObject
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(item);
            if (atlas)
            {
                // 图集打包的文件夹
                Object[] objects = UnityEditor.U2D.SpriteAtlasExtensions.GetPackables(atlas);
                foreach (var objectItem in objects)
                {
                    atlasPack.Add(objectItem.name);
                }
            }
        }
    }

    /// <summary>
    /// 检查 Assets 里的重复名称美术资源
    /// </summary>
    static void CheckSameNameRes()
    {
        // 查找路径，Assets 下的所有文件
        string path = "Assets/";
        // 查找所有美术资源文件，存储文件路径
        var allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("png") || s.EndsWith("jpg") || s.EndsWith("prefab") || s.EndsWith("mat") || s.EndsWith("shader")).ToArray();
        sameNameDict.Clear();
        // 遍历所有美术资源的路径列表
        foreach (var item in allfiles)
        {
            UnityEngine.Object UObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item);
            if (UObject)
            {
                if (sameNameDict.ContainsKey(UObject.name))
                {
                    sameNameDict[UObject.name].Add(item);
                }
                else
                {
                    List<string> pathList = new List<string>();
                    pathList.Add(item);
                    sameNameDict.Add(UObject.name, pathList);
                }
            }
        }
        // 遍历相同名称资源的字典列表
        foreach (var item in sameNameDict)
        {
            // 列表长度大于 1，说明有资源的名称重复了
            if (item.Value.Count > 1)
            {
                // 遍历列表存储的资源路径
                foreach (var item2 in item.Value)
                {
                    UnityEngine.Object UObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item2);
                    if (UObject)
                    {
                        Node node = new Node(UObject, "UObject");
                        res.Add(node);
                    }
                }
            }
        }

        nodeWidth = 400;

        if (res.Count == 0)
        {
            EditorUtility.DisplayDialog("温馨提示", "没有重复名称的美术资源", "okk");
        }
    }

    /// <summary>
    /// 递归绘制节点
    /// </summary>
    /// <param name="node">节点</param>
    static void DrawNextNode(Node node)
    {
        EditorGUILayout.ObjectField(node.data, node.data.GetType(), true, GUILayout.Width(nodeWidth));
        if (node.next != null)
        {
            DrawNextNode(node.next);
        }
    }

    /// <summary>
    /// 替换图片
    /// </summary>
    static void ReplaceSprite()
    {
        if (res.Count == 0)
        {
            EditorUtility.DisplayDialog("温馨提示", "当前节点列表为空", "okk");
            return;
        }
        if (!File.Exists(replaceSpritePath))
        {
            EditorUtility.DisplayDialog("温馨提示", "替换图不存在，请检查替换图的路径", "okk");
            return;
        }
        // 加载替换后的图片
        whiteSprite = AssetDatabase.LoadAssetAtPath<Sprite>(replaceSpritePath);
        // 遍历查找出来的节点列表
        foreach (var item in res)
        {
            ReplaceNextNode(item);
        }

        EditorUtility.DisplayDialog("温馨提示", "替换完成", "okk");
    }

    /// <summary>
    /// 递归操作节点
    /// </summary>
    /// <param name="node">节点</param>
    static void ReplaceNextNode(Node node)
    {
        // Image 组件
        if (node.type.Equals("image"))
        {
            Image img = node.data as Image;
            // 替换图片
            img.sprite = whiteSprite;
            img.type = Image.Type.Simple;
        }
        if (node.next != null)
        {
            ReplaceNextNode(node.next);
        }
        // GameObject
        if (node.type.Equals("prefab"))
        {
            // 保存 prefab 的修改
            GameObject prefab = node.data as GameObject;
            PrefabUtility.SavePrefabAsset(prefab);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 清空节点列表
    /// </summary>
    static void ClearRes()
    {
        res.Clear();
    }
}

/// <summary>
/// 链表节点
/// </summary>
public class Node
{
    public UnityEngine.Object data;    // 链表数据
    public string type;                // 链表类型
    public Node next;                  // 链表指针

    public Node(UnityEngine.Object data, string type)
    {
        this.data = data;
        this.type = type;
    }

    /// <summary>
    /// 链表尾插法
    /// </summary>
    /// <param name="data">链表数据</param>
    public Node Add(UnityEngine.Object data, string type)
    {
        Node node = new Node(data, type);
        this.next = node;
        return node;
    }
}