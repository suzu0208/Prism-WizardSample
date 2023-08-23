using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using WizardSample.Views;

namespace WizardSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public ReactivePropertySlim<string> Title { get; set; } = new("Prism Application");

        /// <summary>
        /// Pageの順番
        /// </summary>
        private List<Type> order = new()
        {
            typeof(Page1),
            typeof(Page2),
            typeof(LastPage)
        };

        #region Button Property

        public ReactivePropertySlim<bool> IsBusy = new(false);

        public ReadOnlyReactiveProperty<bool> CanPrevButton { get; }
        private ReactiveProperty<bool> _canPrevButton = new(false);

        public ReadOnlyReactiveProperty<bool> CanNextButton { get; }
        private ReactiveProperty<bool> _canNextButton { get; } = new(true);

        public ReadOnlyReactiveProperty<bool> CanCloseButton { get; }
        private ReactiveProperty<bool> _canCloseButton { get; } = new(false);

        #endregion

        #region Command

        public ICommand PrevCommand { get; }

        public ICommand NextCommand { get; }

        public ICommand CloseCommand { get; }

        #endregion Command

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            PrevCommand = new DelegateCommand(OnPrevButton);
            NextCommand = new DelegateCommand(OnNextButton);
            CloseCommand = new DelegateCommand<Window>(OnCloseButton);

            // 何かの処理中はPage遷移を禁止させたいので、can-ButtonとIsBusyを監視する。
            CanPrevButton = IsBusy.CombineLatest(_canPrevButton, (isbusy, canButton) => !isbusy && canButton).ToReadOnlyReactiveProperty<bool>();
            CanNextButton = IsBusy.CombineLatest(_canNextButton, (isbusy, canButton) => !isbusy && canButton).ToReadOnlyReactiveProperty<bool>();
            CanCloseButton = IsBusy.CombineLatest(_canCloseButton, (isbusy, canButton) => !isbusy && canButton).ToReadOnlyReactiveProperty<bool>();
        }

        private void OnPrevButton()
        {
            // 表示中のPageを取得
            var page = _regionManager.Regions["ContentRegion"].ActiveViews.First().GetType();

            // 前のPageを取得
            var index = order.IndexOf(page);
            var prevPage = order[index - 1];

            // 前のPageに渡すParameterを設定
            var param = new NavigationParameters();
            param.Add(PageViewModelBase.DirectionKeyword, PageViewModelBase.PrevKeyword);

            _regionManager.RequestNavigate("ContentRegion", prevPage.Name, RequestNavigateCallback, param);
        }

        private void OnNextButton()
        {
            // 表示中のPageを取得
            var page = _regionManager.Regions["ContentRegion"].ActiveViews.First().GetType();

            // 次のPageを取得
            var index = order.IndexOf(page);
            var nextPage = order[index + 1];

            // 次のPageに渡すParameterを設定
            var param = new NavigationParameters();
            param.Add(PageViewModelBase.DirectionKeyword, PageViewModelBase.NextKeyword);
            param.Add(nameof(IsBusy), IsBusy);

            _regionManager.RequestNavigate("ContentRegion", nextPage.Name, RequestNavigateCallback, param);
        }

        private void RequestNavigateCallback(NavigationResult result)
        {
            var page = _regionManager.Regions["ContentRegion"].ActiveViews.First().GetType();
            _canPrevButton.Value = order.IndexOf(page) != 0;
            _canNextButton.Value = order.Last() != page;
            _canCloseButton.Value = order.Last() == page;
        }

        private void OnCloseButton(Window wnd) => wnd?.Close();
    }
}
