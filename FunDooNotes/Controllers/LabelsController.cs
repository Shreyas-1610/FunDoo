using CommonLayer.Models;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Context;
using Repository.Entity;
using System;
using System.Collections.Generic;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        
        private readonly ILabelsManager manager;

        public LabelsController(ILabelsManager manager)
        {
            this.manager = manager;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateLabel")]
        public IActionResult CreateLabel(LabelsModel model)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var create = manager.CreateLabel(UserId, model);
                return Ok(new ResponseModel<Labels> { Success = true, Message = "Created label", Data = create });
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Exception", Data = e.Message });
            }
        }

        [Authorize]
        [HttpGet("GetLabelById")]
        public IActionResult GetLabel(int LabelId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var getLabel = manager.GetLabelById(LabelId, UserId);
                return Ok(new ResponseModel<Labels> { Success = true, Message = "Label info", Data = getLabel });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = true, Message="Exception",Data = e.Message});
            }
        }

        [Authorize]
        [HttpGet("GetAllLabels")]
        public IActionResult GetAll()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var getAll = manager.GetAllLabels(UserId);
                return Ok(new ResponseModel<List<Labels>> { Success = true, Message = "All labels", Data = getAll });
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = true, Message = "Exception", Data = e.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateLabel")]
        public IActionResult UpdateLabel(int LabelId, string LabelName)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var updateLabel = manager.UpdateLabel(LabelName, LabelId, UserId);
                if (updateLabel)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "updated label", Data = updateLabel });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = true, Message = "No label found" });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = true, Message = "Exception", Data = e.Message });
            }
        }

        [Authorize]
        [HttpDelete("DeleteLabel")]
        public IActionResult DeleteLabel(int LabelId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                var deleteLabel = manager.DeleteLabel(LabelId, UserId);
                if (deleteLabel) 
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Label deleted successfully", Data = deleteLabel });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = true, Message = "No label found" });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = true, Message = "Exception", Data = e.Message });
            }
        }
    }
}
