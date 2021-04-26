using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private bool m_ShowTutorial = true;
    public bool pickedColor = false, painted = false;

    [SerializeField] private List<Animator> m_ColorPickers;
    [SerializeField] private CanvasGroup pickCanvasGroup, paintCanvasGroup01, paintCanvasGroup02, finalInstruction;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        ResetCanvases();
    }

    private void ResetCanvases()
    {
        pickCanvasGroup.alpha = 0;
        paintCanvasGroup01.alpha = 0;
        paintCanvasGroup02.alpha = 0;
        finalInstruction.alpha = 0;

        pickCanvasGroup.blocksRaycasts = false;
        paintCanvasGroup01.blocksRaycasts = false;
        paintCanvasGroup02.blocksRaycasts = false;
        finalInstruction.blocksRaycasts = false;

        pickedColor = false;
        painted = false;
    }

    public void StartTutorial()
    {
        if (m_ShowTutorial)
            StartCoroutine(TutorialSteps());
    }

    public IEnumerator TutorialSteps()
    {
        yield return new WaitForSeconds(1.8f);

        yield return StartCoroutine(Fade(1f, pickCanvasGroup));

        foreach (Animator anim in m_ColorPickers)
            anim.SetBool("Tutorial", true);

        while (!pickedColor)
            yield return null;

        foreach (Animator anim in m_ColorPickers)
            anim.SetBool("Tutorial", false);

        StartCoroutine(Fade(0f, pickCanvasGroup));
        StartCoroutine(Fade(1f, paintCanvasGroup01));
        yield return StartCoroutine(Fade(1f, paintCanvasGroup02));

        while (!painted)
            yield return null;

        StartCoroutine(Fade(0f, paintCanvasGroup01));
        StartCoroutine(Fade(0f, paintCanvasGroup02));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(1f, finalInstruction));
    }

    public IEnumerator Fade(float targetAlpha, CanvasGroup group)
    {
        float step = Mathf.Abs(targetAlpha - group.alpha) / fadeDuration;
        group.blocksRaycasts = true;

        while(!Mathf.Approximately(group.alpha, targetAlpha))
        {
            group.alpha = Mathf.MoveTowards(group.alpha, targetAlpha, step * Time.deltaTime);
            yield return null;
        }

        if (targetAlpha == 0)
            group.blocksRaycasts = false;
    }

    public void FinishTutorial()
    {
        StartCoroutine(Fade(0f, finalInstruction));
        m_ShowTutorial = false;
    }

    public void StopTutorial()
    {
        StopAllCoroutines();
        ResetCanvases();
    }
}
