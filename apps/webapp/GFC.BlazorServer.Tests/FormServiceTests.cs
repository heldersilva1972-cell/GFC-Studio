using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace GFC.BlazorServer.Tests
{
    public class FormServiceTests
    {
        private readonly DbContextOptions<GfcDbContext> _dbContextOptions;

        public FormServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<GfcDbContext>()
                .UseInMemoryDatabase(databaseName: "GfcTestDb")
                .Options;
        }

        private GfcDbContext CreateContext() => new GfcDbContext(_dbContextOptions);

        [Fact]
        public async Task CreateFormAsync_ShouldAddFormToDatabase()
        {
            // Arrange
            await using var context = CreateContext();
            var service = new FormService(context);
            var form = new Form { Name = "Test Form" };

            // Act
            await service.CreateFormAsync(form);

            // Assert
            var forms = await context.Forms.ToListAsync();
            forms.Should().ContainSingle();
            forms[0].Name.Should().Be("Test Form");
        }

        [Fact]
        public async Task GetFormByIdAsync_ShouldReturnCorrectForm()
        {
            // Arrange
            await using var context = CreateContext();
            var form = new Form { Name = "Test Form" };
            context.Forms.Add(form);
            await context.SaveChangesAsync();
            var service = new FormService(context);

            // Act
            var result = await service.GetFormByIdAsync(form.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Form");
        }

        [Fact]
        public async Task UpdateFormAsync_ShouldUpdateFormInDatabase()
        {
            // Arrange
            await using var context = CreateContext();
            var form = new Form { Name = "Original Name" };
            context.Forms.Add(form);
            await context.SaveChangesAsync();
            var service = new FormService(context);

            // Act
            form.Name = "Updated Name";
            await service.UpdateFormAsync(form);

            // Assert
            var updatedForm = await context.Forms.FindAsync(form.Id);
            updatedForm.Name.Should().Be("Updated Name");
        }

        [Fact]
        public async Task DeleteFormAsync_ShouldRemoveFormFromDatabase()
        {
            // Arrange
            await using var context = CreateContext();
            var form = new Form { Name = "Test Form" };
            context.Forms.Add(form);
            await context.SaveChangesAsync();
            var service = new FormService(context);

            // Act
            await service.DeleteFormAsync(form.Id);

            // Assert
            var forms = await context.Forms.ToListAsync();
            forms.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveSubmissionAsync_ShouldAddSubmissionToDatabase()
        {
            // Arrange
            await using var context = CreateContext();
            var form = new Form { Name = "Test Form" };
            context.Forms.Add(form);
            await context.SaveChangesAsync();
            var service = new FormService(context);
            var submission = new FormSubmission { FormId = form.Id, SubmissionData = "{}" };

            // Act
            await service.SaveSubmissionAsync(submission);

            // Assert
            var submissions = await context.FormSubmissions.ToListAsync();
            submissions.Should().ContainSingle();
            submissions[0].FormId.Should().Be(form.Id);
        }

        [Fact]
        public async Task GetSubmissionsByFormIdAsync_ShouldReturnCorrectSubmissions()
        {
            // Arrange
            await using var context = CreateContext();
            var form = new Form { Name = "Test Form" };
            context.Forms.Add(form);
            await context.SaveChangesAsync();
            var submission1 = new FormSubmission { FormId = form.Id, SubmissionData = "{}" };
            var submission2 = new FormSubmission { FormId = form.Id, SubmissionData = "{}" };
            context.FormSubmissions.AddRange(submission1, submission2);
            await context.SaveChangesAsync();
            var service = new FormService(context);

            // Act
            var result = await service.GetSubmissionsByFormIdAsync(form.Id);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateSubmissionStatusAsync_ShouldUpdateStatusInDatabase()
        {
            // Arrange
            await using var context = CreateContext();
            var form = new Form { Name = "Test Form" };
            context.Forms.Add(form);
            await context.SaveChangesAsync();
            var submission = new FormSubmission { FormId = form.Id, SubmissionData = "{}", Status = "Pending" };
            context.FormSubmissions.Add(submission);
            await context.SaveChangesAsync();
            var service = new FormService(context);

            // Act
            await service.UpdateSubmissionStatusAsync(submission.Id, "Approved");

            // Assert
            var updatedSubmission = await context.FormSubmissions.FindAsync(submission.Id);
            updatedSubmission.Status.Should().Be("Approved");
        }
    }
}
