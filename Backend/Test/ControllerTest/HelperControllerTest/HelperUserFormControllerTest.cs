using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm;
using Xunit;
using NurseRecordingSystem.API.Controllers;

namespace NurseRecordingSystem.Test.ControllerTest.HelperControllerTest
{
    public class HelperUserFormControllerTest
    {
        private readonly Mock<IViewUserForm> _mockViewService;
        private readonly HelperUserFormController _controller;

        public HelperUserFormControllerTest()
        {
            _mockViewService = new Mock<IViewUserForm>();
            _controller = new HelperUserFormController(_mockViewService.Object);
        }

        [Fact]
        public async Task GetUserForm_SuccessfulView_ReturnsOk()
        {
            // Arrange
            var formId = 1;
            var expectedForm = new ViewUserFormResponseDTO
            {
                FormId = formId,
                IssueType = "Medical",
                IssueDescryption = "Patient has fever",
                Status = "Active",
                UserId = 123,
                PatientName = "John Doe",
                CreatedOn = DateTime.Now,
                CreatedBy = "Nurse1",
                UpdatedOn = DateTime.Now,
                UpdatedBy = "Nurse1",
                IsActive = true
            };
            _mockViewService.Setup(service => service.GetUserFormAsync(formId)).ReturnsAsync(expectedForm);

            // Act
            var result = await _controller.GetUserForm(formId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedForm, result.Value);
        }

        [Fact]
        public async Task GetUserForm_FormNotFound_ReturnsNotFound()
        {
            // Arrange
            var formId = 999;
            _mockViewService.Setup(service => service.GetUserFormAsync(formId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetUserForm(formId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains($"Patient form with ID {formId} was not found or is archived.", result.Value?.ToString());
        }

        [Fact]
        public async Task GetUserForm_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var formId = 1;
            _mockViewService.Setup(service => service.GetUserFormAsync(formId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetUserForm(formId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("An error occurred while retrieving the patient form details.", result.Value?.ToString());
        }
    }
}
