using ClosedXML.Excel;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LoanManagementSystem.Service
{
    public class NpaService : INpaService
    {
        private readonly INpaRepository _npaRepository;
        private readonly ILoanApplicationRepository _loanApplicationRepository;

        public NpaService(INpaRepository npaRepository,ILoanApplicationRepository loanApplicationRepository)
        {
            _npaRepository = npaRepository;
            _loanApplicationRepository = loanApplicationRepository;
        }

        public async Task<Npa> AddCustomerToNpaAsync(int customerId,int applicationId)
        {
            // Get overdue from repository
            var overdue = await _npaRepository.GetCustomerOverdueAmountAsync(customerId,applicationId);

            var npa = new Npa
            {
                CustomerId = customerId,
                LoanApplicationId = applicationId,
                TotalOverdue = overdue
            };

            await _npaRepository.AddNpaAsync(npa);

            var loanApp = await _loanApplicationRepository.GetById(applicationId);
            if (loanApp != null)
            {
                loanApp.IsNpa = true;
                //await _loanApplicationRepository.Update(loanApp);
                //await _loanApplicationRepository.SaveChangesAsync();
            }

            await _npaRepository.SaveChangesAsync();
            return npa;
        }

        public async Task<IEnumerable<Npa>> GetAllNpasAsync()
        {
            return await _npaRepository.GetAllNpasAsync();
        }

        public async Task<decimal> GetTotalOverdueAllAsync()
        {
            return await _npaRepository.GetTotalOverdueAllAsync();
        }

        public async Task<IEnumerable<Npa>> GetFilteredNpasAsync(string sortBy, bool ascending)
        {
            var npas = await _npaRepository.GetAllNpasAsync();

            return sortBy switch
            {
                "overdue" => ascending ? npas.OrderBy(n => n.TotalOverdue) : npas.OrderByDescending(n => n.TotalOverdue),
                "date" => ascending ? npas.OrderBy(n => n.AddedDate) : npas.OrderByDescending(n => n.AddedDate),
                _ => npas.OrderBy(n => n.NpaId)
            };
        }


        public async Task<byte[]> ExportNpasToExcelAsync(string sortBy, bool asc)
        {
            //var npas = await GetAllNpasAsync();
            var npas = await GetFilteredNpasAsync(sortBy, asc);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("NPA Report");

            // Header
            worksheet.Cell(1, 1).Value = "Customer Name";
            worksheet.Cell(1, 2).Value = "Loan Application Id";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Total Overdue";

            int row = 2;

            foreach (var npa in npas)
            {
                worksheet.Cell(row, 1).Value = npa.Customer.Customer_Name;
                worksheet.Cell(row, 2).Value = npa.LoanApplicationId;
                worksheet.Cell(row, 3).Value = npa.Customer.User?.Email ?? "";
                worksheet.Cell(row, 4).Value = npa.TotalOverdue;
                row++;
            }

            // Optional: add grand total at the bottom
            worksheet.Cell(row, 3).Value = "Total";
            worksheet.Cell(row, 4).Value = npas.Sum(n => n.TotalOverdue);

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<(byte[] FileContent, string ContentType, string FileName)> ExportNpasToPdfAsync(string sortBy, bool asc)
        {
            //var npas = await _npaRepository.GetAllNpasAsync();
            var npas = await GetFilteredNpasAsync(sortBy, asc);
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Set page margins
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);

                    // Header section with professional styling
                    page.Header().Height(80).Background(Colors.Blue.Darken3).Padding(15).Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("NPA REPORT")
                                .FontSize(24)
                                .Bold()
                                .FontColor(Colors.White);

                            column.Item().Text($"Generated on: {DateTime.Now:MMMM dd, yyyy}")
                                .FontSize(12)
                                .FontColor(Colors.Grey.Lighten2);
                        });

                        //row.ConstantItem(60).Height(50).Placeholder().Background(Colors.White.Darken1);
                        row.ConstantItem(60)
                                   .Height(50)
                                   .Background(Colors.White)
                                   .AlignMiddle()   // optional, if you want vertical centering
                                   .AlignCenter()   // optional, if you want horizontal centering
                                   .Text("");       // empty text acts as placeholder



                    });

                    // Content section
                    page.Content().Padding(20).Column(column =>
                    {
                        // Summary section
                        column.Item().Padding(10).Background(Colors.Grey.Lighten4).Border(1, Colors.Grey.Medium).Column(summaryColumn =>
                        {
                            
                            summaryColumn.Item().Container()
                              .Padding(5) // apply padding here
                              .Background(Colors.Grey.Lighten4) // optional background
                              .Border(1, Colors.Grey.Medium)   // optional border
                              .Text("Summary")
                              .FontSize(16)
                              .Bold()
                              .FontColor(Colors.Blue.Darken2);


                            summaryColumn.Item().Row(summaryRow =>
                            {
                                summaryRow.RelativeItem().Text($"Total NPAs: {npas.Count()}")
                                    .FontSize(12)
                                    .FontColor(Colors.Black);

                                summaryRow.RelativeItem().Text($"Total Amount: {npas.Sum(n => n.TotalOverdue):C}")
                                    .FontSize(12)
                                    .Bold()
                                    .FontColor(Colors.Red.Darken1);
                            });
                        });

                        column.Item().Padding(10);

                        // Main data table
                        column.Item().Table(table =>
                        {
                            // Define columns with proper spacing
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);   // S.No
                                columns.RelativeColumn(3);    // Customer Name
                                columns.RelativeColumn(6);    // Email
                                columns.ConstantColumn(40); // Loan App Id
                                columns.RelativeColumn(4);    // Total Overdue
                                columns.ConstantColumn(60);   // Status
                            });


                            // Table header with professional styling
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("S.N")
                                    .FontSize(12).Bold().FontColor(Colors.White);

                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("Customer Name")
                                    .FontSize(12).Bold().FontColor(Colors.White);

                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("Email Address")
                                    .FontSize(12).Bold().FontColor(Colors.White);

                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("Id").FontColor(Colors.White).Bold();

                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("Total Overdue")
                                    .FontSize(12).Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("Status")
                                    .FontSize(12).Bold().FontColor(Colors.White);
                            });

                            // Table rows with alternating colors and borders
                            int serialNumber = 1;
                            foreach (var npa in npas)
                            {
                                var rowColor = serialNumber % 2 == 0 ? Colors.Grey.Lighten5 : Colors.White;
                                var statusColor = npa.TotalOverdue > 10000 ? Colors.Red.Lighten4 : Colors.Yellow.Lighten4;
                                var statusText = npa.TotalOverdue > 10000 ? "Critical" : "Warning";

                                table.Cell().Background(rowColor).Border(0.5f, Colors.Grey.Medium).Padding(8).AlignRight()
                                    .Text(serialNumber.ToString())
                                    .FontSize(10)
                                    .FontColor(Colors.Black);

                                table.Cell().Background(rowColor).Border(0.5f, Colors.Grey.Medium).Padding(8)
                                    .Text(npa.Customer.Customer_Name ?? "N/A")
                                    .FontSize(10)
                                    .FontColor(Colors.Black);

                                table.Cell().Background(rowColor).Border(0.5f, Colors.Grey.Medium).Padding(8)
                                    .Text(npa.Customer.User?.Email ?? "Not Available")
                                    .FontSize(10)
                                    .FontColor(Colors.Blue.Darken1);


                                table.Cell().Background(rowColor).Border(0.5f, Colors.Grey.Medium).Padding(8).AlignRight().Text(npa.LoanApplicationId.ToString());

                                table.Cell().Background(rowColor).Border(0.5f, Colors.Grey.Medium).Padding(8).AlignRight()
                                    .Text($"{npa.TotalOverdue:C}")
                                    .FontSize(10)
                                    .Bold()
                                    .FontColor(Colors.Red.Darken1);

                                table.Cell().Background(statusColor).Border(0.5f, Colors.Grey.Medium).Padding(8)
                                    .Text(statusText)
                                    .FontSize(9)
                                    .Bold()
                                    .FontColor(Colors.Black);

                                serialNumber++;
                            }
                        });
                    });

                    // Footer section
                    page.Footer().Height(40).Background(Colors.Grey.Lighten3).Padding(10).Row(row =>
                    {
                        row.RelativeItem().Text("Confidential Document - Internal Use Only")
                            .FontSize(10)
                            .Italic()
                            .FontColor(Colors.Grey.Darken1);


                        row.ConstantItem(100).AlignRight().Text(text =>
                        {
                            text.Span("Page ").FontSize(10).FontColor(Colors.Grey.Darken1);
                            text.CurrentPageNumber().FontSize(10).FontColor(Colors.Grey.Darken1);
                            text.Span(" / ").FontSize(10).FontColor(Colors.Grey.Darken1);
                            text.TotalPages().FontSize(10).FontColor(Colors.Grey.Darken1);
                        });
                    });
                });
            });

            var fileContent = document.GeneratePdf();
            return (fileContent, "application/pdf", $"NpaReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

    }

}
