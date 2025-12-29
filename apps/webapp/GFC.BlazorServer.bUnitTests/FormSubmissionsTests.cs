using Bunit;
using GFC.BlazorServer.Components.Pages.Admin;
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace GFC.BlazorServer.bUnitTests
{
    public class FormSubmissionsTests : TestContext
    {
        [Fact]
        public void FormSubmissions_RendersCorrectly_WithForms()
        {
            // Arrange
            var forms = new List<Form>
            {
                new Form { Id = 1, Name = "Form 1" },
                new Form { Id = 2, Name = "Form 2" }
            };

            var formServiceMock = new Mock<IFormService>();
            formServiceMock.Setup(s => s.GetAllFormsAsync()).ReturnsAsync(forms);
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            // Act
            var cut = RenderComponent<FormSubmissions>();

            // Assert
            cut.Find("h3").TextContent.Should().Be("Form Submissions");
            cut.FindAll("option").Count.Should().Be(3); // Includes "-- Select a form --"
        }

        [Fact]
        public void FormSubmissions_SelectingAForm_DisplaysSubmissions()
        {
            // Arrange
            var forms = new List<Form> { new Form { Id = 1, Name = "Test Form" } };
            var submissions = new List<FormSubmission>
            {
                new FormSubmission { Id = 1, FormId = 1, SubmittedAt = DateTime.Now, SubmissionData = "{}", Status = "Pending" }
            };

            var formServiceMock = new Mock<IFormService>();
            formServiceMock.Setup(s => s.GetAllFormsAsync()).ReturnsAsync(forms);
            formServiceMock.Setup(s => s.GetSubmissionsByFormIdAsync(1)).ReturnsAsync(submissions);
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            var cut = RenderComponent<FormSubmissions>();

            // Act
            cut.Find("select").Change("1");

            // Assert
            cut.Find("table").Should().NotBeNull();
            cut.FindAll("tbody tr").Count.Should().Be(1);
        }

        [Fact]
        public void FormSubmissions_ApproveButton_CallsUpdateServiceMethod()
        {
            // Arrange
            var forms = new List<Form> { new Form { Id = 1, Name = "Test Form" } };
            var submissions = new List<FormSubmission>
            {
                new FormSubmission { Id = 1, FormId = 1, SubmittedAt = DateTime.Now, SubmissionData = "{}", Status = "Pending" }
            };

            var formServiceMock = new Mock<IFormService>();
            formServiceMock.Setup(s => s.GetAllFormsAsync()).ReturnsAsync(forms);
            formServiceMock.Setup(s => s.GetSubmissionsByFormIdAsync(1)).ReturnsAsync(submissions);
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            var cut = RenderComponent<FormSubmissions>();
            cut.Find("select").Change("1");

            // Act
            cut.Find("button.btn-success").Click();

            // Assert
            formServiceMock.Verify(s => s.UpdateSubmissionStatusAsync(1, "Approved"), Times.Once);
        }
    }
}
