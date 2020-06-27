namespace Rage.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        public MenuViewModel MenuViewModel { get;}
        public MainViewModel MainViewModel { get; }
        public BottomViewModel BottomViewModel {get; set;}

        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel();
            MainViewModel = new MainViewModel();
            BottomViewModel = new BottomViewModel();
            
        }
    }
}
