using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] RectTransform levelCompleteLabel;
    [SerializeField] Image skinIcon;
    [SerializeField] Image skinFillProgress;
    [SerializeField] RectTransform tapToNext;
    //[SerializeField] TMP_Text skinProgressLabel;

    // Start is called before the first frame update
    void OnEnable()
    {
        levelCompleteLabel.localScale = Vector3.zero;
        tapToNext.localScale = Vector3.zero;

        KnifeSkin skin = FindObjectOfType<KnifeStorage>().GetNextSkin();

        if (skin != null)
        {
            skinFillProgress.sprite = skin.Icon;
            skinIcon.sprite = skin.IconBack;
        }
        else
        {
            skinIcon.enabled = false;
            skinFillProgress.enabled = false;
        }
        FindObjectOfType<KnifeStorage>().AddSkinProgress(out float oldProgress, out float newProgress, out bool newSkin);
        if(newSkin)
        {
            newProgress = 1f;
        }
        Sequence sequence = DOTween.Sequence();
        skinFillProgress.fillAmount = oldProgress;
        sequence.Append(levelCompleteLabel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.3f, 0.3f));
        sequence.Append(skinIcon.transform.DOScale(1.2f, 0.3f).SetEase(Ease.InOutCirc));
        sequence.Append(skinFillProgress.DOFillAmount(newProgress, 0.5f).SetEase(Ease.InOutCubic));
        sequence.Append(skinIcon.transform.DOScale(1f, 0.4f).SetEase(Ease.InOutCirc));
        sequence.Append(tapToNext.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.3f, 0.3f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
