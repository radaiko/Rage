namespace Rage.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        public MenuPageViewModel MenuPageViewModel { get;}
        public MainPageViewModel MainPageViewModel { get; }
        public BottomPageViewModel BottomPageViewModel {get; set;}

        public MainWindowViewModel()
        {
            MenuPageViewModel = new MenuPageViewModel();
            MainPageViewModel = new MainPageViewModel();
            BottomPageViewModel = new BottomPageViewModel();
            
        }
    }
}
