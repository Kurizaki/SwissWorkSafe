using System.Collections.ObjectModel;
using System.Windows;
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
    /// and commands to perform the search and navigate to other views.
    /// </summary>
    public class ArticleSearchViewModel : ViewModelBase
    {
        #region Private Fields

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
            SearchCommand = new RelayCommand(_ => OnSearch(), _ => CanExecuteSearch());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the search command can execute.
        /// </summary>
        /// <returns><c>true</c> if the search term is not empty; otherwise, <c>false</c>.</returns>
        private bool CanExecuteSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchTerm);
        }

        /// <summary>
        /// Executes the search logic by invoking the <see cref="ArticleSearch"/> model.
        /// Updates the <see cref="Results"/> collection with the search results.
        /// </summary>
        private void OnSearch()
        {
            try
            {
                // Update the ArticleSearch model with the current search term.
                _articleSearch.TextInput = SearchTerm;

                // Retrieve articles matching the search term.
                var articles = _articleSearch.FindRelevantArticles();

                // Update the Results collection with the new articles.
                Results.Clear();
                foreach (var article in articles)
                {
                    Results.Add(article);
                }
            }
            catch (Exception)
            {
                // Handle exceptions as needed (notify the user).
                MessageBox.Show("An error occurred while searching for articles.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
