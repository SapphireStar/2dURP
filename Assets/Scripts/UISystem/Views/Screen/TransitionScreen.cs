using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField]
    private Image m_background;
    [SerializeField]
    private float m_transitionTime;

    private CancellationTokenSource m_tokenSource;
    private void Start()
    {
        m_transitionTime = 0.3f;
        m_background = GetComponent<Image>();
        m_tokenSource = new CancellationTokenSource();
    }

    public async UniTask TransitionEnter()
    {
        m_tokenSource.Cancel();
        m_tokenSource.Dispose();
        m_tokenSource = new CancellationTokenSource();
        await SetAlphaSmoothly(1, m_transitionTime);
    }

    public async UniTask TransitionOut()
    {
        m_tokenSource.Cancel();
        m_tokenSource.Dispose();
        m_tokenSource = new CancellationTokenSource();
        await SetAlphaSmoothly(0, m_transitionTime);
    }
    public void StopTransition()
    {
        m_tokenSource.Cancel();
        m_tokenSource.Dispose();
        m_tokenSource = new CancellationTokenSource();
    }

    public void SetTransitionTime(float time)
    {
        m_transitionTime = time;
    }

    async UniTask SetAlphaSmoothly(float target, float transitionTime)
    {
        float curAlpha = m_background.color.a;
        float gap = target - curAlpha;
        float delta = gap / transitionTime;
        float startTime = 0;
        while (startTime <= transitionTime)
        {
            float elpsedAlpha =curAlpha + startTime * delta;
            m_background.color = new Color(0, 0, 0, elpsedAlpha);
            await UniTask.Yield(m_tokenSource.Token);
            startTime += Time.deltaTime;
        }
        m_background.color = new Color(0, 0, 0, target);
    }
    private void OnDestroy()
    {
        m_tokenSource.Cancel();
        m_tokenSource.Dispose();
    }
}
