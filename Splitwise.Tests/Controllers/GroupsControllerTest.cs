using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Controllers;
using Splitwise.Model;
using Splitwise.Models;
using Splitwise.Models.Enums;
using Splitwise.Service;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace Splitwise.Tests.Controllers
{
    [TestClass]
    public class GroupsControllerTest
    {
        private GroupsController controller;

        [TestMethod]
        public void PostGroup_CorrectGroup_ReturnsId()
        {
            var fakeGroup = new Group
            {
                Name = "MyGroup",
                Category = GroupCategory.Friends,
                Users = new List<User>()
                {
                    new User { Username = "User1" }
                },
                CurrentBalance = 0
            };

            var fakeSaveResult = new SaveResultModel<Group>
            {
                Model = fakeGroup,
                Success = true
            };

            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.CreateGroup(It.IsAny<Group>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object);

            var result = controller.Post(fakeGroup);

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Group>));
        }

        [TestMethod]
        public void PostGroup_NullGroup_ReturnsBadRequest()
        {
            var fakeSaveResult = new SaveResultModel<Group>
            {
                Model = null,
                Success = false
            };

            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.CreateGroup(It.IsAny<Group>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object);

            var result = controller.Post(null);

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
