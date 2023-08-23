using Prism.Regions;
using Reactive.Bindings;

namespace WizardSample.ViewModels
{
    public class Page2ViewModel : PageViewModelBase
    {
        /// <summary>
        /// Page上部の説明文
        /// </summary>
        public ReactivePropertySlim<string> Detail { get; } = new(string.Empty);

        #region PageViewModelBase Functions

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var file = navigationContext.Parameters["File"] as string;
            Detail.Value = $"選択されたファイルは {file} です。";
        }

        #endregion PageViewModelBase Functions
    }
}
