using System.Threading.Tasks;
using HabitTracker.Models.TokenAuth;
using HabitTracker.Web.Controllers;
using Shouldly;
using Xunit;

namespace HabitTracker.Web.Tests.Controllers
{
    public class HomeController_Tests: HabitTrackerWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}