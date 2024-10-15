using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;


        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("login")]

        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDTO.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Email!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username or Password is incorrect");
            }

            return Ok(
                new CreatedUserDTO
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }



        [HttpPost("register")]

        public async Task<ActionResult> Register([FromBody] RegisterDTO reqDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var AppUser = new AppUser
                {
                    UserName = reqDTO.Username,
                    Email = reqDTO.Email,
                };

#pragma warning disable CS8604 // Possible null reference argument.
                var createdUser = await _userManager.CreateAsync(AppUser, reqDTO.Password);
#pragma warning restore CS8604 // Possible null reference argument.

                if (createdUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(AppUser, "User");
                    if (role.Succeeded)
                    {
                        return Ok(
                            new CreatedUserDTO
                            {
                                Username = AppUser.UserName,
                                Email = AppUser.Email,
                                Token = _tokenService.CreateToken(AppUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, role.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}