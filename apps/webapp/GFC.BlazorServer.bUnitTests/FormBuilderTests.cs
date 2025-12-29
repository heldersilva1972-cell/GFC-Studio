using Bunit;
using GFC.BlazorServer.Components.Pages.Admin;
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace GFC.BlazorServer.bUnitTests
{
    public class FormBuilderTests : TestContext
    {
        [Fact]
        public void FormBuilder_RendersCorrectly_WithExistingForm()
        {
            // Arrange
            var form = new Form
            {
                Id = 1,
                Name = "Test Form",
                Description = "Test Description",
                FormFields = new List<FormField>
                {
                    new FormField { Id = 1, Label = "Field 1", Order = 1 },
                    new FormField { Id = 2, Label = "Field 2", Order = 2 }
                }
            };

            var formServiceMock = new Mock<IFormService>();
            formServiceMock.Setup(s => s.GetFormByIdAsync(1)).ReturnsAsync(form);
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            // Act
            var cut = RenderComponent<FormBuilder>(parameters => parameters
                .Add(p => p.FormId, 1));

            // Assert
            cut.Find("h3").TextContent.Should().Be("Form Builder");
            cut.Find("input[value='Test Form']").Should().NotBeNull();
            cut.Find("textarea").TextContent.Should().Be("Test Description");
            cut.FindAll(".card").Count.Should().Be(2);
        }

        [Fact]
        public void FormBuilder_AddFieldButton_AddsNewField()
        {
            // Arrange
            var formServiceMock = new Mock<IFormService>();
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            var cut = RenderComponent<FormBuilder>();

            // Act
            cut.Find("button.btn-primary").Click();

            // Assert
            cut.FindAll(".card").Count.Should().Be(1);
        }

        [Fact]
        public void FormBuilder_RemoveFieldButton_RemovesField()
        {
            // Arrange
            var formServiceMock = new Mock<IFormService>();
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            var cut = RenderComponent<FormBuilder>();
            cut.Find("button.btn-primary").Click(); // Add a field

            // Act
            cut.Find("button.btn-danger").Click();

            // Assert
            cut.FindAll(".card").Count.Should().Be(0);
        }

        [Fact]
        public void FormBuilder_SaveButton_CallsUpdateServiceMethod_ForExistingForm()
        {
            // Arrange
            var form = new Form { Id = 1, Name = "Test Form" };
            var formServiceMock = new Mock<IFormService>();
            formServiceMock.Setup(s => s.GetFormByIdAsync(1)).ReturnsAsync(form);
            Services.AddSingleton<IFormService>(formServiceMock.Object);

            var cut = RenderComponent<FormBuilder>(parameters => parameters
                .Add(p => p.FormId, 1));

            // Act
            cut.Find("button.btn-success").Click();

            // Assert
            formServiceMock.Verify(s => s.UpdateFormAsync(It.IsAny<Form>()), Times.Once);
        }
    }
}
