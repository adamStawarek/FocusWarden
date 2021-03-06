using FocusWarden.Tests.UI.Reporting.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TestResult = FocusWarden.Tests.UI.Reporting.Models.TestResult;

namespace FocusWarden.Tests.UI.Reporting
{
    public class ReportHelper : IReportHelper
    {
        private readonly IEmailHelper mailHelper;
        private readonly IFtpHelper ftpHelper;
        private List<TestResult> testResults;

        private static IReportHelper instance;
        public static IReportHelper GetInstance()
        {
            if (instance == null) instance = new ReportHelper();
            return instance;
        }

        private ReportHelper()
        {
            mailHelper = new EmailHelper();
            ftpHelper = new FtpHelper();
            testResults = new List<TestResult>();
        }

        public void AddResult(TestResult testResult) => testResults.Add(testResult);

        public void ClearResults() => testResults = new List<TestResult>();

        public async Task PublishAsync()
        {
            var testSuites = testResults.GroupBy(r => r.TestSuite);

            foreach (var testSuiteWithSteps in testSuites)
            {
                //TODO: Upload screenshoots to AzureBlob or somewhere else
                //await UploadScreenshoots(results: testSuiteWithSteps.ToList(), testSuite: testSuiteWithSteps.Key);

                string htmlContent = CreateHtml(results: testSuiteWithSteps.ToList(), testSuite: testSuiteWithSteps.Key);

                await mailHelper.SendEmailAsync("FocusWarden UI Tests", htmlContent, true);
            }
        }

        private async Task UploadScreenshoots(List<TestResult> results, string testSuite)
        {
            var counter = 1;
            var isDirectoryCreated = false;

            while (!isDirectoryCreated)
            {
                isDirectoryCreated = await ftpHelper.TryCreateDirectoryAsync($"{DateTime.Now:yyMMdd}_{testSuite}_{counter}");

                if (counter > 9 || isDirectoryCreated) break;

                counter++;
            }

            results.ToList().ForEach(r => this.SaveImage(r.Attachment, $"{r.Priority}_{r.TestCase}.png"));


            var files = results.ToList().Select(r => $"{r.Priority}_{r.TestCase}.png").ToArray();

            var urls = await ftpHelper.TryUploadFilesAsync(files, $"{DateTime.Now.ToString("yyMMdd")}_{testSuite}_{counter}");

            for (int i = 0; i < results.Count; i++)
            {
                results[i].ScreenshootUrl = urls[i];
            }

            files.ToList().ForEach(f => File.Delete(f));
        }

        private void SaveImage(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        private string CreateHtml(List<TestResult> results, string testSuite)
        {
            string table = $"<table>" +
                                "<tr>" +
                                    "<td><b>Priority</b></td> " +
                                    "<td><b>Step</b></td> " +
                                    "<td><b>Screenshoot</b></td> " +
                                    "<td><b>Exceptions</b></td> " +
                                    "<td><b>Logs</b></td> " +
                                "</tr>";

            foreach (var result in results.OrderBy(r => r.Priority))
            {
                table += $"<tr style='background: {(result.Outcome == UnitTestOutcome.Passed ? "lightgreen" : "pink")}'> " +
                    $"<td>{result.Priority}</td>" +
                    $"<td>{result.TestCase}</td>" +
                     $"<td><img src=\"{result.ScreenshootUrl}\" style='height: 100px'></td>" +
                    $"<td>{string.Join("<br>", result.Exceptions?.Distinct())}</td> " +
                    $"<td>{string.Join("<br>", result.Logs)}</td> " +
                    $"</tr>";
            }

            table += "</table>";

            var style = @"<style>
                                    body {
                                      font-family: 'lato', sans-serif;
                                      color: black;
                                    }
                                    table {
                                      font-family: arial, sans-serif;
                                      border-collapse: collapse;
                                      width: 100%;
                                      font-size: 14px;  
                                    }
                                    td, th {
                                      border: 1px solid #dddddd;
                                      text-align: left;
                                      padding: 8px;
                                    }
                                </style>";

            var htmlContent = $"<html>" +
                    $"<head>{style}</head>" +
                    $"<body>" +
                        $"<h2> {testSuite} <small> {results.Count(r => r.Outcome == UnitTestOutcome.Passed)} passed, {results.Count(r => r.Outcome == UnitTestOutcome.Failed)} failed </ small ><h2>" +
                        $"{table}" +
                    $"</body>" +
              $"</html>";

            return htmlContent;
        }
    }
}
