using iTextSharp.text;
using iTextSharp.text.pdf;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace HtmlToPdf48
{
    class Program
    {
        public static void ConvertToPdf(string url, string outputPath)
        {
            // Set up Selenium WebDriver
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run Chrome in headless mode (no GUI)
            using (var driver = new ChromeDriver(options))
            {
                // Navigate to the website
                driver.Navigate().GoToUrl(url);

                // Wait for the page to fully load
                System.Threading.Thread.Sleep(5000); // Adjust this time as needed

                // Capture the screenshot of the webpage
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                // Convert the screenshot to a byte array
                byte[] screenshotBytes = screenshot.AsByteArray;

                // Save the byte array to a file
                string screenshotPath = Path.Combine(Path.GetTempPath(), "screenshot.png");
                File.WriteAllBytes(screenshotPath, screenshotBytes);

                // Convert the image to PDF
                using (var document = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create)))
                    {
                        document.Open();
                        var image = Image.GetInstance(screenshotPath);
                        document.Add(image);
                        document.Close();
                    }
                }

                // Delete the temporary screenshot file
                File.Delete(screenshotPath);
            }
        }

        static void Main(string[] args)
        {
            // Example usage
            ConvertToPdf("https://www.msn.com/en-ph", "output.pdf");
        }
    }
}
