using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification Instance { get; private set; }

    [SerializeField] private GameObject notificationLog;
    [SerializeField] private GameObject background;
    [SerializeField] private float displayDuration = 3f; // Adjust duration
    [SerializeField] private Color backgroundColor = new(1, 1, 1, 0.75f); // Light background for better readability
    private Color textColor = Color.black;

    private Queue<string> queueLog = new();
    private bool isNotificating;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    public void SetNotification(string str)
    {
        queueLog.Enqueue(str);
        if (!isNotificating)
        {
            StartCoroutine(NotificationQueue());
        }
    }

    private IEnumerator NotificationQueue()
    {
        isNotificating = true;
        notificationLog.SetActive(true);
        background.SetActive(true);

        var text = notificationLog.GetComponentInChildren<TextMeshProUGUI>();
        var backgroundImage = background.GetComponent<Image>();

        text.color = textColor;
        backgroundImage.color = backgroundColor;

        while (queueLog.Count > 0)
        {
            text.text = queueLog.Dequeue();
            yield return StartCoroutine(FadeInAndOut());
        }

        notificationLog.SetActive(false);
        background.SetActive(false);
        isNotificating = false;
    }

    private IEnumerator FadeInAndOut()
    {
        var elapsedTime = 0f;
        var fadeDuration = 0.5f;

        while (elapsedTime < fadeDuration)
        {
            var alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            SetAlpha(notificationLog, alpha);
            SetAlpha(background, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(notificationLog, 1);
        SetAlpha(background, 1);

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            SetAlpha(notificationLog, alpha);
            SetAlpha(background, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(notificationLog, 0);
        SetAlpha(background, 0);
    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        var text = obj.GetComponentInChildren<TextMeshProUGUI>();
        var localBackground = obj.GetComponent<Image>();

        if (text != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }

        if (localBackground != null)
        {
            localBackground.color = new Color(localBackground.color.r, localBackground.color.g, localBackground.color.b,
                alpha);
        }
    }
}