
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Model;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace _301270677_prathan__Lab02
{
    /// <summary>
    /// Interaction logic for PdfViewerWindow.xaml
    /// </summary>
    public partial class PdfViewerWindow : Window
    {
        private BooksListWindow bookListWindow;
        private Book selectedBook { get; set; }
        const string tableName = "Bookshelf";
        private BookRepository bookRepository;
        public PdfViewerWindow(BooksListWindow booksListWindow, Book selectedBook)
        {
            InitializeComponent();
            this.bookListWindow = booksListWindow;
            this.selectedBook = selectedBook;
            bookRepository = new BookRepository();
            LoadPdfFromS3(selectedBook);
       
        }

        private async void LoadPdfFromS3(Book selectedBook)
        {
            Stream pdfStream = await bookRepository.GetPdfStreamAsync(selectedBook);

            try
            {
               
                // Read the response stream into a memory stream
                MemoryStream memoryStream = new MemoryStream();
                pdfStream.CopyTo(memoryStream);

                // Set the position to the beginning of the memory stream
                memoryStream.Position = 1;

                // Load the PDF document from the memory stream
                pdfviewer1.Load(memoryStream);

                int lastPageNumber = bookRepository.GetLastPageNumberForUser(selectedBook,tableName);

                // Set the PDF viewer's current page to the last page number
                pdfviewer1.GotoPage(lastPageNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}");
            }
        }

        private async Task BookMark()
        {
            int pageNumber = pdfviewer1.CurrentPageIndex;
            await bookRepository.UpdateBookmarkAttributesAsync(selectedBook, pageNumber, tableName);
        }

        private async void BookMark_Click(object sender, RoutedEventArgs e)
        {
            await BookMark();
        }

        private async void Closed_PdfViewerWindow(object sender, EventArgs e)
        {
            await BookMark();
            bookListWindow.BookTitles.Clear(); 
            await bookListWindow.loadDataAsync(selectedBook.UserName); 
            this.Close();
            bookListWindow.Show();
        }
    }
}
