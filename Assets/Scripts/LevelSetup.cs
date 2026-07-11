using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class LevelSetup
{
    private const float WorldHalfWidth = 8.5f;
    private const float WorldHalfHeight = 4.8f;

    private static Sprite squareSprite;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void BuildGame()
    {
        if (Object.FindObjectOfType<PlayerController>() != null)
        {
            return;
        }

        Time.timeScale = 1f;
        squareSprite = CreateSquareSprite();

        GameObject root = new GameObject("GameRoot");

        Camera camera = EnsureCamera(root.transform);
        CreateBackground(root.transform);
        CreateWalls(root.transform);

        UiBundle ui = CreateUi(root.transform);

        GameObject managerObject = new GameObject("GameManager");
        managerObject.transform.SetParent(root.transform);
        GameManager manager = managerObject.AddComponent<GameManager>();
        manager.Configure(ui.ScoreText, ui.EndPanel, ui.EndTitle, ui.EndBody, ui.RestartButton);

        CreatePlayer(root.transform);
        CreateCollectibles(root.transform, manager);
        CreateEnemies(root.transform);

        camera.transform.position = new Vector3(0f, 0f, -10f);
    }

    private static Camera EnsureCamera(Transform parent)
    {
        Camera camera = Object.FindObjectOfType<Camera>();
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            camera = cameraObject.AddComponent<Camera>();
        }

        camera.transform.SetParent(parent);
        camera.orthographic = true;
        camera.orthographicSize = 5.4f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.07f, 0.08f, 0.11f);

        if (camera.GetComponent<AudioListener>() == null)
        {
            camera.gameObject.AddComponent<AudioListener>();
        }

        return camera;
    }

    private static void CreateBackground(Transform parent)
    {
        GameObject background = CreateSpriteObject("Background", parent, Vector2.zero, new Color(0.12f, 0.15f, 0.20f));
        background.transform.localScale = new Vector3(18f, 10.5f, 1f);
        background.GetComponent<SpriteRenderer>().sortingOrder = -10;

        for (int x = -8; x <= 8; x += 2)
        {
            GameObject line = CreateSpriteObject($"GridV{x}", parent, new Vector2(x, 0f), new Color(0.20f, 0.24f, 0.30f, 0.45f));
            line.transform.localScale = new Vector3(0.025f, 9.7f, 1f);
            line.GetComponent<SpriteRenderer>().sortingOrder = -9;
        }

        for (int y = -4; y <= 4; y += 2)
        {
            GameObject line = CreateSpriteObject($"GridH{y}", parent, new Vector2(0f, y), new Color(0.20f, 0.24f, 0.30f, 0.45f));
            line.transform.localScale = new Vector3(17f, 0.025f, 1f);
            line.GetComponent<SpriteRenderer>().sortingOrder = -9;
        }
    }

    private static void CreateWalls(Transform parent)
    {
        CreateWall("WallTop", parent, new Vector2(0f, WorldHalfHeight + 0.35f), new Vector2(WorldHalfWidth * 2f + 1f, 0.7f));
        CreateWall("WallBottom", parent, new Vector2(0f, -WorldHalfHeight - 0.35f), new Vector2(WorldHalfWidth * 2f + 1f, 0.7f));
        CreateWall("WallLeft", parent, new Vector2(-WorldHalfWidth - 0.35f, 0f), new Vector2(0.7f, WorldHalfHeight * 2f + 1f));
        CreateWall("WallRight", parent, new Vector2(WorldHalfWidth + 0.35f, 0f), new Vector2(0.7f, WorldHalfHeight * 2f + 1f));
    }

    private static void CreateWall(string name, Transform parent, Vector2 position, Vector2 size)
    {
        GameObject wall = CreateSpriteObject(name, parent, position, new Color(0.28f, 0.33f, 0.42f));
        wall.transform.localScale = new Vector3(size.x, size.y, 1f);
        wall.AddComponent<BoxCollider2D>();
    }

    private static void CreatePlayer(Transform parent)
    {
        GameObject player = CreateSpriteObject("Player", parent, Vector2.zero, new Color(0.25f, 0.75f, 1f));
        player.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        player.AddComponent<Rigidbody2D>();
        player.AddComponent<BoxCollider2D>();
        player.AddComponent<PlayerController>();
    }

    private static void CreateCollectibles(Transform parent, GameManager manager)
    {
        Vector2[] positions =
        {
            new Vector2(-6.5f, 3.2f),
            new Vector2(-2.8f, 2.3f),
            new Vector2(2.6f, 3.1f),
            new Vector2(6.3f, 1.4f),
            new Vector2(4.8f, -2.9f),
            new Vector2(0.2f, -3.4f),
            new Vector2(-4.7f, -2.3f),
            new Vector2(-7.0f, -0.4f)
        };

        foreach (Vector2 position in positions)
        {
            GameObject gem = CreateSpriteObject("Gem", parent, position, new Color(1f, 0.86f, 0.18f));
            gem.transform.localScale = new Vector3(0.42f, 0.42f, 1f);
            gem.AddComponent<CircleCollider2D>();
            gem.AddComponent<Collectible>();
            manager.RegisterCollectible();
        }
    }

    private static void CreateEnemies(Transform parent)
    {
        CreateEnemy(parent, "EnemyHorizontal", new Vector2(-5.2f, 0.9f), new Vector2(5.2f, 0.9f), 2.7f);
        CreateEnemy(parent, "EnemyVertical", new Vector2(2.2f, -3.2f), new Vector2(2.2f, 3.4f), 2.2f);
    }

    private static void CreateEnemy(Transform parent, string name, Vector2 pointA, Vector2 pointB, float speed)
    {
        GameObject pathA = new GameObject($"{name}_PointA");
        pathA.transform.SetParent(parent);
        pathA.transform.position = pointA;

        GameObject pathB = new GameObject($"{name}_PointB");
        pathB.transform.SetParent(parent);
        pathB.transform.position = pointB;

        GameObject enemy = CreateSpriteObject(name, parent, pointA, new Color(1f, 0.25f, 0.25f));
        enemy.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        enemy.AddComponent<Rigidbody2D>();
        enemy.AddComponent<BoxCollider2D>();
        SimpleEnemy simpleEnemy = enemy.AddComponent<SimpleEnemy>();
        simpleEnemy.Configure(pathA.transform, pathB.transform, speed);
    }

    private static UiBundle CreateUi(Transform parent)
    {
        Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        GameObject canvasObject = new GameObject("Canvas");
        canvasObject.transform.SetParent(parent);
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObject.AddComponent<GraphicRaycaster>();
        EnsureEventSystem(canvasObject.transform);

        Text scoreText = CreateText("ScoreText", canvasObject.transform, font, 28, TextAnchor.UpperLeft);
        RectTransform scoreRect = scoreText.rectTransform;
        scoreRect.anchorMin = new Vector2(0f, 1f);
        scoreRect.anchorMax = new Vector2(0f, 1f);
        scoreRect.pivot = new Vector2(0f, 1f);
        scoreRect.anchoredPosition = new Vector2(22f, -18f);
        scoreRect.sizeDelta = new Vector2(360f, 60f);

        GameObject endPanel = new GameObject("EndPanel");
        endPanel.transform.SetParent(canvasObject.transform);
        Image panelImage = endPanel.AddComponent<Image>();
        panelImage.color = new Color(0.02f, 0.025f, 0.035f, 0.86f);
        RectTransform panelRect = endPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 0f);
        panelRect.anchorMax = new Vector2(1f, 1f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Text title = CreateText("EndTitle", endPanel.transform, font, 54, TextAnchor.MiddleCenter);
        title.rectTransform.anchoredPosition = new Vector2(0f, 80f);
        title.rectTransform.sizeDelta = new Vector2(640f, 80f);

        Text body = CreateText("EndBody", endPanel.transform, font, 24, TextAnchor.MiddleCenter);
        body.rectTransform.anchoredPosition = new Vector2(0f, 18f);
        body.rectTransform.sizeDelta = new Vector2(700f, 60f);

        Button restartButton = CreateButton("RestartButton", endPanel.transform, font);
        restartButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -70f);

        endPanel.SetActive(false);

        return new UiBundle(scoreText, endPanel, title, body, restartButton);
    }

    private static void EnsureEventSystem(Transform parent)
    {
        if (Object.FindObjectOfType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.transform.SetParent(parent);
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
    }

    private static Text CreateText(string name, Transform parent, Font font, int size, TextAnchor anchor)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent);
        Text text = textObject.AddComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.alignment = anchor;
        text.color = Color.white;
        text.raycastTarget = false;
        return text;
    }

    private static Button CreateButton(string name, Transform parent, Font font)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent);
        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.22f, 0.55f, 0.92f);
        Button button = buttonObject.AddComponent<Button>();

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(220f, 56f);

        Text label = CreateText("Label", buttonObject.transform, font, 25, TextAnchor.MiddleCenter);
        label.text = "Restart";
        RectTransform labelRect = label.rectTransform;
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        return button;
    }

    private static GameObject CreateSpriteObject(string name, Transform parent, Vector2 position, Color color)
    {
        GameObject spriteObject = new GameObject(name);
        spriteObject.transform.SetParent(parent);
        spriteObject.transform.position = position;

        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sprite = squareSprite;
        renderer.color = color;

        return spriteObject;
    }

    private static Sprite CreateSquareSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
    }

    private struct UiBundle
    {
        public UiBundle(Text scoreText, GameObject endPanel, Text endTitle, Text endBody, Button restartButton)
        {
            ScoreText = scoreText;
            EndPanel = endPanel;
            EndTitle = endTitle;
            EndBody = endBody;
            RestartButton = restartButton;
        }

        public Text ScoreText { get; }
        public GameObject EndPanel { get; }
        public Text EndTitle { get; }
        public Text EndBody { get; }
        public Button RestartButton { get; }
    }
}
