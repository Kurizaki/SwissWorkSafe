using SwissWorkSafe.Commands;
using SwissWorkSafe.Views;

namespace SwissWorkSafe.ViewModels
{
    /*
         $$$$$$\                $$\                     $$\      $$\                     $$\        $$$$$$\             $$$$$$\
        $$  __$$\               \__|                    $$ | $\  $$ |                    $$ |      $$  __$$\           $$  __$$\
        $$ /  \__|$$\  $$\  $$\ $$\  $$$$$$$\  $$$$$$$\ $$ |$$$\ $$ | $$$$$$\   $$$$$$\  $$ |  $$\ $$ /  \__| $$$$$$\  $$ /  \__|$$$$$$\
        \$$$$$$\  $$ | $$ | $$ |$$ |$$  _____|$$  _____|$$ $$ $$\$$ |$$  __$$\ $$  __$$\ $$ | $$  |\$$$$$$\   \____$$\ $$$$\    $$  __$$\
         \____$$\ $$ | $$ | $$ |$$ |\$$$$$$\  \$$$$$$\  $$$$  _$$$$ |$$ /  $$ |$$ |  \__|$$$$$$  /  \____$$\  $$$$$$$ |$$  _|   $$$$$$$$ |
        $$\   $$ |$$ | $$ | $$ |$$ | \____$$\  \____$$\ $$$  / \$$$ |$$ |  $$ |$$ |      $$  _$$<  $$\   $$ |$$  __$$ |$$ |     $$   ____|
        \$$$$$$  |\$$$$$\$$$$  |$$ |$$$$$$$  |$$$$$$$  |$$  /   \$$ |\$$$$$$  |$$ |      $$ | \$$\ \$$$$$$  |\$$$$$$$ |$$ |     \$$$$$$$\
         \______/  \_____\____/ \__|\_______/ \_______/ \__/     \__| \______/ \__|      \__|  \__| \______/  \_______|\__|      \_______|
        Authors: Keanu Koelewijn, Rebecca Wili, Salma Tanner, Lorenzo Lai
    */
    /// <summary>
    /// The MainWindowViewModel is responsible for managing the current view displayed in the MainWindow.
    /// It initializes with the MenuView and can navigate to other views based on user actions.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;

        /// <summary>
        /// Gets or sets the current view displayed in the MainWindow.
        /// This override ensures that the view updates properly when navigated.
        /// </summary>
        public override object CurrentView
        {
            get => base.CurrentView;
            set
            {
                if (base.CurrentView != value)
                {
                    base.CurrentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        /// <summary>
        /// Initializes the MainWindowViewModel. Sets up the navigation logic
        /// and defaults to showing the MenuView.
        /// </summary>
        public MainWindowViewModel()
            : base() // Call the parameterless base constructor
        {
            // Set up the navigation action
            _navigateAction = NavigateTo;

            // Initialize the NavigateCommand with the NavigateTo method
            NavigateCommand = new RelayCommand(param =>
            {
                if (param is string destination && !string.IsNullOrEmpty(destination))
                {
                    _navigateAction(destination);
                }
            });

            // Initialize the application with the MenuView
            CurrentView = new MenuView
            {
                DataContext = new MenuViewModel(NavigateTo)
            };
        }

        /// <summary>
        /// Navigates to a new view based on the destination parameter.
        /// This switches the CurrentView to the requested UserControl
        /// and provides the appropriate ViewModel with a reference to NavigateTo.
        /// </summary>
        /// <param name="destination">The view to navigate to.</param>
        private void NavigateTo(string destination)
        {
            switch (destination)
            {
                case "Termination":
                    CurrentView = new TerminationView
                    {
                        DataContext = new TerminationViewModel(NavigateTo)
                    };
                    break;

                case "SalaryContinuation":
                    CurrentView = new SalaryContinuationView
                    {
                        DataContext = new SalaryContinuationViewModel(NavigateTo)
                    };
                    break;

                case "ArticleSearch":
                    CurrentView = new ArticleSearchView
                    {
                        DataContext = new ArticleSearchViewModel(NavigateTo)
                    };
                    break;

                case "Help":
                    CurrentView = new HelpView
                    {
                        DataContext = new HelpViewModel(NavigateTo)
                    };
                    break;

                case "MainWindow":
                    CurrentView = new MenuView
                    {
                        DataContext = new MenuViewModel(NavigateTo)
                    };
                    break;

                default:
                    // Fallback to MenuView if unrecognized
                    CurrentView = new MenuView
                    {
                        DataContext = new MenuViewModel(NavigateTo)
                    };
                    break;
            }
        }
    }
}
