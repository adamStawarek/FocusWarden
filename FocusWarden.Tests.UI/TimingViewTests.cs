using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FocusWarden.Tests.UI.Reporting;
using FocusWarden.Tests.UI.Reporting.Interfaces;
using FocusWarden.Tests.UI.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FocusWarden.Tests.UI
{
    [TestClass]
    public class TimingViewTests
    {
        private IReportHelper reportHelper = ReportHelper.GetInstance();

        private FlaUI.Core.Application application;
        private UIA3Automation automation;
        private Window window;

        public TestContext TestContext { get; set; } // Set automatically by MSTest

        [TestInitialize]
        public void Init()
        {
            application = FlaUI.Core.Application.Launch(Configuration.ApplicationPath);
            automation = new UIA3Automation();
            window = application.GetMainWindow(automation);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            var screenshot = FlaUI.Core.Capturing.Capture.MainScreen();
            var testResult = new Reporting.Models.TestResult()
            {
                Attachment = screenshot.BitmapImage,
                Outcome = TestContext.CurrentTestOutcome,
                TestCase = TestContext.TestName,
                Exceptions = Array.Empty<string>(),
                Logs = Array.Empty<string>(),
                TestSuite = TestContext.FullyQualifiedTestClassName.Split('.').Last(), //remove namespace part
                Priority = ((PriorityAttribute)this.GetType().GetMethod(TestContext.TestName).GetCustomAttributes(typeof(PriorityAttribute), true)[0]).Priority
            };
            reportHelper.AddResult(testResult);

            var min = this.GetType()
                .GetMethods()
                .Select(m => m.GetCustomAttributes(typeof(PriorityAttribute), true))
                .Where(m => m.Any())
                .Select(m => m.First())
                .Cast<PriorityAttribute>()
                .Min(m => m.Priority);

            if (min == testResult.Priority)
            {
                await reportHelper.PublishAsync();
                reportHelper.ClearResults();
            }

            application.Close();
            automation.Dispose();
        }

        [TestMethod, Priority(1)]
        public async Task Should_Change_Time_After_Start_Session()
        {
            var timingView = window.FindFirstDescendant(c => c.ByClassName("TimerViewControl"));
            var timeLabel = timingView.FindFirstChild(c => c.ByAutomationId("TimeLabel")).AsLabel();
            var originalTime = timeLabel.Text;

            var startBtn = timingView.FindFirstChild(c => c.ByAutomationId("StartButton")).AsButton();
            startBtn.Click();

            await Task.Delay(1000);
           
            Assert.AreNotEqual(originalTime, timeLabel.Text);

            var stopBtn = timingView.FindFirstChild(c => c.ByAutomationId("StopButton")).AsButton();
            stopBtn.Click();

            Assert.AreEqual(originalTime, timeLabel.Text);
        }

        [TestMethod, Priority(2)]
        public async Task Should_Add_New_TODO_Item()
        {
            var timingView = window.FindFirstDescendant(c => c.ByClassName("TaskListControl"));

            var addTododBtn = timingView.FindFirstChild(c => c.ByAutomationId("AddTodoItemButton")).AsButton();
            addTododBtn.Click();

            await Task.Delay(500);

            var addTododTextBox = window.FindFirstDescendant(c => c.ByAutomationId("AddTodoItemTextBox")).AsTextBox();
            addTododTextBox.Text = "test";

            var saveTodoBtn = window.FindFirstDescendant(c => c.ByAutomationId("SaveTodoItemButton")).AsButton();
            saveTodoBtn.Click();

            await Task.Delay(500);

            var addedTodoItem = window.FindAllDescendants(c => c.ByName("test"));
            Assert.IsNotNull(addedTodoItem);
        }
    }
}
