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
    public class NotesController : ControllerBase
    {
        private readonly INotesManager manager;

        public NotesController(INotesManager manager)
        {
            this.manager = manager;
        }
        [Authorize]
        [HttpPost]
        [Route("CreateNote")]
        public IActionResult CreateNote(NotesModel model)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var notes = manager.CreateNote(UserId, model);
                if (notes != null)
                {
                    return Ok(new ResponseModel<Notes> { Success = true, Message = "New note created", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Unsuccessful", Data = "Not in dB"});
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("GetAllNotes")]
        public ActionResult GetAllNotes()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var getAll = manager.GetAllNotes(UserId);

                return Ok(new ResponseModel<List<Notes>> { Success = true, Message = "All notes of the user:", Data = getAll });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "No notes found", Data = e.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateNote")]
        public ActionResult UpdateNote(int NotesId, UpdateNoteModel updateNoteModel)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var updateNote = manager.UpdateNote(NotesId, UserId, updateNoteModel);
                if (updateNote)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Update successful", Data = updateNote });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = true, Message = "Update not required", Data = updateNote });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteNote")]
        public IActionResult DeleteNote(int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var deleteNode = manager.DeleteNote(NotesId, UserId);
                if (deleteNode)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Note deleted successfully", Data = deleteNode });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note not found", Data = deleteNode});
                }
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.Message });
            }
        }
    }
}
