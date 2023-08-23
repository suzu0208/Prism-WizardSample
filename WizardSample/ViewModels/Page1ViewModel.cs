using Microsoft.Win32;
using Prism.Commands;
using Prism.Regions;
using Reactive.Bindings;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace WizardSample.ViewModels
{
    public class Page1ViewModel : PageViewModelBase
    {
        [Required(ErrorMessage = "ファイルパスを指定してください。")]
        [RegularExpression(@"[a-zA-Z]:\\[\w-\\\.]*|(\\\\[\w-\\\.]*|""\\\\[\w-\.].*"")", ErrorMessage = "ファイルパスを指定してください。")]
        public ReactiveProperty<string> FilePath { get; set; }

        #region Command

        public ICommand FileSelectCommand { get; }

        #endregion Command

        public Page1ViewModel()
        {
            FileSelectCommand = new DelegateCommand(OnClickFileSelectButton);

            FilePath = new ReactiveProperty<string>(mode: DefaultMode).SetValidateAttribute(() => FilePath);
        }

        #region PageViewModelBase Functions

        public override bool Validation()
        {
            FilePath.ForceValidate();

            return !FilePath.HasErrors;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("File", FilePath.Value);
        }

        #endregion PageViewModelBase Functions

        private void OnClickFileSelectButton()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
                FilePath.Value = dlg.FileName;
        }
    }
}
