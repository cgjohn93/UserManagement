using System;
using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    [BindProperty]
    public int RecordIdView { get; set; }



    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;


    [HttpGet("")]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("active")]
    public ViewResult ListActive()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.Where(x => x.IsActive == true).ToList()
        };

        return View("List", model);
    }

    [HttpGet("inactive")]
    public ViewResult ListNotActive()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.Where(x => x.IsActive == false).ToList()
        };

        return View("List", model);
    }

    [HttpGet("view")]
    public ViewResult ViewRecord(int id)
    {
        var items = _userService.GetAll();

        var filteredUser = items
            .Where(p => p.Id == id)
            .Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
            })
            .ToList();

        var model = new UserListViewModel
        {
            Items = filteredUser
        };

        return View("Edit", model);
    }

    public class FormData
    {
        public int Id { get; set; }
        public required string Forename { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Active { get; set; }
    }

    [HttpPost]
    public IActionResult Edit(FormData formData)
    {
        var user = _userService.GetAll().FirstOrDefault(p => p.Id == formData.Id);

        if (user != null)
        {
            user.Forename = formData.Forename;
            user.Surname = formData.Surname;
            user.Email = formData.Email;
            user.IsActive = formData.Active;
            user.DateOfBirth = formData.DateOfBirth;

            var items = _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
            });

            var model = new UserListViewModel
            {
                Items = items.ToList()
            };

            return View("List", model);
        }
        else
        {
            return NotFound();
        }

    }

    [HttpPost("view")]
    public IActionResult Delete(int id)
    {
        var user = _userService.GetAll().FirstOrDefault(p => p.Id == id);

        //I'm not sure what to do here to be honest, usually when using EFCore I would just do something along the lines of
        //context.Remove() but when the database is a local list I'm not actually sure how to mimic the same thing.


        if (user != null)
        {
            var items = _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
            });

            var model = new UserListViewModel
            {
                Items = items.ToList()
            };

            return View("List", model);
        }
        else
        {
            return NotFound();
        }

    }

}
