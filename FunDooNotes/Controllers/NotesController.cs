using CommonLayer.Models;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Repository.Context;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly FunDooDbContext context;

        public NotesController(INotesManager manager, IDistributedCache distributedCache, FunDooDbContext context)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;
            this.context = context;
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
        [HttpPut]
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

        [Authorize]
        [HttpPut]
        [Route("TogglePin")]
        public IActionResult CheckPin(int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var checkPin = manager.CheckPin(NotesId, UserId);
                if (checkPin)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Changed pin toggle", Data = checkPin });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note not found", Data = checkPin });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Note not found", Data = e.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ToggleArchive")]
        public IActionResult CheckArchive(int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var checkArchive = manager.CheckArchive(NotesId, UserId);
                if (checkArchive)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Changed archive toggle", Data = checkArchive });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note not found", Data = checkArchive });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Note not found", Data = e.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ToggleTrash")]
        public IActionResult CheckTrash(int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var checkTrash = manager.CheckTrash(NotesId, UserId);
                if (checkTrash)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Changed trash toggle", Data = checkTrash });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note not found", Data = checkTrash });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Note not found!!", Data = e.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateColor")]
        public IActionResult UpdateColor(int NotesId, string Color)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var color = manager.UpdateColor(NotesId, UserId, Color);
                return Ok(new ResponseModel<string> { Success = true, Message = "Color updated successfully", Data = color});
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = true, Message = "Note not found", Data = e.Message});
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateImage")]
        public IActionResult UpdateImage(int NotesId, string Image)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var updateImage = manager.UpdateImage(NotesId, UserId, Image);
                if (updateImage)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Image updated", Data = updateImage });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Image not required", Data = updateImage });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateReminder")]
        public IActionResult UpdateReminder(int NotesId, DateTime Reminder)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var updateReminder = manager.UpdateReminder(NotesId, UserId, Reminder);
                if (updateReminder)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Image updated", Data = updateReminder });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Image not required", Data = updateReminder });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.Message });
            }
        }

        [HttpGet("RedisGetAll")]
        public async Task<IActionResult> GetAllNotesUsingRedis()
        {
            var cacheKey = "NotesList";
            string SerializedNotesLst;
            var NotesList = new List<Notes>();
            var RedisNotesList = await distributedCache.GetAsync(cacheKey);
            if (RedisNotesList != null)
            {
                SerializedNotesLst = Encoding.UTF8.GetString(RedisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<Notes>>(SerializedNotesLst);
            }
            else
            {
                NotesList = context.Notes.ToList();
                SerializedNotesLst = JsonConvert.SerializeObject(NotesList);
                RedisNotesList = Encoding.UTF8.GetBytes(SerializedNotesLst);
                var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                await distributedCache.SetAsync(cacheKey, RedisNotesList, options);
            }
            return Ok(NotesList);
        }
    }
}
