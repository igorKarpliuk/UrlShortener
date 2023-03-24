using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.Core.Models.Identity;
using UrlShortener.Data.ViewModels;
using UrlShortener.Web.Controllers;

namespace UrlShortener.Tests
{
    public class LoginControllerTests
    {
        //[Fact]
        //public void LoginReturnsViewWithModelIfModelIsNotValid()
        //{
        //    var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        //    var contextAccessor = new Mock<IHttpContextAccessor>();
        //    var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        //    var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);
        //    var mockRoleManager = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(),
        //null, null, null, null);
        //    var controller = new LoginController(mockSignInManager.Object, mockUserManager.Object,
        //        mockRoleManager.Object);
        //    controller.ModelState.AddModelError("Login", "Required");
        //    LoginViewModel user = new LoginViewModel() { Login = "", Password = "dd" };

        //    var result = controller.Login(user);
        //    Assert.IsType<ViewResult>(result);
        //}
        [Fact]
        public void LoginRedirectsIfLoginIsSuccessful()
        {
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockUserStore, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);
            var mockRoleManager = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(),
        null, null, null, null);
            var controller = new LoginController(mockSignInManager.Object, mockUserManager.Object,
                mockRoleManager.Object);
            mockUserStore.Setup(x => x.CreateAsync(new User() { UserName = "test" }))
                .Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Object.CreateAsync();
            LoginViewModel user = new LoginViewModel() { Login = "test", Password = "1234Test@" };

            var result = controller.Login(user);

            Assert.IsType<RedirectResult>(result);
        }
    }
}