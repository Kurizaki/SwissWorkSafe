# SwissWorkSafe 🚀

## Einführung 📘
Willkommen bei **SwissWorkSafe**! 🇨🇭🎉 Diese Anwendung wurde entwickelt, um euch die Schweizer Arbeitsgesetze einfach und zugänglich zu machen. Mit einer umfassenden Sammlung von Artikeln und Signalwörtern ermöglicht **SwissWorkSafe** euch ein tieferes Verständnis der Rechtslage und erleichtert die gezielte Suche. 🔍👩‍⚖️

Egal, ob ihr Arbeitnehmer, Arbeitgeber oder juristisch interessiert seid – SwissWorkSafe ist euer zuverlässiger Begleiter im Schweizer Arbeitsrecht. 💼📚

---

## Projektübersicht 🛠️
SwissWorkSafe ist eine moderne **WPF-Anwendung**, die mit **C#** entwickelt wurde und auf **.NET 8** abzielt. 💻  
Die Anwendung nutzt eine **SQLite-Datenbank**, um Artikel des Arbeitsrechts und relevante Signalwörter effizient zu speichern und zu verwalten. Die Daten werden aus einer **JSON-Datei** importiert und automatisch in die Datenbank eingefügt, wenn sie noch nicht vorhanden sind. 📥🗄️

---

## Hauptmerkmale 🌟
- **Umfangreiche Arbeitsrechtsdatenbank**:  
  Enthält zahlreiche Artikel mit präzisen Beschreibungen.
  
- **Signalwörter für vereinfachte Navigation**:  
  Jedes Thema ist mit relevanten Keywords versehen, um die Suche zu optimieren.

- **Benutzerfreundliche Oberfläche**:  
  Dank der WPF-Technologie bietet die Anwendung eine intuitive und moderne Benutzeroberfläche.

- **Automatisierter Datenimport**:  
  Lädt Artikel und Signalwörter aus einer JSON-Datei und synchronisiert sie nahtlos mit der SQLite-Datenbank.

---

## Installation und Ausführung 🚀

### Voraussetzungen ✔️
- **.NET 8 SDK**
- **Visual Studio 2022** oder höher

### Installationsschritte 📝
1. **Repository klonen**:  
   ```bash
   git clone https://github.com/YourUsername/SwissWorkSafe.git
   cd SwissWorkSafe
   ```

2. **Abhängigkeiten wiederherstellen**:  
   Öffnet das Projekt in Visual Studio und stellt sicher, dass alle NuGet-Pakete wiederhergestellt werden. 🔧

3. **Datenbank einrichten**:  
   Beim ersten Start der Anwendung wird die SQLite-Datenbank automatisch erstellt, und die Artikeldaten aus der `articles.json`-Datei werden eingefügt. 🗃️

4. **Anwendung ausführen**:  
   Startet die Anwendung mit Visual Studio (`F5`) oder verwendet den Befehl:  
   ```bash
   dotnet run
   ```

---

## Projektstruktur 📂

- **Abhängigkeiten**:  
  Enthält alle externen NuGet-Pakete, die im Projekt verwendet werden.

- **Commands**:  
  - `RelayCommand.cs`: Implementiert ein allgemeines Command-Muster zur Steuerung der Anwendung.

- **Database**:  
  - **Models**:  
    - **Articles**:  
      - `Article.cs`: Datenmodell für Artikel.  
      - `ArticleInserter.cs`: Logik für das Einfügen von Artikeln und Signalwörtern in die SQLite-Datenbank.  
    - **Core**:  
      - `ArticleSearch.cs`: Logik für die Suche nach Artikeln.  
      - `SalaryContinuation.cs`: Enthält Logik zu Themen wie Lohnfortzahlung.  
      - `SwissWorkSafe.cs`: Hauptklasse der Anwendung.  
      - `Termination.cs`: Logik für Kündigungsregelungen.  
    - **Settings**:  
      - `Autostart.cs`: Verwaltung von Autostart-Einstellungen.  
      - `Colors.cs`: Farbmanagement.  
      - `Language.cs`: Sprachunterstützung.  
      - `Settings.cs`: Speicherung allgemeiner Anwendungseinstellungen.  
      - `Tooltips.cs`: Verwaltung von Tooltips für die Benutzeroberfläche.

- **Resources**:  
  - `App.xaml`: Konfigurationsdatei für die WPF-Anwendung.  
  - `articles.json`: Enthält Artikeldaten und Signalwörter im JSON-Format.  
  - `Colors.xaml`: Definition der Farbressourcen für die UI.

- **ViewModels**:  
  - `ArticleSearchViewModel.cs`: ViewModel für die Artikelsuche.  
  - `HelpViewModel.cs`: ViewModel für die Hilfeseite.  
  - `MainWindowViewModel.cs`: Haupt-ViewModel der Anwendung.  
  - `MenuViewModel.cs`: ViewModel für das Menü.  
  - `SalaryContinuationViewModel.cs`: ViewModel für Lohnfortzahlungsregelungen.  
  - `SettingsViewModel.cs`: ViewModel für die Anwendungseinstellungen.  
  - `TerminationViewModel.cs`: ViewModel für Kündigungsregelungen.  
  - `ViewModelBase.cs`: Basisklasse für alle ViewModels.

- **Views**:  
  - `ArticleSearchView.xaml`: UI für die Artikelsuche.  
  - `HelpView.xaml`: UI für die Hilfeseite.  
  - `MainWindow.xaml`: Hauptfenster der Anwendung.  
  - `MenuView.xaml`: UI für das Menü.  
  - `SalaryContinuationView.xaml`: UI für Lohnfortzahlungsinformationen.  
  - `SettingsView.xaml`: UI für die Einstellungen.  
  - `TerminationView.xaml`: UI für Kündigungsregelungen.

- **AssemblyInfo.cs**:  
  Enthält Assembly-Metadaten und -Konfiguration.

---

## Mitwirkende 🤝
- **Keanu Koelewijn**  
- **Rebecca Wili**  
- **Salma Tanner**  
- **Lorenzo Lai**  

---

## Lizenz 📄
Dieses Projekt ist unter der **MIT-Lizenz** lizenziert. Weitere Details findet ihr in der `LICENSE`-Datei.  

---

## Kontakt ✉️
Habt ihr Fragen, Feedback oder Ideen? Wir freuen uns über eure Beiträge und Anregungen! Nutzt unser **[GitHub Discussions Forum](https://github.com/Kurizaki/SwissWorkSafe/discussions)**, um:

- Fragen zu stellen
- Feature-Vorschläge zu machen
- Feedback zu geben
- Probleme zu diskutieren
- Euch mit der Community auszutauschen

👉 **[Zum GitHub Discussions Forum](https://github.com/Kurizaki/SwissWorkSafe/discussions)**
