using SwissWorkSafe.Models.Articles;
using System;
using System.IO;
using System.Windows;

namespace SwissWorkSafe.Resources
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
    /// Interaction logic for the application.
    /// Initializes and manages the application's lifecycle events.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Overrides the OnStartup method to perform custom actions when the application starts.
        /// </summary>
        /// <param name="e">Provides data for startup events.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Console.WriteLine("Application is starting...");

            try
            {
                Console.WriteLine("Running ArticleInserter.EnsureArticlesAreInserted() at startup...");
                ArticleInserter.EnsureArticlesAreInserted();
                Console.WriteLine("Article insertion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during article insertion: {ex.Message}");

                // Display an error message to the user using a MessageBox
                MessageBox.Show(
                    $"An error occurred during article insertion:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Shutdown();
            }
        }
    }
}
