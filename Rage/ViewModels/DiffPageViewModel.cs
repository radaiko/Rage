namespace Rage.ViewModels
{

    public class DiffPageViewModel : ViewModelBase
    {
        public string DiffConsole { get; set; }

        public DiffPageViewModel(string inputDiff)
        {
            this.DiffConsole = inputDiff;
        }
    }
}