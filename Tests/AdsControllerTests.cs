using Microsoft.AspNetCore.Mvc;
using Projekt_ASP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AdsControllerTests
    {
        [Fact]
        public void Create_Get_ReturnsView()
        {
            var controller = new AdsController(null);
            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async void Details_NullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new AdsController(null);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
