using System.Collections.ObjectModel;
using System.Windows.Input;
using SwissWorkSafe.Commands;
using SwissWorkSafe.Models.Articles;
using SwissWorkSafe.Models.Core;

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
    /// ViewModel for the Article Search functionality.
    /// Provides properties for the search term, results collection,
    /// error messages, and commands to perform the search and navigate to other views.
    /// </summary>
    public class ArticleSearchViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Instance of <see cref="TooltipViewModel"/> to manage tooltip data.
        /// </summary>
        public TooltipViewModel TooltipViewModel { get; }

        /// <summary>
        /// The term entered by the user to search for articles.
        /// </summary>
        private string _searchTerm;

        /// <summary>
        /// Collection of articles returned by the search.
        /// </summary>
        private ObservableCollection<Article> _results;

        /// <summary>
        /// Instance of <see cref="ArticleSearch"/> to handle search logic.
        /// </summary>
        private readonly ArticleSearch _articleSearch;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        private string _errorMessage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the term entered by the user to search for articles.
        /// </summary>
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm != value)
                {
                    _searchTerm = value;
                    OnPropertyChanged();
                    // Updates the CanExecute state of the SearchCommand
                    ((RelayCommand)SearchCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of articles returned by the search.
        /// </summary>
        public ObservableCollection<Article> Results
        {
            get => _results;
            set
            {
                if (_results != value)
                {
                    _results = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the command that initiates the article search.
        /// </summary>
        public ICommand SearchCommand { get; }

        #endregion  

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleSearchViewModel"/> class.
        /// </summary>
        /// <param name="navigateAction">Action to handle navigation between views.</param>
        public ArticleSearchViewModel(Action<string> navigateAction)
            : base(navigateAction)
        {
            // Initialize the ArticleSearch model with an empty search term.
            _articleSearch = new ArticleSearch { TextInput = string.Empty };

            // Initialize search term and results collection.
            _searchTerm = string.Empty;
            _results = new ObservableCollection<Article>();

            // Initialize the SearchCommand with the OnSearch method.
            SearchCommand = new RelayCommand(OnSearch, CanExecuteSearch);

            // Initialize TooltipViewModel with predefined tooltips.
            TooltipViewModel = new TooltipViewModel(new List<Tooltip>
            {
                new Tooltip("Geben Sie hier Ihren Suchbegriff ein, z.B. 'Kündigungsfrist' oder 'Lohnfortzahlung', um relevante Artikel zu finden.\n"),
                new Tooltip("Klicken Sie auf diese Schaltfläche, um die Artikelsuche zu starten und passende Artikel anzuzeigen."),
                new Tooltip("Sie können Abkürzungen wie GAV, NAV oder ZPO für spezifische Artikel verwenden."),
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the search command can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns><c>true</c> if the search term is not empty; otherwise, <c>false</c>.</returns>
        private bool CanExecuteSearch(object parameter)
        {
            return !string.IsNullOrWhiteSpace(SearchTerm);
        }

        /// <summary>
        /// Executes the search logic by invoking the <see cref="ArticleSearch"/> model.
        /// Updates the <see cref="Results"/> collection with the search results.
        /// Sets the <see cref="ErrorMessage"/> in case of errors.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSearch(object parameter)
        {
            try
            {
                // Update the ArticleSearch model with the current search term.
                _articleSearch.TextInput = SearchTerm;

                // Retrieve articles that match the search term.
                var articles = _articleSearch.FindRelevantArticles();

                // Update the Results collection with the retrieved articles.
                Results.Clear();
                foreach (var article in articles)
                {
                    Results.Add(article);
                }

                // Clear any existing error messages upon successful search.
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle errors by setting the ErrorMessage and clearing results.
                ErrorMessage = $"Fehler: {ex.Message}";
                Results.Clear();
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        #endregion
    }
}
