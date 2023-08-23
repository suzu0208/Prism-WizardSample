using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardSample.ViewModels
{
    public abstract class PageViewModelBase : BindableBase, IConfirmNavigationRequest
    {
        public const ReactivePropertyMode DefaultMode = ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError;

        public const string DirectionKeyword = "Direction";

        public const string PrevKeyword = "Prev";

        public const string NextKeyword = "Next";

        /// <summary>
        /// 別Taskで処理中など、ページ遷移を無効にしたい場合にtrueにします。
        /// </summary>
        public ReactivePropertySlim<bool> IsBusy { get; private set; }

        /// <summary>
        /// Pageの入力内容の評価を行います。
        /// </summary>
        /// <returns>成否</returns>
        public virtual bool Validation() => true;

        #region IConfirmNavigationRequest Functions

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <summary>
        /// 他Pageに遷移するときに呼ばれるメソッド。
        /// </summary>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

        /// <summary>
        /// 他Pageから遷移してきたときに呼ばれるメソッド。
        /// </summary>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsBusy = navigationContext.Parameters[nameof(IsBusy)] as ReactivePropertySlim<bool>;
        }

        /// <summary>
        /// Page遷移する直前に呼ばれるメソッド。
        /// </summary>
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            var direction = navigationContext.Parameters[DirectionKeyword] as string;
            if (direction == PrevKeyword)
                // [前へ]での遷移は無条件で遷移させる。
                continuationCallback(true);
            else
                // [次へ]での遷移は入力内容をチェックする。
                continuationCallback(Validation());
        }

        #endregion
    }
}
