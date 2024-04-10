using Common.HotelReservationsManager.Constants;
using HotelReservationsManager.Common;
using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.DB;
using HotelReservationsManager.Models.Requests;
using HotelReservationsManager.Models.Response;
using HotelReservationsManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationsManager.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly HotelDbContext _dbContext;
        private readonly JwtService _jwtService;
        public AdminController(
            HotelDbContext dbContext,
            JwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        [HttpPost]
        public ActionResult CreateUser(AdminCreateUserRequest request)
        {
            //additional check with the db
            int adminId = _jwtService.GetUserIdFromToken(User);
            var adminUser = _dbContext.Users.Find(adminId);

            if (adminUser == null)
            {User
            if (adminUser.Role != Role.Admin)
            {
                return Unauthorized();
            }

            if (!RegexValidation.IsUserNameValid(request.UserName))
            {
                return BadRequest("Incorrect Username");
            }

            if (!RegexValidation.IsPasswordValid(request.Password))
            {
                return BadRequest("Incorrect Password, need to have special symbol, digit, uppercase letter");
            }Userquest.LastName.Length > AppConstants.NameMaxLength)
            {
                return BadRequest("Invalid Name lenght!");
            }

            if (request.MiddleName != null)
            {
                if (request.MiddleName.Length < AppConstants.NameMinLength
                || request.MiddleName.Length > AppConstants.NameMaxLength)
                {
                    return BadRequest("Invalid Name lenght!");
                }
            }User
                return BadRequest("Invalid EGN lenght!");
            }

            if (request.Phone.Length > AppConstants.PhoneMaxLenght)
            {
                return BadRequest("Invalid Phone lenght!");
            }

            if (!RegexValidation.IsEmailValid(request.Email))
            {
                return BadRequest("Invalid Email!");
            }

            if (request.EndDate <= DateTime.Now)
            {
                return BadRequest("Invalid end date!");
            }

            User user = new User()
            {
                UserName = request.UserName,
                Password = request.Password,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                EGN = request.EGN,
                Phone = request.Phone,
                Email = request.Email,
                HireDate = DateTime.Now,
                IsActive = true,
                EndDate = request.EndDate,
                Role = Role.Normal
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteUser(AdminDeleteUserRequest request)
        {
            int adminId = _jwtService.GetUserIdFromToken(User);
            var adminUser = _dbContext.Users.Find(adminId);

            if (adminUser == null)
            {
                return Unauthorized();
            }

            if (adminUser.Role != Role.Admin)
            {
                return Unauthorized();
            }

            var userForDelete = _dbContext.Users.Find(request.Id);

            if (userForDelete == null)
            {
                return BadRequest("Invalid Id!");
            }

            _dbContext.Users.Remove(userForDelete);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public ActionResult<AdminSearchForUserResponse> SearchForUser(AdminSearchForUserRequest request)
        {
            int adminId = _jwtService.GetUserIdFromToken(User);
            var adminUser = _dbContext.Users.Find(adminId);

            if (adminUser == null)
            {
                return Unauthorized();
            }

            if (adminUser.Role != Role.Admin)
            {
                return Unauthorized();
            }

            var user = _dbContext.Users
                .FirstOrDefault(u => u.UserName == request.UserName);

            if (user == null)
            {
                return BadRequest("Not found!");
            }

            int userReservationsCount = _dbContext.Reservations
                .Where(r => r.User.Id == user.Id)
                .Count();

            int userReservationsForThisYear = _dbContext.Reservations
                .Where(r => r.User.Id == user.Id && r.StartDate.Year == DateTime.Now.Year)
                .Count();

            decimal userReservationsTotalPrice = _dbContext.Reservations
                .Where(r => r.User.Id == user.Id)
                .Sum(r => r.TotalPrice);

            AdminSearchForUserResponse response = new AdminSearchForUserResponse()
            {
                UserName = request.UserName,
                Email = user.Email,
                HireDate = user.HireDate,
                EndDate = user.EndDate,
                Role = user.Role == Role.Normal ? "Normal" : "Admin",
                IsActive = user.IsActive,
                ReservationsCount = userReservationsCount,
                ReservationsCountForThisYear = userReservationsForThisYear,
                ReservationsTotalPrice = userReservationsTotalPrice
            };

            return Ok(response);
        }

        [HttpGet]
        public ActionResult<List<AdminSearchForUserResponse>> GetAllEmployees(AdminGetAllEmployeesRequest request)
        {
            int adminId = _jwtService.GetUserIdFromToken(User);
            var adminUser = _dbContext.Users.Find(adminId);

            if (adminUser == null)
            {
                return Unauthorized();
            }

            if (adminUser.Role != Role.Admin)
            {
                return Unauthorized();
            }

            //var employees = _dbContext.Users
            //    .Skip(request.PageNumber * request.EmployeesCountPerPage)
            //    .Take(request.EmployeesCountPerPage)
            //    .ToList();


            var employees = _dbContext.Users
                .Skip(request.DisplayedCount)
                .Take(request.EmployeesCountPerPage)
                .ToList();
        }
    }
}
