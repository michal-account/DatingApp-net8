using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        return Ok();
        // using var hmac = new HMACSHA512();

        // var user = new AppUser
        // {
        //     UserName = registerDto.Username.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // tutaj używam encoding.utf8.getbytes, bo chcemy zahaszować hasło i funkcja computeHash przyjmuje byte[], wiec potrzebujemy nasz string zmienic na bajty
        //     PasswordSalt = hmac.Key // używamy hmac.Key, aby zasolić hasło
        // };

        // context.Users.Add(user);
        // await context.SaveChangesAsync();

        // return new UserDto
        // {
        //     Username = user.UserName,
        //     Token = tokenService.CreateToken(user)
        // };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => 
            x.UserName == loginDto.Username.ToLower());
        
        if (user is null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt); // Problem w tym, że jeśli podczas logowania klucz byłby inny niż podczas rejestracji, to wynikowy hash również by się różnił i porównanie haseł zawsze by zawodziło.

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); 

        for(int i = 0; i < computedHash.Length; i++ ) // robimy pętle for bo jest to tablica bajtów i porównujemy po prostu te hashe
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password"); // tutaj porównujemy po kolei bajty 
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }


    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // Bob != bob
    }
}
