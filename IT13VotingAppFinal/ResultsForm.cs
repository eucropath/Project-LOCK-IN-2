using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DrawingColor = System.Drawing.Color;
using DrawingFont = System.Drawing.Font;
using WinFormsControl = System.Windows.Forms.Control;

namespace IT13VotingAppFinal
{
    public partial class ResultsForm : Form
    {
        private Form parentForm;
        private string userRole;
        private Button btnExportExcel;
        private Button btnExportWord;

        public ResultsForm(string role = "Voter", Form parent = null)
        {
            InitializeComponent();
            userRole = role;
            parentForm = parent;
            this.Load += ResultsForm_Load;
            this.Resize += (s, e) => CenterControls();

            if (userRole == "Admin")
            {
                InitializeExportButtons();
            }
            this.FormClosing += ResultsForm_FormClosing;
        }

        private void InitializeExportButtons()
        {
            btnExportExcel = new Button
            {
                Text = "Export to Excel",
                Width = 140,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                BackColor = DrawingColor.FromArgb(46, 125, 50),
                ForeColor = DrawingColor.White
            };
            btnExportExcel.FlatAppearance.BorderSize = 0;
            btnExportExcel.Click += btnExportExcel_Click;
            this.Controls.Add(btnExportExcel);

            btnExportWord = new Button
            {
                Text = "Export to Word",
                Width = 140,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                BackColor = DrawingColor.FromArgb(25, 118, 210),
                ForeColor = DrawingColor.White
            };
            btnExportWord.FlatAppearance.BorderSize = 0;
            btnExportWord.Click += btnExportWord_Click;
            this.Controls.Add(btnExportWord);
        }

        #region Export Methods

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void btnExportWord_Click(object sender, EventArgs e)
        {
            ExportToWord();
        }

        private void ExportToExcel()
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.Title = "Save Election Results";
                    saveDialog.FileName = $"ElectionResults_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (saveDialog.ShowDialog() != DialogResult.OK) return;

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Election Results");

                        CreateExcelHeader(ws);
                        int currentRow = AddExcelResults(ws, 5);
                        currentRow = AddExcelWinners(ws, currentRow + 2);
                        AddExcelTotalVotes(ws, currentRow + 2);

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(saveDialog.FileName);

                        MessageBox.Show("Results exported successfully to Excel!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (MessageBox.Show("Would you like to open the file?", "Open File",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(saveDialog.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateExcelHeader(IXLWorksheet ws)
        {
            ws.Cell("A1").Value = "ELECTION RESULTS - ADMIN VIEW";
            ws.Range("A1:C1").Merge();
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.FromArgb(70, 130, 180);
            ws.Cell("A1").Style.Font.FontColor = XLColor.White;

            ws.Cell("A2").Value = $"Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";
            ws.Range("A2:C2").Merge();
            ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("A4").Value = "Position";
            ws.Cell("B4").Value = "Candidate";
            ws.Cell("C4").Value = "Vote Count";

            var headerRange = ws.Range("A4:C4");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(173, 216, 230);
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private int AddExcelResults(IXLWorksheet ws, int startRow)
        {
            DataTable dt = DataAccess.ExecuteProcedureToDataTable("sp_GetResults");
            int row = startRow;

            foreach (DataRow dr in dt.Rows)
            {
                ws.Cell(row, 1).Value = dr["Position"].ToString();
                ws.Cell(row, 2).Value = dr["Candidate"].ToString();
                ws.Cell(row, 3).Value = Convert.ToInt32(dr["VoteCount"]);

                ws.Range($"A{row}:C{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A{row}:C{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                row++;
            }

            return row;
        }

        private int AddExcelWinners(IXLWorksheet ws, int startRow)
        {
            ws.Cell(startRow, 1).Value = "WINNERS BY POSITION";
            ws.Range($"A{startRow}:C{startRow}").Merge();
            ws.Cell(startRow, 1).Style.Font.Bold = true;
            ws.Cell(startRow, 1).Style.Font.FontSize = 14;
            ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(70, 130, 180);
            ws.Cell(startRow, 1).Style.Font.FontColor = XLColor.White;

            int row = startRow + 2;
            DataTable winnersDt = DataAccess.ExecuteProcedureToDataTable("sp_GetElectionWinners");

            foreach (DataRow dr in winnersDt.Rows)
            {
                ws.Cell(row, 1).Value = dr["Position"].ToString();
                ws.Cell(row, 2).Value = dr["Candidate"].ToString();
                ws.Cell(row, 3).Value = $"{dr["VoteCount"]} votes";

                ws.Range($"A{row}:C{row}").Style.Font.Bold = true;
                ws.Range($"A{row}:C{row}").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 224);
                row++;
            }

            return row;
        }

        private void AddExcelTotalVotes(IXLWorksheet ws, int row)
        {
            DataTable dt = DataAccess.ExecuteProcedureToDataTable("sp_GetResults");
            var totalVotes = dt.AsEnumerable().Sum(r => Convert.ToInt64(r["VoteCount"]));

            ws.Cell(row, 1).Value = "TOTAL VOTES:";
            ws.Cell(row, 2).Value = totalVotes;
            ws.Range($"A{row}:B{row}").Style.Font.Bold = true;
            ws.Range($"A{row}:B{row}").Style.Font.FontSize = 12;
            ws.Range($"A{row}:B{row}").Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private void ExportToWord()
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Word Documents|*.docx";
                    saveDialog.Title = "Save Election Results";
                    saveDialog.FileName = $"ElectionResults_{DateTime.Now:yyyyMMdd_HHmmss}.docx";

                    if (saveDialog.ShowDialog() != DialogResult.OK) return;

                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(
                        saveDialog.FileName, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        AddWordTitle(body);
                        AddWordDate(body);
                        body.AppendChild(new Paragraph());

                        DataTable dt = DataAccess.ExecuteProcedureToDataTable("sp_GetResults");
                        body.AppendChild(CreateResultsTable(dt));

                        body.AppendChild(new Paragraph());
                        body.AppendChild(new Paragraph());

                        AddWordWinners(body);
                        AddWordTotalVotes(body, dt);

                        mainPart.Document.Save();
                    }

                    MessageBox.Show("Results exported successfully to Word!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Would you like to open the file?", "Open File",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Word: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddWordTitle(Body body)
        {
            Paragraph titlePara = body.AppendChild(new Paragraph());
            Run titleRun = titlePara.AppendChild(new Run());
            titleRun.AppendChild(new Text("ELECTION RESULTS - ADMIN VIEW"));

            RunProperties titleProps = new RunProperties();
            titleProps.AppendChild(new Bold());
            titleProps.AppendChild(new FontSize() { Val = "32" });
            titleRun.PrependChild(titleProps);

            ParagraphProperties titleParaProps = new ParagraphProperties();
            titleParaProps.Append(new Justification() { Val = JustificationValues.Center });
            titlePara.PrependChild(titleParaProps);
        }

        private void AddWordDate(Body body)
        {
            Paragraph datePara = body.AppendChild(new Paragraph());
            Run dateRun = datePara.AppendChild(new Run());
            dateRun.AppendChild(new Text($"Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}"));

            ParagraphProperties dateParaProps = new ParagraphProperties();
            dateParaProps.Append(new Justification() { Val = JustificationValues.Center });
            datePara.PrependChild(dateParaProps);
        }

        private void AddWordWinners(Body body)
        {
            Paragraph summaryTitle = body.AppendChild(new Paragraph());
            Run summaryRun = summaryTitle.AppendChild(new Run());
            summaryRun.AppendChild(new Text("WINNERS BY POSITION"));

            RunProperties summaryProps = new RunProperties();
            summaryProps.AppendChild(new Bold());
            summaryProps.AppendChild(new FontSize() { Val = "28" });
            summaryRun.PrependChild(summaryProps);

            body.AppendChild(new Paragraph());

            DataTable winnersDt = DataAccess.ExecuteProcedureToDataTable("sp_GetElectionWinners");
            foreach (DataRow dr in winnersDt.Rows)
            {
                Paragraph winnerPara = body.AppendChild(new Paragraph());
                Run winnerRun = winnerPara.AppendChild(new Run());
                winnerRun.AppendChild(new Text(
                    $"• {dr["Position"]}: {dr["Candidate"]} ({dr["VoteCount"]} votes)"));

                RunProperties winnerProps = new RunProperties();
                winnerProps.AppendChild(new Bold());
                winnerRun.PrependChild(winnerProps);
            }
        }

        private void AddWordTotalVotes(Body body, DataTable dt)
        {
            body.AppendChild(new Paragraph());
            body.AppendChild(new Paragraph());

            var totalVotes = dt.AsEnumerable().Sum(r => Convert.ToInt64(r["VoteCount"]));
            Paragraph totalPara = body.AppendChild(new Paragraph());
            Run totalRun = totalPara.AppendChild(new Run());
            totalRun.AppendChild(new Text($"TOTAL VOTES: {totalVotes}"));

            RunProperties totalProps = new RunProperties();
            totalProps.AppendChild(new Bold());
            totalProps.AppendChild(new FontSize() { Val = "24" });
            totalRun.PrependChild(totalProps);
        }

        private Table CreateResultsTable(DataTable dt)
        {
            Table table = new Table();

            TableProperties props = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                )
            );
            table.AppendChild(props);

            TableRow headerRow = new TableRow();
            headerRow.Append(
                CreateTableCell("Position", true),
                CreateTableCell("Candidate", true),
                CreateTableCell("Vote Count", true)
            );
            table.AppendChild(headerRow);

            foreach (DataRow dr in dt.Rows)
            {
                TableRow dataRow = new TableRow();
                dataRow.Append(
                    CreateTableCell(dr["Position"].ToString(), false),
                    CreateTableCell(dr["Candidate"].ToString(), false),
                    CreateTableCell(dr["VoteCount"].ToString(), false)
                );
                table.AppendChild(dataRow);
            }

            return table;
        }

        private TableCell CreateTableCell(string text, bool isHeader)
        {
            TableCell cell = new TableCell();
            Paragraph para = new Paragraph();
            Run run = new Run();
            run.AppendChild(new Text(text));

            if (isHeader)
            {
                RunProperties runProps = new RunProperties();
                runProps.AppendChild(new Bold());
                run.PrependChild(runProps);

                TableCellProperties cellProps = new TableCellProperties();
                cellProps.Append(new Shading()
                {
                    Val = ShadingPatternValues.Clear,
                    Color = "auto",
                    Fill = "4682B4"
                });
                cell.Append(cellProps);
            }

            para.AppendChild(run);
            cell.AppendChild(para);
            return cell;
        }

        #endregion

        #region Data Loading Methods

        private void LoadPositions()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllPositions");
            cmbPositions.DataSource = dt;
            cmbPositions.DisplayMember = "Position";
            cmbPositions.ValueMember = "Position";
        }

        private void LoadResults(string position = "")
        {
            DataTable dt = GetResultsData(position);
            ConfigureDataGridView(dt);
            UpdateWinnerLabel(dt);
            UpdateTotalVotesLabel(dt);
        }

        private DataTable GetResultsData(string position)
        {
            if (userRole == "Admin")
            {
                return string.IsNullOrEmpty(position)
                    ? DataAccess.ExecuteProcedureToDataTable("sp_GetResults")
                    : DataAccess.ExecuteProcedureToDataTable("sp_GetResultsByPosition",
                        new MySql.Data.MySqlClient.MySqlParameter("@in_position", position));
            }
            else
            {
                if (string.IsNullOrEmpty(position))
                {
                    return DataAccess.ExecuteProcedureToDataTable("sp_GetElectionWinners");
                }
                else
                {
                    var allWinners = DataAccess.ExecuteProcedureToDataTable("sp_GetElectionWinners");
                    if (allWinners.Rows.Count > 0)
                    {
                        var filtered = allWinners.AsEnumerable()
                            .Where(r => r["Position"].ToString() == position);

                        return filtered.Any() ? filtered.CopyToDataTable() : allWinners.Clone();
                    }
                    return allWinners;
                }
            }
        }

        private void ConfigureDataGridView(DataTable dt)
        {
            dgvResults.DataSource = dt;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (userRole == "Voter" && dgvResults.Columns.Contains("VoteCount"))
            {
                dgvResults.Columns["VoteCount"].Visible = false;
            }

            if (dgvResults.Columns.Contains("VoteCount"))
            {
                dgvResults.Columns["VoteCount"].DefaultCellStyle.Font = 
                    new DrawingFont("Segoe UI", 10, FontStyle.Bold);
            }

            if (dgvResults.Columns.Contains("CandidateId"))
            {
                dgvResults.Columns["CandidateId"].Visible = false;
            }
        }

        private void UpdateWinnerLabel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                var maxVotes = dt.AsEnumerable().Max(r => Convert.ToInt64(r["VoteCount"]));
                var winnerRows = dt.AsEnumerable()
                    .Where(r => Convert.ToInt64(r["VoteCount"]) == maxVotes)
                    .Select(r => r["Candidate"].ToString());

                lblWinner.Text = userRole == "Admin"
                    ? $"Winner: {string.Join(", ", winnerRows)} ({maxVotes} votes)"
                    : $"Winner: {string.Join(", ", winnerRows)}";
            }
            else
            {
                lblWinner.Text = "Winner: N/A";
            }
        }

        private void UpdateTotalVotesLabel(DataTable dt)
        {
            if (userRole == "Admin")
            {
                var totalVotes = dt.AsEnumerable().Sum(r => Convert.ToInt64(r["VoteCount"]));
                lblTotalVotes.Text = $"Total Votes: {totalVotes}";
                lblTotalVotes.Visible = true;
            }
            else
            {
                lblTotalVotes.Visible = false;
            }
        }

        #endregion

        #region Form Initialization and Styling

        private void ResultsForm_Load(object sender, EventArgs e)
        {
            ConfigureFormSettings();
            ConfigureBackground();
            ConfigureTitle();
            ConfigureLabels();
            ConfigureButtons();
            ConfigureComboBox();
            ConfigureDataGrid();

            LoadPositions();
            LoadResults();
            CenterControls();
        }

        private void ConfigureFormSettings()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(950, 650);
        }

        private void ConfigureBackground()
        {
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();
        }

        private void ConfigureTitle()
        {
            label1.Text = userRole == "Admin" 
                ? "Election Results - Admin View" 
                : "Election Results";
            label1.Font = new DrawingFont("Segoe UI", 18, FontStyle.Bold);
            label1.ForeColor = DrawingColor.White;
            label1.BackColor = DrawingColor.Transparent;
            label1.AutoSize = true;
            label1.Parent = pictureBox1;
        }

        private void ConfigureLabels()
        {
            foreach (WinFormsControl ctrl in this.Controls)
            {
                if (ctrl is Label lbl && lbl != label1)
                {
                    lbl.BackColor = DrawingColor.Transparent;
                    lbl.ForeColor = DrawingColor.White;
                    lbl.Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold);
                    lbl.Parent = pictureBox1;
                }
            }
        }

        private void ConfigureButtons()
        {
            StyleButton(btnRefresh, "Refresh", DrawingColor.SteelBlue, DrawingColor.White);
            StyleButton(button1, "Back", DrawingColor.IndianRed, DrawingColor.White);
        }

        private void ConfigureComboBox()
        {
            cmbPositions.Font = new DrawingFont("Segoe UI", 10);
            cmbPositions.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPositions.Width = 200;
        }

        private void ConfigureDataGrid()
        {
            dgvResults.BackgroundColor = DrawingColor.White;
            dgvResults.GridColor = DrawingColor.LightGray;
            dgvResults.DefaultCellStyle.BackColor = DrawingColor.White;
            dgvResults.DefaultCellStyle.ForeColor = DrawingColor.Black;
            dgvResults.AlternatingRowsDefaultCellStyle.BackColor = DrawingColor.AliceBlue;
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = DrawingColor.SteelBlue;
            dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = DrawingColor.White;
            dgvResults.ColumnHeadersDefaultCellStyle.Font = 
                new DrawingFont("Segoe UI", 10, FontStyle.Bold);
            dgvResults.EnableHeadersVisualStyles = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

       

        private void StyleButton(Button btn, string text, DrawingColor backColor, DrawingColor foreColor)
        {
            btn.Text = text;
            btn.Width = 120;
            btn.Height = 40;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold);
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
        }

        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;

            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 20;

            dgvResults.Width = this.ClientSize.Width - 120;
            dgvResults.Left = (this.ClientSize.Width - dgvResults.Width) / 2;
            dgvResults.Top = label1.Bottom + 20;
            dgvResults.Height = this.ClientSize.Height / 3;

            cmbPositions.Left = centerX - (cmbPositions.Width / 2);
            cmbPositions.Top = dgvResults.Bottom + 15;

            lblWinner.Left = centerX - 200;
            lblWinner.Top = cmbPositions.Bottom + 20;

            if (lblTotalVotes.Visible)
            {
                lblTotalVotes.Left = centerX - 200;
                lblTotalVotes.Top = lblWinner.Bottom + 10;
                btnRefresh.Left = centerX - (btnRefresh.Width / 2);
                btnRefresh.Top = lblTotalVotes.Bottom + 20;
            }
            else
            {
                btnRefresh.Left = centerX - (btnRefresh.Width / 2);
                btnRefresh.Top = lblWinner.Bottom + 20;
            }

            if (userRole == "Admin" && btnExportExcel != null && btnExportWord != null)
            {
                int buttonSpacing = 20;
                int totalButtonWidth = btnExportExcel.Width + buttonSpacing + btnExportWord.Width;
                int startX = centerX - (totalButtonWidth / 2);

                btnExportExcel.Left = startX;
                btnExportExcel.Top = btnRefresh.Bottom + 15;

                btnExportWord.Left = btnExportExcel.Right + buttonSpacing;
                btnExportWord.Top = btnExportExcel.Top;

                button1.Left = centerX - (button1.Width / 2);
                button1.Top = btnExportExcel.Bottom + 15;
            }
            else
            {
                button1.Left = centerX - (button1.Width / 2);
                button1.Top = btnRefresh.Bottom + 20;
            }
        }

        #endregion

        #region Event Handlers

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var pos = cmbPositions.SelectedValue?.ToString() ?? "";
            LoadResults(pos);
        }

        private void cmbPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pos = cmbPositions.SelectedValue?.ToString() ?? "";
            LoadResults(pos);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResultsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;   // stop the app from quitting
                this.Hide();

                if (parentForm != null && !parentForm.IsDisposed)
                {
                    parentForm.Show(); // return to AdminDashboard
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void lblTotalVotes_Click(object sender, EventArgs e) { }
        private void lblWinner_Click(object sender, EventArgs e) { }
 
        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        #endregion
    }
}