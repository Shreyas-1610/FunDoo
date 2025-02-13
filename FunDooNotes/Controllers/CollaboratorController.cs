using CommonLayer.Models;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorManager manager;
        public CollaboratorController(ICollaboratorManager manager)
        {
            this.manager = manager;
        }

        [Authorize]
        [HttpPost("AddCollaborator")]
        public IActionResult AddCollab(CollaboratorModel model)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var addCollaborator = manager.CreateCollaborator(UserId, model);
                return Ok(new ResponseModel<Collaborator> { Success = true, Message = "Add collaborator", Data = addCollaborator });
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.Message });
            }
        }

        [Authorize]
        [HttpGet("GetAllCollaborators")]
        public IActionResult GetAll()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var allCollabs = manager.GetCollaborators(UserId);
                return Ok(new ResponseModel<List<Collaborator>> { Success = true, Message = "All collaborators:", Data = allCollabs });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.Message });
            }
        }

        [Authorize]
        [HttpDelete("DeleteCollaborator")]
        public IActionResult DeleteCollaborator(int CollaboratorId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);

                bool isDeleted = manager.DeleteCollaborator(UserId, CollaboratorId);

                if (isDeleted)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Collaborator removed",Data = isDeleted });
                }
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Collaborator not found or unauthorized action" });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.Message });
            }
        }
    }
}
