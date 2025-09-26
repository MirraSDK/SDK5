using MirraGames.SDK.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    [RequireComponent(typeof(UIDocument))]
    internal class FeaturesView : MonoBehaviour {

        private readonly WaitForSecondsRealtime waitForOneSecond = new(1.0f);

        private VisualElement viewportElement;

        private void OnEnable() {
            MirraSDK.WaitForProviders(() => {
                CreateViewport();
                StartCoroutine(UpdateViewportCoroutine());
            });
        }

        private void CreateViewport() {
            UIDocument document = gameObject.GetComponent<UIDocument>();
            IEventAggregator eventAggregator = new EventAggregator();
            VisualTreeAsset featuresViewAsset = VisualTreeReference.Load(nameof(FeaturesView)).VisualTree;
            viewportElement = featuresViewAsset.Instantiate();
            viewportElement.style.flexGrow = 1;
            document.rootVisualElement.Add(viewportElement);
        }

        private IEnumerator UpdateViewportCoroutine() {
            while (true) {
                yield return waitForOneSecond;
                UpdateViewportValues();
            }
        }

        private void UpdateViewportValues() {
            FeaturesContainer[] featuresContainers = viewportElement.Query<FeaturesContainer>().ToList().ToArray();
            foreach (FeaturesContainer featuresContainer in featuresContainers) {
                featuresContainer.UpdateValues();
            }
        }

    }

}