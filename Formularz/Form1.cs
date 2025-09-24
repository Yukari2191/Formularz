using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Doc;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Formularz
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=Wnioski.db;Version=3;";
        private string currentDbPath = @"Data Source=Wnioski.db;Version=3;";
        private string originalTemplatePL = "";
        private string originalTemplateEN = "";
        private string currentLanguage = "";
        public Form1()
        {
            InitializeComponent();
            DaneWnioska.Columns.Add("Id", "ID");
            DaneWnioska.Columns.Add("Date", "Data");
            DaneWnioska.Columns.Add("Preview", "Podgląd");
            DaneWnioska.Columns.Add("Language", "Język");

            using (SQLiteConnection conn = new SQLiteConnection(currentDbPath))
            {
                conn.Open();
                string create = "CREATE TABLE IF NOT EXISTS Requests (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Content TEXT, Language TEXT)";
                SQLiteCommand cmd = new SQLiteCommand(create, conn);
                cmd.ExecuteNonQuery();
            }
        }
        private async void button_J_PL_Click(object sender, EventArgs e)
        {
            string wniosekURL = "https://cat.put.poznan.pl/sites/default/files/wzory_dokumentow/2023/Wniosek_o_egzamin_komisyjny.doc";
            await LoadDocFromUrlAsync(wniosekURL, "PL");
        }
        private async void button_J_Ang_Click(object sender, EventArgs e)
        {
            string wniosekURL = "https://cat.put.poznan.pl/sites/default/files/wzory_dokumentow/2023/Wniosek_o_egzamin_komisyjny_en.doc";
            await LoadDocFromUrlAsync(wniosekURL, "EN");
        }
        private async Task LoadDocFromUrlAsync(string url, string lang)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync(url);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        Document document = new Document(ms);
                        string text = document.GetText();
                        if (lang == "PL")
                        {
                            originalTemplatePL = text;
                            currentLanguage = "PL";
                        }
                        else
                        {
                            originalTemplateEN = text;
                            currentLanguage = "EN";
                        }
                        richTextBoxWniosek.Text = text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
        private void LoadToGrid()
        {
            DaneWnioska.Rows.Clear();
            using (SQLiteConnection conn = new SQLiteConnection(currentDbPath))
            {
                conn.Open();
                string query = "SELECT Id, Date, Content, Language FROM Requests ORDER BY Date DESC";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string date = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string content = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string lang = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    string preview = "No preview";
                    string[] fields = content.Split(new[] { "||" }, StringSplitOptions.None);
                    if (fields.Length >= 2)
                    {
                        string name = fields.Length > 2 ? fields[2] : "";
                        string subject = fields.Length > 8 ? fields[8] : "";
                        preview = $"{name} - {subject} ({lang})";
                        if (preview.Length > 50) preview = preview.Substring(0, 50) + "...";
                    }
                    DaneWnioska.Rows.Add(id, date, preview, lang);
                }
            }
        }
        private List<string> ExtractFields(string filledText, string lang)
        {
            filledText = Regex.Replace(filledText, @"[ \t]+", " ");

            var fields = new List<string>();
            var patterns = new Dictionary<string, string>();

            string sep = @"[-,.\\/]";
            string datePattern = $@"(?:\d{{4}}{sep}\d{{2}}{sep}\d{{2}}|\d{{2}}{sep}\d{{2}}{sep}\d{{4}})";
            string letterPattern = @"[A-Za-zŁłŚśĆćŃńÓóŻżŹź\-]+";
            string dotsPattern = @"[\s.\u2026]*";

            if (lang == "EN")
            {
                patterns = new Dictionary<string, string>
        {
            { "Date", $@"(?:Poznań, the day|Posnań, dnia|Poznań, dnia){dotsPattern}({datePattern}){dotsPattern}" },
            { "Student ID", $@"{dotsPattern}(\d+){dotsPattern}Student ID No" },
            { "Name", $@"{dotsPattern}({letterPattern}\s+{letterPattern}){dotsPattern}Name and surname" },
            { "Semester, Year", $@"{dotsPattern}(\d+\s*,\s*\d+){dotsPattern}Semester, Year" },
            { "Field and level of study", @"(\S+\s+\S+)\s+Field and level of study" },
            { "Subject", $@"(?:Board\s+)?in the following\s+course:{dotsPattern}([\s\S]+?){dotsPattern}- \.?ECTS points" },
            { "Points", $@"- \.?ECTS points{dotsPattern}(\d+){dotsPattern}" },
            { "Teacher", $@"Teacher:{dotsPattern}({letterPattern}\s+{letterPattern}){dotsPattern}Explanation:" },
            { "Explanation", $@"Explanation:{dotsPattern}([\s\S]*?){dotsPattern}{datePattern}\s+{letterPattern}\s+{letterPattern}{dotsPattern}date and signature" },
            { "Date and signature", $@"{dotsPattern}({datePattern}\s+{letterPattern}\s+{letterPattern}){dotsPattern}date and signature" },
            { "Committee 1", $@"1\.{dotsPattern}([\s\S]+?){dotsPattern}2\." }, 
            { "Committee 2", @"2\.\s+(.*?)\s+3\." }, 
            { "Committee 3", @"3.\s+(.*?)\s+Poznań," }, 
            { "DecisionDate", $@"Poznań,{dotsPattern}({datePattern}){dotsPattern}" },
            { "Stamp and signature", $@"{dotsPattern}({letterPattern}\s+{letterPattern}){dotsPattern}stamp and signature" }
        };
            }
            else // PL
            {
                patterns = new Dictionary<string, string>
        {
            { "Data", $@"Poznań, dnia[\s.]*({datePattern})[\s.]*" },
            { "Numer albumu", $@"[\s.]*(\S+)[\s.]*numer albumu" },
            { "Nazwisko i imię", $@"[\s.]*({letterPattern}\s+{letterPattern})[\s.]*nazwisko i imię" },
            { "Semestr, rok", @"(\S+\s+\S+)\s+semestr, rok" },
            { "Kierunek i stopień studiów", @"(\S+\s+\S+)\s+kierunek i stopień studiów" },
            { "Przedmiot", @"z przedmiotu\s+(.*?)- .punkty" },
            { "Punkty", @"- .punkty(\S+)" },
            { "Prowadzący", $@"prowadzonego przez[\s.]*({letterPattern}\s+{letterPattern})[\s.]*Uzasadnienie wniosku:" },
            { "Uzasadnienie wniosku", $@"Uzasadnienie wniosku:[\s.]*([\s\S]*?)[\s.]*{datePattern}\s+{letterPattern}\s+{letterPattern}[\s.]*data i podpis studenta" },
            { "Data i podpis studenta", $@"[\s.]*({datePattern}\s+{letterPattern}\s+{letterPattern})[\s.]*data i podpis studenta" },
            { "Członek komisji 1", @"1.\s+(.*?)\s+2." },
            { "Członek komisji 2", @"2\.\s+(.*?)\s+3\." },
            { "Członek komisji 3", @"3.\s+(.*?)\s+Poznań, dnia" },
            { "Data komisji", $@"Poznań, dnia[\s.]*({datePattern})[\s.]*" },
            { "Pieczątka i podpis", $@"{dotsPattern}({letterPattern}\s+{letterPattern}){dotsPattern}Pieczątka i podpis" }
        };
            }

            string debugInfo = "Extracted fields:\n";

            foreach (var kv in patterns)
            {
                string label = kv.Key;
                string regex = kv.Value;
                string value = "-";

                var match = Regex.Match(filledText, regex, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    value = match.Groups[1].Value.Trim();
                    // убираем многоточия и лишние пробелы
                    value = Regex.Replace(value, @"[\.\u2026]{2,}", "");
                    value = Regex.Replace(value, @"\s+", " ").Trim();
                    if (string.IsNullOrEmpty(value)) value = "-";
                }

                fields.Add(value);
                debugInfo += $"{label}: {value}\n";
            }

            MessageBox.Show(debugInfo, "Extracted Fields");
            return fields;
        }
        private string FillTemplate(string template, string joinedFields)
        {
            string[] fields = joinedFields.Split(new[] { "||" }, StringSplitOptions.None);
            template = template.Replace("\r\n", "\n").Replace("\r", "\n");

            // Паттерн для заполнителей: 3+ точек или многоточий (включая смешанные)
            const string placeholderPattern = @"[.\u2026]{3,}";

            var matches = Regex.Matches(template, placeholderPattern, RegexOptions.Multiline);
            if (matches.Count == 0) return template; // Нет заполнителей

            var sb = new StringBuilder();
            int lastEnd = 0;
            int fieldIndex = 0;
            int matchIndex = 0;

            while (matchIndex < matches.Count && fieldIndex < fields.Length)
            {
                Match current = matches[matchIndex];
                sb.Append(template, lastEnd, current.Index - lastEnd);

                // Вставляем поле
                string replacement = fields[fieldIndex] != "-" ? fields[fieldIndex] : "";
                sb.Append(replacement);
                lastEnd = current.Index + current.Length;
                fieldIndex++;
                matchIndex++;

                // Специальная обработка после вставки Explanation (индекс поля 8, fieldIndex теперь 9)
                if (fieldIndex == 9) // После "Date and signature"
                {
                    // Проверяем, есть ли еще заполнители, предназначенные для "Explanation"
                    while (matchIndex < matches.Count)
                    {
                        Match next = matches[matchIndex];
                        string between = template.Substring(lastEnd, next.Index - lastEnd).Trim();
                        // Если между заполнителями нет текста (только пробелы или пустая строка),
                        // и следующий заполнитель относится к пустой строке, это лишний заполнитель для Explanation
                        int afterEnd = next.Index + next.Length;
                        int lineEnd = template.IndexOf('\n', afterEnd);
                        if (lineEnd == -1) lineEnd = template.Length;
                        string after = template.Substring(afterEnd, lineEnd - afterEnd).Trim();

                        if (string.IsNullOrWhiteSpace(between) && string.IsNullOrWhiteSpace(after))
                        {
                            // Пропускаем лишний заполнитель для Explanation
                            lastEnd = lineEnd + (lineEnd < template.Length ? 1 : 0);
                            matchIndex++;
                        }
                        else
                        {
                            // Следующий заполнитель относится к следующему полю (например, Committee 1)
                            break;
                        }
                    }
                }
            }

            // Обрабатываем оставшиеся заполнители (удаляем их)
            while (matchIndex < matches.Count)
            {
                Match current = matches[matchIndex];
                sb.Append(template, lastEnd, current.Index - lastEnd);
                sb.Append("");
                lastEnd = current.Index + current.Length;
                matchIndex++;
            }

            // Добавляем остаток текста
            sb.Append(template, lastEnd, template.Length - lastEnd);

            // Отладка: показываем, какие поля используются для заполнения
            MessageBox.Show($"Заполнение шаблона полями: {string.Join("||", fields)}", "Поля шаблона");
            return sb.ToString();
        }
        private void SaveToDB(string filledText)
        {
            if (string.IsNullOrWhiteSpace(filledText))
            {
                MessageBox.Show("Текст для сохранения пуст. Введите данные.");
                return;
            }

            string langToUse = string.IsNullOrEmpty(currentLanguage) ? "PL" : currentLanguage;

            var fieldsList = ExtractFields(filledText, langToUse);
            if (fieldsList == null || fieldsList.Count == 0)
            {
                MessageBox.Show("Не удалось извлечь поля. Проверьте документ.");
                return;
            }

            string content = string.Join("||", fieldsList);

            using (SQLiteConnection conn = new SQLiteConnection(currentDbPath))
            {
                conn.Open();
                string query = "INSERT INTO Requests (Date, Content, Language) VALUES (@date, @content, @lang)";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@lang", langToUse);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show($"Saved to DB: {content}", "Saved Data");
            LoadToGrid();
        }
        private void DeleteFromDB(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Requests WHERE Id = @id";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        private void buttonApplyWniosek_Click(object sender, EventArgs e)
        {
            SaveToDB(richTextBoxWniosek.Text);
            LoadToGrid();
        }
        private void buttonRemoweWniosek_Click(object sender, EventArgs e)
        {
            richTextBoxWniosek.Text = "";
            originalTemplatePL = "";
            originalTemplateEN = "";
            currentLanguage = "";
        }
        private void buttonApplyGrid_Click(object sender, EventArgs e)
        {
            if (DaneWnioska.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку в таблице.");
                return;
            }

            int id = (int)DaneWnioska.CurrentRow.Cells[0].Value;
            string lang = DaneWnioska.CurrentRow.Cells[3].Value?.ToString() ?? "";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Content, Language FROM Requests WHERE Id = @id";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string joined = reader.GetString(0);
                    string savedLang = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string template = savedLang == "PL" ? originalTemplatePL : originalTemplateEN;
                    if (string.IsNullOrEmpty(template))
                    {
                        MessageBox.Show("Шаблон для языка '" + (string.IsNullOrEmpty(savedLang) ? "неизвестного" : savedLang) + "' не загружен. Сначала загрузите документ.");
                        return;
                    }

                    string filled = FillTemplate(template, joined);
                    richTextBoxWniosek.Text = filled;
                    currentLanguage = savedLang;
                    MessageBox.Show("Данные загружены для версии: " + (string.IsNullOrEmpty(savedLang) ? "неизвестной" : savedLang));
                }
            }
        }
        private void buttonClearGrid_Click(object sender, EventArgs e)
        {
            if (DaneWnioska.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку в таблице.");
                return;
            }

            if (DaneWnioska.CurrentRow.Cells[0].Value == null || !int.TryParse(DaneWnioska.CurrentRow.Cells[0].Value.ToString(), out int id))
            {
                MessageBox.Show("Неверный идентификатор строки.");
                return;
            }

            DeleteFromDB(id);
            LoadToGrid();
        }
        private void buttonConvertToSQL_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "SQLite Database (*.db)|*.db|All Files (*.*)|*.*";
                saveFileDialog.Title = "Сохранить базу данных";
                saveFileDialog.DefaultExt = "db";
                saveFileDialog.FileName = $"Wniosek_{DateTime.Now:yyyyMMdd_HHmmss}.db";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string newDbPath = saveFileDialog.FileName;
                    string filledText = richTextBoxWniosek.Text;

                    if (string.IsNullOrWhiteSpace(filledText))
                    {
                        MessageBox.Show("Текст для сохранения пуст. Введите данные.");
                        return;
                    }

                    var fieldsList = ExtractFields(filledText, currentLanguage ?? "PL");
                    if (fieldsList == null || fieldsList.Count == 0)
                    {
                        MessageBox.Show("Не удалось извлечь поля. Проверьте документ.");
                        return;
                    }

                    string content = string.Join("||", fieldsList);

                    using (SQLiteConnection conn = new SQLiteConnection($"Data Source={newDbPath};Version=3;"))
                    {
                        conn.Open();
                        string create = "CREATE TABLE Requests (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Content TEXT, Language TEXT)";
                        SQLiteCommand cmd = new SQLiteCommand(create, conn);
                        cmd.ExecuteNonQuery();

                        string query = "INSERT INTO Requests (Date, Content, Language) VALUES (@date, @content, @lang)";
                        cmd = new SQLiteCommand(query, conn);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@content", content);
                        cmd.Parameters.AddWithValue("@lang", currentLanguage ?? "PL");
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"База данных сохранена как: {newDbPath}");
                    }
                }
            }
        }
        private void buttonConvertFromSQL_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBoxWniosek.Text))
            {
                DialogResult result = MessageBox.Show("Несохраненные данные будут удалены. Продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQLite Database (*.db)|*.db|All Files (*.*)|*.*";
                openFileDialog.Title = "Выберите файл базы данных";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedDbPath = $"Data Source={openFileDialog.FileName};Version=3;";
                    currentDbPath = selectedDbPath;

                    DaneWnioska.Rows.Clear();

                    using (SQLiteConnection conn = new SQLiteConnection(currentDbPath))
                    {
                        try
                        {
                            conn.Open();
                            string query = "SELECT Id, Date, Content, Language FROM Requests ORDER BY Date DESC";
                            SQLiteCommand cmd = new SQLiteCommand(query, conn);
                            SQLiteDataReader reader = cmd.ExecuteReader();

                            int rowCount = 0;
                            while (reader.Read())
                            {
                                rowCount++;
                                int id = reader.GetInt32(0);
                                string date = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                string content = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                string lang = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                string preview = "No preview";
                                string[] fields = content.Split(new[] { "||" }, StringSplitOptions.None);
                                if (!string.IsNullOrEmpty(content))
                                {
                                    string name = fields.Length > 2 ? fields[2] : "";
                                    string subject = fields.Length > 8 ? fields[8] : "";
                                    preview = $"{name} - {subject} ({lang})";
                                    if (preview.Length > 50) preview = preview.Substring(0, 50) + "...";
                                }
                                DaneWnioska.Rows.Add(id, date, preview, lang);
                                Console.WriteLine($"Loaded row {rowCount}: Id={id}, Date={date}, Content={content}, Lang={lang}");
                            }

                            if (rowCount == 0)
                            {
                                MessageBox.Show("В выбранном файле нет данных.");
                            }
                            else
                            {
                                MessageBox.Show($"Загружено {rowCount} строк из файла: {openFileDialog.FileName}");
                            }

                            richTextBoxWniosek.Text = "";
                            originalTemplatePL = "";
                            originalTemplateEN = "";
                            currentLanguage = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
                            currentDbPath = @"Data Source=Wnioski.db;Version=3;";
                        }
                    }
                }
            }
        }
    }
}