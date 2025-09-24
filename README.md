# Formularz

## Overview
The Formularz is a Windows Forms application written in C# that allows users to manage examination requests by loading, editing, and saving document templates in Polish (PL) and English (EN). The application interacts with a SQLite database to store and retrieve request data, supports downloading document templates from URLs, and provides functionality to extract and fill fields in these templates. The user interface uses Polish labels and button texts for a localized experience.

## Features
- **Load Document Templates**: Download and display examination request templates in Polish or English from specified URLs.
- **Form Field Extraction**: Extract key fields from filled templates using regex patterns tailored for Polish and English documents.
- **Database Integration**: Store and retrieve request data in a SQLite database (`Wnioski.db`).
- **Grid View**: Display saved requests in a DataGridView with columns for ID, Date ("Data"), Preview ("Podgląd"), and Language ("Język").
- **Save and Load Database**: Export request data to a new SQLite database file or import from an existing one.
- **Template Filling**: Populate templates with saved data for editing.
- **Error Handling**: Manage database locking issues with retry logic and display user-friendly error messages.

## Requirements
- **.NET Framework**: Version 4.7.2 or higher.
- **SQLite**: The application uses SQLite for data storage. Ensure the `System.Data.SQLite` NuGet package is installed.
- **Dependencies**: 
  - `System.Data.SQLite` for database operations.
  - `System.Net.Http` for downloading document templates.
  - A library for handling `.doc` files (e.g., a third-party library like `Aspose.Words` or similar for the `Document` class, as referenced in the code).
- **Internet Connection**: Required to download document templates from the provided URLs.

## Installation
1. Clone or download the project repository.
2. Open the solution in Visual Studio.
3. Install the required NuGet package:
   - `System.Data.SQLite`
4. Ensure a library for handling `.doc` files is included (modify the code if a different library is used for the `Document` class).
5. Build and run the project.

## Usage
1. **Launch the Application**:
   - The application window title is "Wniosek". It initializes a SQLite database (`Wnioski.db`) and creates a `Requests` table if it doesn't exist. The grid columns are labeled "ID", "Data", "Podgląd", and "Język".
2. **Load a Template**:
   - Click the "Wersja w j. polskim" button (button_J_PL) to load the Polish template or the "Wersja w j. angielskim" button (button_J_Ang) for the English template.
   - Templates are downloaded from the specified URLs and displayed in the rich text box (richTextBoxWniosek).
3. **Fill and Save Data**:
   - Edit the template in the rich text box.
   - Click "Zapisz" to extract fields and save the data to the database.
   - The grid view updates to show the saved request with a preview.
4. **Manage Requests**:
   - Select a row in the grid and click "Odczytaj" to load and display the saved request in the rich text box.
   - Click "Usuń" to delete a selected request from the database.
5. **Export/Import Database**:
   - Click "Zapisz w DB" to save the current data to a new SQLite database file.
   - Click "Wczytaj z DB" to load data from an existing SQLite database file.
6. **Clear Form**:
   - Click "Odzucenie" to clear the rich text box and reset the template.

## Database Structure
The application uses a SQLite database with the following table:

**Table: Requests**
- `Id` (INTEGER, PRIMARY KEY, AUTOINCREMENT): Unique identifier for each request.
- `Date` (TEXT): Date and time the request was saved (format: `yyyy-MM-dd HH:mm:ss`).
- `Content` (TEXT): Extracted fields from the template, joined with `||`.
- `Language` (TEXT): Language of the template (`PL` or `EN`).

## Notes
- The application handles database locking by implementing retry logic (up to 3 retries with a 100ms delay).
- Field extraction uses regex patterns tailored for Polish and English templates, supporting fields like Date ("Data"), Student ID ("Numer albumu"), Name ("Nazwisko i imię"), and more.
- Ensure the URLs for document templates (`Wniosek_o_egzamin_komisyjny.doc` and `Wniosek_o_egzamin_komisyjny_en.doc`) are accessible.
- The application assumes the `Document` class can extract text from `.doc` files. Replace it with an appropriate library if needed.
- Extract key fields from filled templates using regex patterns tailored for Polish and English documents, supporting various date formats (e.g., YYYY-MM-DD or DD-MM-YYYY and with other separators: [.], [,], [/], [\].).

## Limitations
- No input validation for user-entered data in the rich text box.
- The application assumes the template structure matches the predefined regex patterns.
- The `Document` class dependency is not specified in the code; a compatible library must be included.
- No support for languages other than Polish and English.
- UI layout: The form size is 1110x496 pixels, with the rich text box on the right (406,3; 692x437) and grid on the left (8,73; 392x292).

## Possible future improvements
- Add input validation for template fields.
- Include logging for debugging and error tracking.
- Translate UI to full English if needed for broader accessibility.
