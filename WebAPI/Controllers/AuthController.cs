﻿using Business.Abstract;
using Entities.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var result=await _authService.RegisterAsync(model);             
            if (result.Success)
                   return Ok(result);
            return BadRequest(result);
            
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);
            if (result.Success)
                return Ok(result);

            return BadRequest();
        }
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var result = await _authService.RefreshTokenLoginAsync(refreshTokenDTO.RefreshToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> LogOut()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authService.LogOut(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

      
    }
}
