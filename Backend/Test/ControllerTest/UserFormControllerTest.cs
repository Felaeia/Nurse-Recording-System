using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.IUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using NurseRecordingSystem.Controllers;
using NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs;
using Xunit;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class UserFormControllerTest
    {
        private readonly Mock<ICreateUserForm> _mockCreateUserFormService;
        private readonly Mock<IUpdateUserForm> _mockUpdateUserFormService;
        private readonly Mock<IDeleteUserForm> _mockDeleteUserFormService;
        private readonly UserFormController _userFormController;

        public UserFormControllerTest()
        {
            _mockCreateUserFormService = new Mock<ICreateUserForm>();
            _mockUpdateUserFormService = new Mock<IUpdateUserForm>();
            _mockDeleteUserFormService = new Mock<IDeleteUserForm>();
            _userFormController = new UserFormController(
                _mockCreateUserFormService.Object,
                _mockDeleteUserFormService.Object,
                _mockUpdateUserFormService.Object);
        }

        #region CreateForm Tests
        [Fact]
        public async Task CreateForm_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new UserFormRequestDTO
            {
                issueType = "Medical Issue",
                issueDescryption = "Description",
                status = "Active",
                patientName = "John Doe",
                createdBy = "Nurse1",
                updatedBy = "Nurse1",
                DeletedBy = "Nurse1"
            };
            var userId = "123";
            var creator = "Nurse1";
            var expectedResponse = new UserFormResponseDTO
            {
                IsSuccess = true,
                UserFormId = 1,
                Message = "Form created successfully"
            };

            _mockCreateUserFormService.Setup(s => s.CreateUserFormAsync(request, userId, creator))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userFormController.CreateForm(request, userId, creator) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            var response = result.Value as UserFormResponseDTO;
            Assert.NotNull(response);
            Assert.Equal(expectedResponse.UserFormId, response.UserFormId);
        }

        [Fact]
        public async Task CreateForm_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var request = new UserFormRequestDTO(); // Missing required fields
            var userId = "123";
            var creator = "Nurse1";

            _userFormController.ModelState.AddModelError("issueType", "Required");

            // Act
            var result = await _userFormController.CreateForm(request, userId, creator) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateForm_ArgumentNullException_ReturnsBadRequest()
        {
            // Arrange
            var request = new UserFormRequestDTO
            {
                issueType = "Medical Issue",
                status = "Active",
                patientName = "John Doe",
                createdBy = "Nurse1",
                updatedBy = "Nurse1",
                DeletedBy = "Nurse1"
            };
            var userId = "123";
            var creator = "Nurse1";

            _mockCreateUserFormService.Setup(s => s.CreateUserFormAsync(request, userId, creator))
                .ThrowsAsync(new ArgumentNullException("Some parameter is null"));

            // Act
            var result = await _userFormController.CreateForm(request, userId, creator) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateForm_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var request = new UserFormRequestDTO
            {
                issueType = "Medical Issue",
                status = "Active",
                patientName = "John Doe",
                createdBy = "Nurse1",
                updatedBy = "Nurse1",
                DeletedBy = "Nurse1"
            };
            var userId = "123";
            var creator = "Nurse1";

            _mockCreateUserFormService.Setup(s => s.CreateUserFormAsync(request, userId, creator))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userFormController.CreateForm(request, userId, creator) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }
        #endregion

        #region UpdateUserForm Tests
        [Fact]
        public async Task UpdateUserForm_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new UpdateUserFormRequestDTO
            {
                formId = 1,
                issueType = "Updated Issue",
                status = "Updated",
                patientName = "John Doe"
            };
            var updatedBy = "Nurse1";
            var expectedResponse = new UserFormResponseDTO
            {
                IsSuccess = true,
                UserFormId = 1,
                Message = "Form updated successfully"
            };

            _mockUpdateUserFormService.Setup(s => s.UpdateUserFormAsync(request, updatedBy))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userFormController.UpdateUserForm(request, updatedBy) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as UserFormResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUserForm_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            UpdateUserFormRequestDTO request = null;
            var updatedBy = "Nurse1";

            // Act
            var result = await _userFormController.UpdateUserForm(request, updatedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserForm_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var request = new UpdateUserFormRequestDTO(); // Missing required
            var updatedBy = "Nurse1";

            _userFormController.ModelState.AddModelError("formId", "Required");

            // Act
            var result = await _userFormController.UpdateUserForm(request, updatedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserForm_ArgumentNullException_ReturnsBadRequest()
        {
            // Arrange
            var request = new UpdateUserFormRequestDTO
            {
                formId = 1,
                issueType = "Issue",
                status = "Active",
                patientName = "John Doe"
            };
            var updatedBy = "Nurse1";

            _mockUpdateUserFormService.Setup(s => s.UpdateUserFormAsync(request, updatedBy))
                .ThrowsAsync(new ArgumentNullException("Parameter null"));

            // Act
            var result = await _userFormController.UpdateUserForm(request, updatedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserForm_NotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateUserFormRequestDTO
            {
                formId = 1,
                issueType = "Issue",
                status = "Active",
                patientName = "John Doe"
            };
            var updatedBy = "Nurse1";

            _mockUpdateUserFormService.Setup(s => s.UpdateUserFormAsync(request, updatedBy))
                .ThrowsAsync(new Exception("Form not found or is already deleted"));

            // Act
            var result = await _userFormController.UpdateUserForm(request, updatedBy) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserForm_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var request = new UpdateUserFormRequestDTO
            {
                formId = 1,
                issueType = "Issue",
                status = "Active",
                patientName = "John Doe"
            };
            var updatedBy = "Nurse1";

            _mockUpdateUserFormService.Setup(s => s.UpdateUserFormAsync(request, updatedBy))
                .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _userFormController.UpdateUserForm(request, updatedBy) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }
        #endregion

        #region DeleteUserForm Tests
        [Fact]
        public async Task DeleteUserForm_ValidRequest_ReturnsOk()
        {
            // Arrange
            int formId = 1;
            string deletedBy = "Nurse1";

            _mockDeleteUserFormService.Setup(s => s.DeleteUserFormAsync(formId, deletedBy))
                .ReturnsAsync(true);

            // Act
            var result = await _userFormController.DeleteUserForm(formId, deletedBy) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains("successfully deleted", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteUserForm_InvalidFormId_ReturnsBadRequest()
        {
            // Arrange
            int formId = 0;
            string deletedBy = "Nurse1";

            // Act
            var result = await _userFormController.DeleteUserForm(formId, deletedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUserForm_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            int formId = 1;
            string deletedBy = null; // or "Unknown"

            // Act
            var result = await _userFormController.DeleteUserForm(formId, deletedBy) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUserForm_NotFound_ReturnsNotFound()
        {
            // Arrange
            int formId = 1;
            string deletedBy = "Nurse1";

            _mockDeleteUserFormService.Setup(s => s.DeleteUserFormAsync(formId, deletedBy))
                .ThrowsAsync(new Exception("Form not found or is already deleted"));

            // Act
            var result = await _userFormController.DeleteUserForm(formId, deletedBy) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUserForm_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int formId = 1;
            string deletedBy = "Nurse1";

            _mockDeleteUserFormService.Setup(s => s.DeleteUserFormAsync(formId, deletedBy))
                .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _userFormController.DeleteUserForm(formId, deletedBy) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }
        #endregion
    }
}
