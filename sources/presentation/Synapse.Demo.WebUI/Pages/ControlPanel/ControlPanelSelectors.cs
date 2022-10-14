namespace Synapse.Demo.WebUI.Pages.ControlPanel.State
{
    public static class ControlPanelSelectors
    {
        public static IObservable<int> SelectCount(this IFeature<ControlPanelState> feature)
        {
            return feature.Select(featureState =>
                    featureState.Count
                )
                .DistinctUntilChanged();
        }
    }
}
