using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SwissWorkSafe.Commands;

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
    /// A base class for ViewModels within the SwissWorkSafe application.
    /// Implements INotifyPropertyChanged for data binding scenarios.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Fields

        private bool _isMenuOpen;
        private object _currentView;

        // Protected navigation action for derived classes
        protected Action<string> _navigateAction;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        public ViewModelBase()
        {
            // Navigation action not set.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class with a navigation action.
        /// </summary>
        /// <param name="navigateAction">The action to invoke when navigating.</param>
        public ViewModelBase(Action<string> navigateAction)
        {
            _navigateAction = navigateAction ?? throw new ArgumentNullException(nameof(navigateAction));
            NavigateCommand = new RelayCommand(param =>
            {
                if (param is string destination && !string.IsNullOrEmpty(destination))
                {
                    _navigateAction(destination);
                }
            });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether a side menu (or popup) is open.
        /// </summary>
        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                if (_isMenuOpen != value)
                {
                    _isMenuOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current active view (UserControl).
        /// </summary>
        public virtual object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to navigate between views.
        /// </summary>
        public ICommand NavigateCommand { get; protected set; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed. Automatically supplied by CallerMemberName.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
