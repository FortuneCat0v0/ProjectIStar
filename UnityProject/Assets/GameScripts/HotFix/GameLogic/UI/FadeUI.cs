using DG.Tweening;
using UnityEngine;

namespace GameLogic
{
    [Window(UILayer.Top, hideTimeToClose: 0)]
    public partial class FadeUI
    {
        private const float FadeDuration = 0.35f;
        private const float BlackScreenDuration = 0.15f;

        private Sequence _fadeSequence;

        protected override void OnCreate()
        {
            m_imgBlack.color = new Color(
                m_imgBlack.color.r,
                m_imgBlack.color.g,
                m_imgBlack.color.b,
                0f);

            _fadeSequence = DOTween.Sequence()
                .SetUpdate(true)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .Append(m_imgBlack.DOFade(1f, FadeDuration).SetEase(Ease.InOutSine))
                .AppendCallback(DaySettlement.Settle)
                .AppendInterval(BlackScreenDuration)
                .Append(m_imgBlack.DOFade(0f, FadeDuration).SetEase(Ease.InOutSine))
                .OnComplete(() =>
                {
                    _fadeSequence = null;
                    Close();
                });
        }

        protected override void OnDestroy()
        {
            _fadeSequence?.Kill();
            _fadeSequence = null;
        }
    }
}
