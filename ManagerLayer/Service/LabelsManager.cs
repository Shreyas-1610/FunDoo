using CommonLayer.Models;
using ManagerLayer.Interface;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Service
{
    public class LabelsManager : ILabelsManager
    {
        private readonly ILabelsRepository repository;

        public LabelsManager(ILabelsRepository repository)
        {
            this.repository = repository;
        }

        public Labels CreateLabel(int UserId, LabelsModel model)
        {
           return repository.CreateLabel(UserId, model); 
        }
        public Labels GetLabelById(int LabelId, int UserId)
        {
            return repository.GetLabelById(LabelId, UserId);
        }

        public List<Labels> GetAllLabels(int UserId)
        {
            return repository.GetAllLabels(UserId);
        }
        public bool UpdateLabel(string LabelName, int LabelId, int UserId)
        {
            return repository.UpdateLabel(LabelName, LabelId, UserId);
        }

        public bool DeleteLabel(int LabelId, int UserId)
        {
            return repository.DeleteLabel(LabelId, UserId);
        }
    }
}
