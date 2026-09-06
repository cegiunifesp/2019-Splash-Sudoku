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

    private Coroutine m_TutorialCoroutine;

    private void Awake()
    {
        ResetCanvases();
    }

    private void ResetCanvases()
    {
        SetCanvasGroupState(pickCanvasGroup, 0f, false, false);
        SetCanvasGroupState(paintCanvasGroup01, 0f, false, false);
        SetCanvasGroupState(paintCanvasGroup02, 0f, false, false);
        SetCanvasGroupState(finalInstruction, 0f, false, false);

        pickedColor = false;
        painted = false;
    }

    private void SetCanvasGroupState(CanvasGroup group, float alpha, bool interactable, bool blocksRaycasts)
    {
        if (group != null)
        {
            group.alpha = alpha;
            group.interactable = interactable;
            group.blocksRaycasts = blocksRaycasts;
        }
    }

    public void StartTutorial()
    {
        if (m_ShowTutorial && m_TutorialCoroutine == null)
        {
            m_TutorialCoroutine = StartCoroutine(TutorialSteps());
        }
    }

    public IEnumerator TutorialSteps()
    {
        yield return new WaitForSeconds(1.8f);

        // Force reset triggers to ignore initial setup
        pickedColor = false; 
        painted = false;

        // Steps 1 & 2 (Floating hints - do not block clicks)
        yield return StartCoroutine(Fade(1f, pickCanvasGroup));

        if (m_ColorPickers != null)
        {
            foreach (Animator anim in m_ColorPickers)
            {
                if (anim != null)
                    anim.SetBool("Tutorial", true);
            }
        }

        while (!pickedColor)
            yield return null;

        if (m_ColorPickers != null)
        {
            foreach (Animator anim in m_ColorPickers)
            {
                if (anim != null)
                    anim.SetBool("Tutorial", false);
            }
        }

        StartCoroutine(Fade(0f, pickCanvasGroup));
        StartCoroutine(Fade(1f, paintCanvasGroup01));
        yield return StartCoroutine(Fade(1f, paintCanvasGroup02));

        while (!painted)
            yield return null;

        StartCoroutine(Fade(0f, paintCanvasGroup01));
        StartCoroutine(Fade(0f, paintCanvasGroup02));

        yield return new WaitForSeconds(1f);

        // Step 3 (Rules panel - blocks background and enables button)
        if (finalInstruction != null)
        {
            finalInstruction.blocksRaycasts = true;
            finalInstruction.interactable = true;
        }
        yield return StartCoroutine(Fade(1f, finalInstruction));

        m_TutorialCoroutine = null;
    }

    public IEnumerator Fade(float targetAlpha, CanvasGroup group)
    {
        if (group == null)
            yield break;

        float effectiveDuration = Mathf.Max(0.01f, fadeDuration);
        float step = Mathf.Abs(targetAlpha - group.alpha) / effectiveDuration;

        while (!Mathf.Approximately(group.alpha, targetAlpha))
        {
            group.alpha = Mathf.MoveTowards(group.alpha, targetAlpha, step * Time.deltaTime);
            yield return null;
        }

        group.alpha = targetAlpha;

        if (targetAlpha == 0f)
        {
            group.blocksRaycasts = false;
            group.interactable = false;
        }
    }

    public void FinishTutorial()
    {
        if (m_TutorialCoroutine != null)
        {
            StopCoroutine(m_TutorialCoroutine);
            m_TutorialCoroutine = null;
        }

        StartCoroutine(Fade(0f, finalInstruction));
        m_ShowTutorial = false;
    }

    public void StopTutorial()
    {
        if (m_TutorialCoroutine != null)
        {
            StopCoroutine(m_TutorialCoroutine);
            m_TutorialCoroutine = null;
        }

        StopAllCoroutines();
        ResetCanvases();
    }
}
